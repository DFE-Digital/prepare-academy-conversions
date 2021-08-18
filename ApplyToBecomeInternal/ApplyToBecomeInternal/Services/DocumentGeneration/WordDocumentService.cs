using ApplyToBecomeInternal.Extensions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ApplyToBecomeInternal.Services.WordDocument
{

	public class WordDocumentService
	{
		public byte[] Create<TDocument>(string template, TDocument document)
		{
			using var ms = CreateMemoryStream(template);
			using var wordDoc = WordprocessingDocument.Open(ms, true);
			var properties = GetProperties<TDocument>();

			foreach (var text in GetAllText(wordDoc))
			{
				var matchingProperties = properties.Where(p => text.InnerText.Contains($"{p.GetCustomAttribute<DocumentTextAttribute>()?.Placeholder}", StringComparison.OrdinalIgnoreCase)).ToArray();
				foreach (var property in matchingProperties)
				{
					var attribute = property.GetCustomAttribute<DocumentTextAttribute>();
					var value = property.GetValue(document).ToStringOrDefault(attribute.Default);
					if (attribute.IsRichText && !string.IsNullOrEmpty(value))
					{
						ReplaceWithRichText(wordDoc, text, value);
					}
					text.Text = text.Text.Replace($"{attribute.Placeholder}", value, StringComparison.OrdinalIgnoreCase);
				}
			}

			wordDoc.Close();
			return ms.ToArray();
		}

		private static MemoryStream CreateMemoryStream(string template)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains(template, StringComparison.OrdinalIgnoreCase));
			using var templateStream = assembly.GetManifestResourceStream(resourceName);
			var ms = new MemoryStream();
			templateStream.CopyTo(ms);
			return ms;
		}

		private static PropertyInfo[] GetProperties<TDocument>()
		{
			return typeof(TDocument).GetProperties()
				.Where(p => p.GetCustomAttribute<DocumentTextAttribute>() != null)
				.ToArray();
		}

		private static IEnumerable<Text> GetAllText(WordprocessingDocument wordDoc)
		{
			return wordDoc.MainDocumentPart.Document.Body.Descendants<Text>()
				.Concat(wordDoc.MainDocumentPart.FooterParts.SelectMany(fp => fp.Footer.Descendants<Text>()))
				.ToHashSet();
		}

		private static void ReplaceWithRichText(WordprocessingDocument wordDoc, Text text, string value)
		{
			var htmlDocument = HtmlDocumentFactory.Create(value);
			if (htmlDocument.Body.ChildElementCount > 0)
			{
				var run = text.Parent as Run;
				var paragraph = run.Parent as Paragraph;
				var visitor = new HtmlToOpenXmlVisitor(wordDoc.MainDocumentPart, paragraph.ParagraphProperties, run.RunProperties);
				htmlDocument.Accept(visitor);
				if (visitor.OpenXmlElements.Any())
				{
					var parent = paragraph.Parent;
					parent.RemoveChild(paragraph);
					foreach (var element in visitor.OpenXmlElements)
						parent.Append(element);
				}
			}
		}
	}
}
