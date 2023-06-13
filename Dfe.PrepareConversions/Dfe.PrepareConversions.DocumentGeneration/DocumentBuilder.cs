using Dfe.PrepareConversions.DocumentGeneration.Builders;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Helpers;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dfe.PrepareConversions.DocumentGeneration;

public class DocumentBuilder : IDocumentBuilder
{
   private readonly Body _body;
   private readonly MemoryStream _ms;
   public readonly WordprocessingDocument Document;

   public DocumentBuilder()
   {
      _ms = new MemoryStream();
      Document = WordprocessingDocument.Create(_ms, WordprocessingDocumentType.Document);
      Document.AddMainDocumentPart();
      Document.MainDocumentPart.Document = new Document(new Body());
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

   public void ReplacePlaceholderWithContent(string placeholderText, Action<DocumentBodyBuilder> action)
   {
      List<Paragraph> paragraphs = _body.Descendants<Paragraph>().ToList();
      Paragraph placeholderElement = paragraphs.First(element => element.InnerText.Contains($"[{placeholderText}]"));

      DocumentBodyBuilder builder = new(Document, placeholderElement);
      action(builder);
      placeholderElement.Remove();
   }

   public void AddNumberedList(Action<IListBuilder> action)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddNumberedList(action);
   }

   public void AddBulletedList(Action<IListBuilder> action)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddBulletedList(action);
   }

   public void AddParagraph(Action<IParagraphBuilder> action)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddParagraph(action);
   }

   public void AddParagraph(string text)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddParagraph(text);
   }

   public void AddTable(Action<ITableBuilder> action)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddTable(action);
   }

   public void AddTable(IEnumerable<TextElement[]> rows)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddTable(rows);
   }

   public void AddHeading(Action<IHeadingBuilder> action)
   {
      DocumentBodyBuilder builder = new(Document, _body.ChildElements.Last());
      builder.AddHeading(action);
   }

   public void AddHeader(Action<IHeaderBuilder> action)
   {
      HeaderPart headerPart = Document.MainDocumentPart.HeaderParts.First();

      HeaderBuilder builder = new();
      action(builder);
      builder.Build().Save(headerPart);
   }

   public void AddFooter(Action<IFooterBuilder> action)
   {
      FooterPart footerPart = Document.MainDocumentPart.FooterParts.First();

      FooterBuilder builder = new();
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

   public static DocumentBuilder CreateFromTemplate<TDocument>(MemoryStream stream, TDocument document)
   {
      DocumentBuilder builder = new(stream);
      builder.PopulateTemplateWithDocument(document);
      return builder;
   }

   private void AddNumberingDefinitions()
   {
      NumberingDefinitionsPart part = Document.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>();
      part.Numbering = new Numbering();
   }

   private void SetCompatibilityMode()
   {
      MainDocumentPart mainPart = Document.MainDocumentPart;
      DocumentSettingsPart settingsPart = mainPart.DocumentSettingsPart;

      if (settingsPart != null) return;

      settingsPart = mainPart.AddNewPart<DocumentSettingsPart>();
      settingsPart.Settings = new Settings(new Compatibility(new CompatibilitySetting
      {
         Name = new EnumValue<CompatSettingNameValues>
            (CompatSettingNameValues.CompatibilityMode),
         Val = new StringValue("15"),
         Uri = new StringValue
            ("http://schemas.microsoft.com/office/word")
      }));
      settingsPart.Settings.Save();
   }

   private void AppendSectionProperties()
   {
      SectionProperties props = new();
      AddHeaderToProperties(props);
      AddFooterToProperties(props);
      AddPageMargin(props);
      _body.AppendChild(props);
   }

   private void AddHeaderToProperties(SectionProperties props)
   {
      MainDocumentPart mainDocumentPart = Document.MainDocumentPart;
      HeaderPart headerPart = mainDocumentPart.AddNewPart<HeaderPart>();
      string headerPartId = mainDocumentPart.GetIdOfPart(headerPart);

      HeaderReference headerReference = new() { Id = headerPartId, Type = new EnumValue<HeaderFooterValues>(HeaderFooterValues.Default) };
      props.AppendChild(headerReference);
   }

   private void AddFooterToProperties(SectionProperties props)
   {
      MainDocumentPart mainDocumentPart = Document.MainDocumentPart;
      FooterPart footerPart = mainDocumentPart.AddNewPart<FooterPart>();
      string footerPartId = mainDocumentPart.GetIdOfPart(footerPart);

      FooterReference footerReference = new() { Id = footerPartId, Type = new EnumValue<HeaderFooterValues>(HeaderFooterValues.Default) };
      props.AppendChild(footerReference);
   }

   private static void AddPageMargin(SectionProperties props)
   {
      PageMargin pageMargin = new() { Top = 850, Bottom = 994, Left = 1080, Right = 1080 };
      props.AppendChild(pageMargin);
   }

   private void PopulateTemplateWithDocument<TDocument>(TDocument document)
   {
      IEnumerable<Paragraph> allParagraphs = GetAllParagraphs();
      PropertyInfo[] properties = GetProperties<TDocument>();

      foreach (Paragraph paragraph in allParagraphs)
      {
         PropertyInfo property = properties.FirstOrDefault(p =>
            paragraph.InnerText.Contains($"{p.GetCustomAttribute<DocumentTextAttribute>()?.Placeholder}",
               StringComparison.OrdinalIgnoreCase));

         if (property == null) continue;

         DocumentTextAttribute attribute = property.GetCustomAttribute<DocumentTextAttribute>();

         if (attribute == null) continue;

         foreach (OpenXmlElement paragraphChildElement in paragraph.ChildElements
                     .Where(paragraphChildElement => paragraphChildElement.GetType() != typeof(ParagraphProperties)).ToList())
         {
            paragraph.RemoveChild(paragraphChildElement);
         }

         string val = property.GetValue(document)?.ToString();

         if (attribute.IsRichText)
         {
            List<OpenXmlElement> elements = HtmlToElements.Convert(this, val);

            OpenXmlElement previousElement = paragraph;
            foreach (OpenXmlElement element in elements)
            {
               previousElement.InsertAfterSelf(element);
               previousElement = element;
            }

            paragraph.Remove();
         }
         else
         {
            Run run = new();
            DocumentBuilderHelpers.AddTextToElement(run, val);
            paragraph.AppendChild(run);
         }
      }

      IEnumerable<Paragraph> GetAllParagraphs()
      {
         IEnumerable<Paragraph> footerParts = Document.MainDocumentPart.FooterParts
            .Where(fp => fp.Footer != null)
            .SelectMany(fp => fp.Footer?.Descendants<Paragraph>());
         return Document.MainDocumentPart.Document.Body.Descendants<Paragraph>()
            .Concat(footerParts)
            .ToHashSet();
      }
   }

   private static PropertyInfo[] GetProperties<TDocument>()
   {
      return typeof(TDocument).GetProperties()
         .Where(p => p.GetCustomAttribute<DocumentTextAttribute>() != null)
         .ToArray();
   }
}
