using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentGeneration.Builders;
using DocumentGeneration.Elements;
using DocumentGeneration.Helpers;
using DocumentGeneration.Interfaces;

namespace DocumentGeneration
{
	public class DocumentBuilder : IDocumentBuilder
	{
		public readonly WordprocessingDocument Document;
		private readonly Body _body;
		private readonly MemoryStream _ms;

		public static DocumentBuilder CreateFromTemplate<TDocument>(MemoryStream stream, TDocument document)
		{
			var builder = new DocumentBuilder(stream);
			builder.PopulateTemplateWithDocument(document);
			return builder;
		}

		public DocumentBuilder()
		{
			_ms = new MemoryStream();
			Document = WordprocessingDocument.Create(_ms, WordprocessingDocumentType.Document);
			Document.AddMainDocumentPart();
			Document.MainDocumentPart.Document = new Document(new[] { new Body() });
			_body = Document.MainDocumentPart.Document.Body;
			SetCompatibilityMode();
			AddNumberingDefinitions();
			AppendSectionProperties();
		}

		private DocumentBuilder(MemoryStream ms)
		{
			_ms = ms;
			Document = WordprocessingDocument.Open(ms, true);
			_body = Document.MainDocumentPart.Document.Body;
			if (Document.MainDocumentPart.NumberingDefinitionsPart == null)
			{
				AddNumberingDefinitions();
			}
		}

		private void AddNumberingDefinitions()
		{
			var part = Document.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>();
			part.Numbering = new Numbering();
		}

		public void ReplacePlaceholderWithContent(string placeholderText, Action<DocumentBodyBuilder> action)
		{
			var paragraphs = _body.Descendants<Paragraph>().ToList();
			var placeholderElement =  paragraphs.First(element => element.InnerText == $"[{placeholderText}]");

			var builder = new DocumentBodyBuilder(Document, placeholderElement);
			action(builder);
			placeholderElement.Remove();
		}

		public void AddNumberedList(Action<IListBuilder> action)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddNumberedList(action);
		}

		public void AddBulletedList(Action<IListBuilder> action)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddBulletedList(action);
		}

		public void AddParagraph(Action<IParagraphBuilder> action)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddParagraph(action);
		}

		public void AddParagraph(string text)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddParagraph(text);
		}

		public void AddTable(Action<ITableBuilder> action)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddTable(action);
		}

		public void AddTable(IEnumerable<TextElement[]> rows)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddTable(rows);
		}

		public void AddHeading(Action<IHeadingBuilder> action)
		{
			var builder = new DocumentBodyBuilder(Document, _body.ChildElements.Last());
			builder.AddHeading(action);
		}

		public void AddHeader(Action<IHeaderBuilder> action)
		{
			var headerPart = Document.MainDocumentPart.HeaderParts.First();

			var builder = new HeaderBuilder();
			action(builder);
			builder.Build().Save(headerPart);
		}

		public void AddFooter(Action<IFooterBuilder> action)
		{
			var footerPart = Document.MainDocumentPart.FooterParts.First();

			var builder = new FooterBuilder();
			action(builder);
			builder.Build().Save(footerPart);
		}

		public WordprocessingDocument GetCurrentDocument()
		{
			return Document;
		}

		public byte[] Build()
		{
			Document.Save();
			Document.Close();
			return _ms.ToArray();
		}

		private void SetCompatibilityMode()
		{
			var mainPart = Document.MainDocumentPart;
			var settingsPart = mainPart.DocumentSettingsPart;

			if (settingsPart != null) return;

			settingsPart = mainPart.AddNewPart<DocumentSettingsPart>();
			settingsPart.Settings = new Settings(
				new Compatibility(
					new[] {
						new CompatibilitySetting
						{
							Name = new EnumValue<CompatSettingNameValues>
								(CompatSettingNameValues.CompatibilityMode),
							Val = new StringValue("15"),
							Uri = new StringValue
								("http://schemas.microsoft.com/office/word")
						}
					}
				)
			);
			settingsPart.Settings.Save();
		}

		private void AppendSectionProperties()
		{
			var props = new SectionProperties();
			AddHeaderToProperties(props);
			AddFooterToProperties(props);
			AddPageMargin(props);
			_body.AppendChild(props);
		}

		private void AddHeaderToProperties(SectionProperties props)
		{
			var mainDocumentPart = Document.MainDocumentPart;
			var headerPart = mainDocumentPart.AddNewPart<HeaderPart>();
			var headerPartId = mainDocumentPart.GetIdOfPart(headerPart);

			var headerReference = new HeaderReference { Id = headerPartId, Type = new EnumValue<HeaderFooterValues>(HeaderFooterValues.Default) };
			props.AppendChild(headerReference);
		}

		private void AddFooterToProperties(SectionProperties props)
		{
			var mainDocumentPart = Document.MainDocumentPart;
			var footerPart = mainDocumentPart.AddNewPart<FooterPart>();
			var footerPartId = mainDocumentPart.GetIdOfPart(footerPart);

			var footerReference = new FooterReference { Id = footerPartId, Type = new EnumValue<HeaderFooterValues>(HeaderFooterValues.Default) };
			props.AppendChild(footerReference);
		}

		private static void AddPageMargin(SectionProperties props)
		{
			var pageMargin = new PageMargin { Top = 850, Bottom = 994, Left = 1080, Right = 1080 };
			props.AppendChild(pageMargin);
		}

		private void PopulateTemplateWithDocument<TDocument>(TDocument document)
		{
			var allParagraphs = GetAllParagraphs();
			var properties = GetProperties<TDocument>();

			foreach (var paragraph in allParagraphs)
			{
				var property = properties.FirstOrDefault(p =>
					paragraph.InnerText.Contains($"{p.GetCustomAttribute<DocumentTextAttribute>()?.Placeholder}",
						StringComparison.OrdinalIgnoreCase));

				if (property == null) continue;

				var attribute = property.GetCustomAttribute<DocumentTextAttribute>();

				if (attribute == null) continue;

				foreach (var paragraphChildElement in paragraph.ChildElements
					.Where(paragraphChildElement => paragraphChildElement.GetType() != typeof(ParagraphProperties)).ToList())
				{
					paragraph.RemoveChild(paragraphChildElement);
				}

				var val = property.GetValue(document)?.ToString();

				if (attribute.IsRichText)
				{
					var elements = HtmlToElements.Convert(this, val);
					
					OpenXmlElement previousElement = paragraph;
					foreach(var element in elements)
					{
						previousElement.InsertAfterSelf(element);
						previousElement = element;
					}
					
					paragraph.Remove();
				}
				else
				{
					var run = new Run();
					DocumentBuilderHelpers.AddTextToElement(run, val);
					paragraph.AppendChild(run);
				}
			}

			IEnumerable<Paragraph> GetAllParagraphs()
			{
				var footerParts = Document.MainDocumentPart.FooterParts
					.Where(fp => fp.Footer != null)
					.SelectMany(fp => fp.Footer?.Descendants<Paragraph>());
				return Document.MainDocumentPart.Document.Body.Descendants<Paragraph>()
					.Concat(footerParts)
					.ToHashSet();
			}
		}

		private PropertyInfo[] GetProperties<TDocument>()
		{
			return typeof(TDocument).GetProperties()
				.Where(p => p.GetCustomAttribute<DocumentTextAttribute>() != null)
				.ToArray();
		}
	}
}