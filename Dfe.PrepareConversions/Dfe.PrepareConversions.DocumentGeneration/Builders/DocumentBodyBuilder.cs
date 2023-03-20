using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class DocumentBodyBuilder : IDocumentBodyBuilder
{
   private readonly WordprocessingDocument _document;
   private OpenXmlElement _previousElement;

   public DocumentBodyBuilder(WordprocessingDocument document, OpenXmlElement previousElement)
   {
      _document = document;
      _previousElement = previousElement;
   }

   public void AddParagraph(Action<IParagraphBuilder> action)
   {
      ParagraphBuilder builder = new();
      action(builder);
      Paragraph newElement = builder.Build();
      _previousElement.InsertAfterSelf(newElement);
      _previousElement = newElement;
   }

   public void AddParagraph(string text)
   {
      ParagraphBuilder builder = new();
      builder.AddText(text);
      Paragraph newElement = builder.Build();
      _previousElement.InsertAfterSelf(newElement);
      _previousElement = newElement;
   }

   public void AddTable(Action<ITableBuilder> action)
   {
      TableBuilder builder = new();
      action(builder);
      Table newElement = builder.Build();
      _previousElement.InsertAfterSelf(newElement);
      _previousElement = newElement;
   }

   public void AddTable(IEnumerable<TextElement[]> rows)
   {
      TableBuilder builder = new();
      foreach (TextElement[] row in rows)
      {
         builder.AddRow(rBuilder => { rBuilder.AddCells(row); });
      }

      Table newElement = builder.Build();
      _previousElement.InsertAfterSelf(newElement);
      _previousElement = newElement;
   }

   public void AddNumberedList(Action<IListBuilder> action)
   {
      NumberedListBuilder builder = new(_document.MainDocumentPart.NumberingDefinitionsPart);
      action(builder);
      List<Paragraph> newElements = builder.Build();
      foreach (Paragraph element in newElements)
      {
         _previousElement.InsertAfterSelf(element);
         _previousElement = element;
      }
   }

   public void AddBulletedList(Action<IListBuilder> action)
   {
      BulletedListBuilder builder = new(_document.MainDocumentPart.NumberingDefinitionsPart);
      action(builder);
      List<Paragraph> newElements = builder.Build();
      foreach (Paragraph element in newElements)
      {
         _previousElement.InsertAfterSelf(element);
         _previousElement = element;
      }
   }

   public void AddHeading(Action<IHeadingBuilder> action)
   {
      HeadingBuilder builder = new();
      action(builder);
      List<Paragraph> newElements = builder.Build();
      foreach (Paragraph element in newElements)
      {
         _previousElement.InsertAfterSelf(element);
         _previousElement = element;
      }
   }

   public void AddHeading(string text, HeadingLevel level)
   {
      HeadingBuilder builder = new();
      builder.SetHeadingLevel(level);
      builder.AddText(text);
      List<Paragraph> newElements = builder.Build();
      foreach (Paragraph element in newElements)
      {
         _previousElement.InsertAfterSelf(element);
         _previousElement = element;
      }
   }
}
