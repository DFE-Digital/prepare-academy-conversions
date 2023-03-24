using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.DocumentGeneration.Tests;

public class DocumentBuilderTests
{
   private static Body GenerateDocumentBody(Action<IDocumentBuilder> action)
   {
      DocumentBuilder builder = new();
      action(builder);
      byte[] res = builder.Build();

      using MemoryStream ms = new(res);
      using WordprocessingDocument doc = WordprocessingDocument.Open(ms, false);
      Body documentBody = doc.MainDocumentPart.Document.Body;

      return documentBody;
   }

   private static GenerateDocumentResponse GenerateDocument(Action<IDocumentBuilder> action)
   {
      DocumentBuilder builder = new();
      action(builder);
      byte[] res = builder.Build();
      using MemoryStream ms = new(res);
      using WordprocessingDocument doc = WordprocessingDocument.Open(ms, false);
      return new GenerateDocumentResponse
      {
         Numbering = doc.MainDocumentPart.NumberingDefinitionsPart.Numbering,
         Headers = doc.MainDocumentPart.HeaderParts.Select(p => p.Header).ToList(),
         Footers = doc.MainDocumentPart.FooterParts.Select(p => p.Footer).ToList(),
         Body = doc.MainDocumentPart.Document.Body
      };
   }

   private class GenerateDocumentResponse
   {
      public Body Body { get; init; }
      public Numbering Numbering { get; init; }
      public List<Header> Headers { get; init; }
      public List<Footer> Footers { get; init; }
   }

   public class ParagraphTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenAddingParagraphWithString_GeneratesParagraphWithText()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder => { pBuilder.AddText("Meow"); });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single((IEnumerable)paragraphs);
         Assert.Equal("Meow", paragraphs[0].InnerText);
      }

      [Fact]
      public void GivenAddingParagraphWithTextObject_GeneratesParagraphWithText()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder => { pBuilder.AddText(new TextElement { Value = "Woof" }); });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single(paragraphs);
         Assert.Empty(paragraphs[0].Descendants<Bold>());
         Assert.Equal("Woof", paragraphs[0].InnerText);
      }

      [Fact]
      public void GivenAddingParagraphWithBoldTextObject_GeneratesParagraphWithText()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder =>
            {
               pBuilder.AddText(new TextElement { Value = "Woof", Bold = true });
            });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single(paragraphs);
         Assert.Single(paragraphs[0].Descendants<Bold>());
         Assert.Equal("Woof", paragraphs[0].InnerText);
      }

      [Fact]
      public void GivenAddingParagraphWithItalicTextObject_GeneratesParagraphWithText()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder =>
            {
               pBuilder.AddText(new TextElement { Value = "Woof", Italic = true });
            });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single(paragraphs);
         Assert.Single(paragraphs[0].Descendants<Italic>());
         Assert.Equal("Woof", paragraphs[0].InnerText);
      }

      [Fact]
      public void GivenAddingParagraphWithUnderlineTextObject_GeneratesParagraphWithText()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder =>
            {
               pBuilder.AddText(new TextElement { Value = "Woof", Underline = true });
            });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single(paragraphs);
         Assert.Single(paragraphs[0].Descendants<Underline>());
         Assert.Equal("Woof", paragraphs[0].InnerText);
      }

      [Fact]
      public void GivenAddingParagraphWithMultipleTextObjects_GeneratesParagraphWithText()
      {
         TextElement[] text = { new() { Value = "Meow" }, new() { Value = "Woof", Bold = true }, new() { Value = "Quack" } };

         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder => { pBuilder.AddText(text); });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single(paragraphs);
         Assert.Single(paragraphs[0].Descendants<Bold>());
         Assert.Equal(3, paragraphs[0].Descendants<Run>().Count());
         Assert.Equal("MeowWoofQuack", paragraphs[0].InnerText);
      }

      [Fact]
      public void GivenAddingTextObjectsWithWhitespace_GeneratesParagraphWithPreservedWhitespace()
      {
         TextElement[] text = { new() { Value = "Meow" }, new() { Value = " Woof ", Bold = true }, new() { Value = "Quack" } };

         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder => { pBuilder.AddText(text); });
         });

         List<Text> texts = documentBody.Descendants<Text>().ToList();
         Assert.Equal(SpaceProcessingModeValues.Preserve, texts[0].Space.Value);
      }

      [Theory]
      [InlineData("Meow\r\nWoof")]
      [InlineData("Meow\nWoof")]
      [InlineData("Meow\rWoof")]
      public void GivenTextWithNewLine_CreatesTextSeparatedByNewLines(string text)
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder => { pBuilder.AddText(text); });
         });

         Paragraph paragraph = documentBody.Descendants<Paragraph>().ToList()[0];
         List<Text> texts = paragraph.Descendants<Text>().ToList();
         Assert.Equal(2, texts.Count);
         Assert.Single(paragraph.Descendants<Break>());
      }

      [Theory]
      [InlineData(ParagraphJustification.Left, JustificationValues.Left)]
      [InlineData(ParagraphJustification.Center, JustificationValues.Center)]
      [InlineData(ParagraphJustification.Right, JustificationValues.Right)]
      public void GivenParagraphJustification_JustifiesParagraphCorrectly(
         ParagraphJustification paragraphJustification,
         JustificationValues expectedJustification)
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddParagraph(pBuilder =>
            {
               pBuilder.AddText("Meow");
               pBuilder.Justify(paragraphJustification);
            });
         });

         Paragraph paragraph = documentBody.Descendants<Paragraph>().First();
         ParagraphProperties paragraphProperties = paragraph.ParagraphProperties;
         Assert.Equal(expectedJustification, paragraphProperties.Justification.Val.Value);
      }

      [Fact]
      public void GivenAddingParagraphAsText_AddsParagraphCorrectly()
      {
         Body documentBody = GenerateDocumentBody(builder => { builder.AddParagraph("Meow"); });

         Paragraph paragraph = documentBody.Descendants<Paragraph>().First();
         Assert.Equal("Meow", paragraph.InnerText);
      }
   }

   public class TableTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenAddingATableWithSingleStringCell_GeneratesTable()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddTable(tBuilder => { tBuilder.AddRow(rBuilder => { rBuilder.AddCell("test"); }); });
         });

         List<Table> table = documentBody.Descendants<Table>().ToList();
         Assert.Single(table);
         Assert.Single(documentBody.Descendants<TableProperties>());
         Assert.Single(documentBody.Descendants<TableRow>());
         Assert.Single(documentBody.Descendants<TableCell>());
         Assert.Equal("test", table[0].InnerText);
      }

      [Fact]
      public void GivenAddingATableWithSingleTextCell_GeneratesTable()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddTable(tBuilder =>
            {
               tBuilder.AddRow(rBuilder => { rBuilder.AddCell(new TextElement { Value = "test" }); });
            });
         });

         List<Table> table = documentBody.Descendants<Table>().ToList();
         Assert.Single(table);
         Assert.Single(documentBody.Descendants<TableProperties>());
         Assert.Single(documentBody.Descendants<TableRow>());
         Assert.Single(documentBody.Descendants<TableCell>());
         Assert.Equal("test", table[0].InnerText);
      }

      [Fact]
      public void GivenAddingATableWithMultipleStringCells_GeneratesTable()
      {
         string[] tests = { "test1", "test2" };

         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddTable(tBuilder => { tBuilder.AddRow(rBuilder => { rBuilder.AddCells(tests); }); });
         });

         List<Table> table = documentBody.Descendants<Table>().ToList();
         Assert.Single(table);
         Assert.Single(documentBody.Descendants<TableProperties>());
         Assert.Single(documentBody.Descendants<TableRow>());
         Assert.Equal(2, documentBody.Descendants<TableCell>().Count());

         List<TableCell> tableCells = documentBody.Descendants<TableCell>().ToList();

         Assert.Equal("test1", tableCells[0].InnerText);
         Assert.Equal("test2", tableCells[1].InnerText);
      }

      [Fact]
      public void GivenAddingATableWithMultipleTextCells_GeneratesTable()
      {
         TextElement[] textElements = { new() { Value = "test1" }, new() { Value = "test2" } };

         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddTable(tBuilder =>
            {
               tBuilder.AddRow(rBuilder => { rBuilder.AddCells(textElements); });
            });
         });

         List<Table> table = documentBody.Descendants<Table>().ToList();
         Assert.Single(table);
         Assert.Single(documentBody.Descendants<TableProperties>());
         Assert.Single(documentBody.Descendants<TableRow>());
         Assert.Equal(2, documentBody.Descendants<TableCell>().Count());

         List<TableCell> tableCells = documentBody.Descendants<TableCell>().ToList();

         Assert.Equal("test1", tableCells[0].InnerText);
         Assert.Equal("test2", tableCells[1].InnerText);
      }

      [Fact]
      public void GivenAddingATableByRows_GeneratesTheCorrectTable()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            TextElement[][] textElements =
            {
               new[] { new TextElement { Value = "One" }, new TextElement { Value = "Two" } }, new[] { new TextElement { Value = "Three" }, new TextElement { Value = "Four" } }
            };
            builder.AddTable(textElements);
         });

         List<TableRow> tableRows = documentBody.Descendants<TableRow>().ToList();
         List<TableCell> tableCells = documentBody.Descendants<TableCell>().ToList();

         Assert.Equal(2, tableRows.Count);
         Assert.Equal(4, tableCells.Count);
         Assert.Equal("OneTwo", tableRows[0].InnerText);
         Assert.Equal("ThreeFour", tableRows[1].InnerText);
      }
   }

   public class HeadingTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenHeadingWithString_AddsTextToDocument()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddHeading(hBuilder => { hBuilder.AddText("Meow"); });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single((IEnumerable)paragraphs);
         Assert.Equal("Meow", paragraphs[0].InnerText);
      }

      [Theory]
      [InlineData(HeadingLevel.One, "36")]
      [InlineData(HeadingLevel.Two, "32")]
      [InlineData(HeadingLevel.Three, "28")]
      public void GivenHeadingWithStringAndSize_AddsTextWithCorrectFontSize(HeadingLevel level,
                                                                            string expectedSize)
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddHeading(hBuilder =>
            {
               hBuilder.SetHeadingLevel(level);
               hBuilder.AddText("Meow");
            });
         });

         List<Run> run = documentBody.Descendants<Run>().ToList();
         Assert.Single(run);
         Assert.Equal(expectedSize, run[0].RunProperties.FontSize.Val);
      }

      [Fact]
      public void GivenHeadingWithTextElement_AddsTextToDocument()
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddHeading(hBuilder => { hBuilder.AddText(new TextElement { Value = "Meow" }); });
         });

         List<Paragraph> paragraphs = documentBody.Descendants<Paragraph>().ToList();
         Assert.Single((IEnumerable)paragraphs);
         Assert.Equal("Meow", paragraphs[0].InnerText);
      }

      [Theory]
      [InlineData(HeadingLevel.One, "36")]
      [InlineData(HeadingLevel.Two, "32")]
      [InlineData(HeadingLevel.Three, "28")]
      public void GivenHeadingWithTextElementAndSize_AddsTextWithCorrectFontSize(HeadingLevel level,
                                                                                 string expectedSize)
      {
         Body documentBody = GenerateDocumentBody(builder =>
         {
            builder.AddHeading(hBuilder =>
            {
               hBuilder.SetHeadingLevel(level);
               hBuilder.AddText(new TextElement { Value = "Meow" });
            });
         });

         List<Run> run = documentBody.Descendants<Run>().ToList();
         Assert.Single(run);
         Assert.Equal(expectedSize, run[0].RunProperties.FontSize.Val);
      }
   }

   public class HeaderTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenHeaderHasText_GeneratesHeaderForDocument()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddHeader(hBuilder => { hBuilder.AddParagraph(pBuilder => pBuilder.AddText("Meow")); });
         });

         Assert.Single(response.Headers);
         Header header = response.Headers[0];
         Assert.Single(header.Descendants<Paragraph>());
         Assert.Equal("Meow", header.InnerText);
      }

      [Fact]
      public void GivenHeaderHasTable_GeneratesHeaderForDocument()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddHeader(hBuilder =>
            {
               hBuilder.AddTable(
                  tBuilder => tBuilder.AddRow(rBuilder => { rBuilder.AddCell("Meow"); }));
            });
         });

         Assert.Single(response.Headers);
         Header header = response.Headers[0];
         Assert.Single(header.Descendants<Table>());
         Assert.Single(header.Descendants<Paragraph>());
         Assert.Equal("Meow", header.InnerText);
      }

      [Fact]
      public void GivenAddingATableByRows_GeneratesTheCorrectTable()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddHeader(hBuilder =>
            {
               TextElement[][] textElements =
               {
                  new[] { new TextElement { Value = "One" }, new TextElement { Value = "Two" } },
                  new[] { new TextElement { Value = "Three" }, new TextElement { Value = "Four" } }
               };
               hBuilder.AddTable(textElements);
            });
         });

         Header header = response.Headers[0];

         List<TableRow> tableRows = header.Descendants<TableRow>().ToList();
         List<TableCell> tableCells = header.Descendants<TableCell>().ToList();

         Assert.Equal(2, tableRows.Count);
         Assert.Equal(4, tableCells.Count);
         Assert.Equal("OneTwo", tableRows[0].InnerText);
         Assert.Equal("ThreeFour", tableRows[1].InnerText);
      }
   }

   public class FooterTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenFooterHasText_GeneratesFooterForDocument()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddFooter(fBuilder => { fBuilder.AddParagraph(pBuilder => pBuilder.AddText("Meow")); });
         });

         List<Footer> footers = response.Footers;

         Assert.Single(footers);
         Assert.Single(footers[0].Descendants<Paragraph>());
         Assert.Equal("Meow", footers[0].InnerText);
      }

      [Fact]
      public void GivenFooterHasTable_GeneratesFooterForDocument()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddFooter(fBuilder =>
            {
               fBuilder.AddTable(
                  tBuilder => tBuilder.AddRow(rBuilder => { rBuilder.AddCell("Meow"); }));
            });
         });

         List<Footer> footers = response.Footers;
         Assert.Single(footers[0].Descendants<Table>());
         Assert.Single(footers[0].Descendants<Paragraph>());
         Assert.Equal("Meow", footers[0].InnerText);
      }

      [Fact]
      public void GivenAddingATableByRows_GeneratesTheCorrectTable()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddFooter(fBuilder =>
            {
               TextElement[][] textElements =
               {
                  new[] { new TextElement { Value = "One" }, new TextElement { Value = "Two" } },
                  new[] { new TextElement { Value = "Three" }, new TextElement { Value = "Four" } }
               };
               fBuilder.AddTable(textElements);
            });
         });

         Footer footer = response.Footers[0];
         List<TableRow> tableRows = footer.Descendants<TableRow>().ToList();
         List<TableCell> tableCells = footer.Descendants<TableCell>().ToList();

         Assert.Equal(2, tableRows.Count);
         Assert.Equal(4, tableCells.Count);
         Assert.Equal("OneTwo", tableRows[0].InnerText);
         Assert.Equal("ThreeFour", tableRows[1].InnerText);
      }
   }

   public class BulletedListTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenAddingOneBulletedList_CreatesASingleNumberingDefinitionAndAssignsThem()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddBulletedList(lBuilder => { lBuilder.AddItem("One"); });
         });

         Numbering numbering = response.Numbering;
         List<NumberingFormat> numberingFormats = numbering.Descendants<NumberingFormat>().ToList();
         List<AbstractNum> abstractNumDefinitions = numbering.Descendants<AbstractNum>().ToList();
         List<NumberingInstance> numberingInstances = numbering.Descendants<NumberingInstance>().ToList();
         Paragraph paragraph = response.Body.Descendants<Paragraph>().First();

         Assert.Single(abstractNumDefinitions);
         Assert.Equal(NumberFormatValues.Bullet, numberingFormats[0].Val.Value);
         Assert.Equal(1, abstractNumDefinitions[0].AbstractNumberId.Value);
         Assert.Single(numberingInstances);
         Assert.Equal(1, (int)numberingInstances[0].AbstractNumId.Val);
         Assert.Equal(1, (int)paragraph.ParagraphProperties.NumberingProperties.NumberingId.Val);
      }

      [Fact]
      public void GivenAddingTwoBulletedLists_CreatesTwoNumberingDefinitions()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddBulletedList(lBuilder => { lBuilder.AddItem("One"); });
            builder.AddBulletedList(lBuilder => { lBuilder.AddItem("Two"); });
         });

         Numbering numbering = response.Numbering;
         List<NumberingFormat> numberingFormats = numbering.Descendants<NumberingFormat>().ToList();
         List<AbstractNum> abstractNumDefinitions = numbering.Descendants<AbstractNum>().ToList();
         List<NumberingInstance> numberingInstances = numbering.Descendants<NumberingInstance>().ToList();

         Assert.Equal(NumberFormatValues.Bullet, numberingFormats[0].Val.Value);
         Assert.Equal(NumberFormatValues.Bullet, numberingFormats[1].Val.Value);
         Assert.Equal(2, abstractNumDefinitions.Count);
         Assert.Equal(1, abstractNumDefinitions[0].AbstractNumberId.Value);
         Assert.Equal(2, abstractNumDefinitions[1].AbstractNumberId.Value);
         Assert.Equal(2, numberingInstances.Count);
         Assert.Equal(1, (int)numberingInstances[0].AbstractNumId.Val);
         Assert.Equal(2, (int)numberingInstances[1].AbstractNumId.Val);
      }

      [Fact]
      public void GivenAddingTwoBulletedLists_CreatesTwoNumberingDefinitionsInTheCorrectOrder()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddBulletedList(lBuilder => { lBuilder.AddItem("One"); });
            builder.AddBulletedList(lBuilder => { lBuilder.AddItem("Two"); });
         });

         Numbering numbering = response.Numbering;
         List<OpenXmlElement> numberingFormats = numbering.Elements().ToList();
         Assert.IsType<AbstractNum>(numberingFormats[0]);
         Assert.IsType<AbstractNum>(numberingFormats[1]);

         NumberingInstance abstractNumOne = Assert.IsType<NumberingInstance>(numberingFormats[2]);
         Assert.Equal(1, (int)abstractNumOne.AbstractNumId.Val);
         NumberingInstance abstractNumTwo = Assert.IsType<NumberingInstance>(numberingFormats[3]);
         Assert.Equal(2, (int)abstractNumTwo.AbstractNumId.Val);
      }

      [Fact]
      public void GivenAddingBulletedListWithStringItem_CreatesCorrectBulletedList()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddBulletedList(lBuilder =>
            {
               lBuilder.AddItem("One");
               lBuilder.AddItem("Two");
               lBuilder.AddItem("Three");
            });
         });

         List<Paragraph> paragraphs = response.Body.Descendants<Paragraph>().ToList();

         Assert.Equal(3, paragraphs.Count);
         Assert.True(paragraphs.All(p =>
            p.ParagraphProperties.Descendants<NumberingProperties>().First().NumberingId.Val == 1));
         Assert.Equal("One", paragraphs[0].InnerText);
         Assert.Equal("Two", paragraphs[1].InnerText);
         Assert.Equal("Three", paragraphs[2].InnerText);
      }

      [Fact]
      public void GivenAddingBulletedListWithTextElementItem_CreatesCorrectBulletedList()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddBulletedList(lBuilder =>
            {
               lBuilder.AddItem(new TextElement("One"));
               lBuilder.AddItem(new TextElement("Two"));
               lBuilder.AddItem(new TextElement("Three"));
            });
         });

         List<Paragraph> paragraphs = response.Body.Descendants<Paragraph>().ToList();

         Assert.Equal(3, paragraphs.Count);
         Assert.True(paragraphs.All(p =>
            p.ParagraphProperties.Descendants<NumberingProperties>().First().NumberingId.Val == 1));
         Assert.Equal("One", paragraphs[0].InnerText);
         Assert.Equal("Two", paragraphs[1].InnerText);
         Assert.Equal("Three", paragraphs[2].InnerText);
      }

      [Fact]
      public void GivenAddingBulletedListWithTextElementListItem_CreatesCorrectBulletedList()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddBulletedList(lBuilder =>
            {
               lBuilder.AddItem(new[] { new TextElement("One"), new TextElement("Two") });
            });
         });

         List<Paragraph> paragraphs = response.Body.Descendants<Paragraph>().ToList();

         Assert.Single(paragraphs);
         Assert.True(paragraphs.All(p =>
            p.ParagraphProperties.Descendants<NumberingProperties>().First().NumberingId.Val == 1));
         Assert.Equal("OneTwo", paragraphs[0].InnerText);
      }
   }

   public class NumberedListTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenAddingOneNumberedList_CreatesASingleNumberingDefinitionAndAssignsThem()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddNumberedList(lBuilder => { lBuilder.AddItem("One"); });
         });

         Numbering numbering = response.Numbering;
         List<NumberingFormat> numberingFormats = numbering.Descendants<NumberingFormat>().ToList();
         List<AbstractNum> abstractNumDefinitions = numbering.Descendants<AbstractNum>().ToList();
         List<NumberingInstance> numberingInstances = numbering.Descendants<NumberingInstance>().ToList();
         Paragraph paragraph = response.Body.Descendants<Paragraph>().First();

         Assert.Single(abstractNumDefinitions);
         Assert.Equal(NumberFormatValues.Decimal, numberingFormats[0].Val.Value);
         Assert.Equal(1, abstractNumDefinitions[0].AbstractNumberId.Value);
         Assert.Single(numberingInstances);
         Assert.Equal(1, (int)numberingInstances[0].AbstractNumId.Val);
         Assert.Equal(1, (int)paragraph.ParagraphProperties.NumberingProperties.NumberingId.Val);
      }

      [Fact]
      public void GivenAddingTwoNumberedLists_CreatesTwoNumberingDefinitions()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddNumberedList(lBuilder => { lBuilder.AddItem("One"); });
            builder.AddNumberedList(lBuilder => { lBuilder.AddItem("Two"); });
         });

         Numbering numbering = response.Numbering;
         List<NumberingFormat> numberingFormats = numbering.Descendants<NumberingFormat>().ToList();
         List<AbstractNum> abstractNumDefinitions = numbering.Descendants<AbstractNum>().ToList();
         List<NumberingInstance> numberingInstances = numbering.Descendants<NumberingInstance>().ToList();

         Assert.Equal(NumberFormatValues.Decimal, numberingFormats[0].Val.Value);
         Assert.Equal(NumberFormatValues.Decimal, numberingFormats[1].Val.Value);
         Assert.Equal(2, abstractNumDefinitions.Count);
         Assert.Equal(1, abstractNumDefinitions[0].AbstractNumberId.Value);
         Assert.Equal(2, abstractNumDefinitions[1].AbstractNumberId.Value);
         Assert.Equal(2, numberingInstances.Count);
         Assert.Equal(1, (int)numberingInstances[0].AbstractNumId.Val);
         Assert.Equal(2, (int)numberingInstances[1].AbstractNumId.Val);
      }

      [Fact]
      public void GivenAddingTwoNumberedLists_CreatesTwoNumberingDefinitionsInTheCorrectOrder()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddNumberedList(lBuilder => { lBuilder.AddItem("One"); });
            builder.AddNumberedList(lBuilder => { lBuilder.AddItem("Two"); });
         });

         Numbering numbering = response.Numbering;
         List<OpenXmlElement> numberingFormats = numbering.Elements().ToList();
         Assert.IsType<AbstractNum>(numberingFormats[0]);
         Assert.IsType<AbstractNum>(numberingFormats[1]);

         NumberingInstance abstractNumOne = Assert.IsType<NumberingInstance>(numberingFormats[2]);
         Assert.Equal(1, (int)abstractNumOne.AbstractNumId.Val);
         NumberingInstance abstractNumTwo = Assert.IsType<NumberingInstance>(numberingFormats[3]);
         Assert.Equal(2, (int)abstractNumTwo.AbstractNumId.Val);
      }

      [Fact]
      public void GivenAddingNumberedListWithStringItem_CreatesCorrectNumberedList()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddNumberedList(lBuilder =>
            {
               lBuilder.AddItem("One");
               lBuilder.AddItem("Two");
               lBuilder.AddItem("Three");
            });
         });

         List<Paragraph> paragraphs = response.Body.Descendants<Paragraph>().ToList();

         Assert.Equal(3, paragraphs.Count);
         Assert.True(paragraphs.All(p =>
            p.ParagraphProperties.Descendants<NumberingProperties>().First().NumberingId.Val == 1));
         Assert.Equal("One", paragraphs[0].InnerText);
         Assert.Equal("Two", paragraphs[1].InnerText);
         Assert.Equal("Three", paragraphs[2].InnerText);
      }

      [Fact]
      public void GivenAddingNumberedListWithTextElementItem_CreatesCorrectNumberedList()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddNumberedList(lBuilder =>
            {
               lBuilder.AddItem(new TextElement("One"));
               lBuilder.AddItem(new TextElement("Two"));
               lBuilder.AddItem(new TextElement("Three"));
            });
         });

         List<Paragraph> paragraphs = response.Body.Descendants<Paragraph>().ToList();

         Assert.Equal(3, paragraphs.Count);
         Assert.True(paragraphs.All(p =>
            p.ParagraphProperties.Descendants<NumberingProperties>().First().NumberingId.Val == 1));
         Assert.Equal("One", paragraphs[0].InnerText);
         Assert.Equal("Two", paragraphs[1].InnerText);
         Assert.Equal("Three", paragraphs[2].InnerText);
      }

      [Fact]
      public void GivenAddingNumberedListWithTextElementListItem_CreatesCorrectNumberedList()
      {
         GenerateDocumentResponse response = GenerateDocument(builder =>
         {
            builder.AddNumberedList(lBuilder =>
            {
               lBuilder.AddItem(new[] { new TextElement("One"), new TextElement("Two") });
            });
         });

         List<Paragraph> paragraphs = response.Body.Descendants<Paragraph>().ToList();

         Assert.Single(paragraphs);
         Assert.True(paragraphs.All(p =>
            p.ParagraphProperties.Descendants<NumberingProperties>().First().NumberingId.Val == 1));
         Assert.Equal("OneTwo", paragraphs[0].InnerText);
      }
   }

   public class DocumentFromTemplateTests : DocumentBuilderTests
   {
      private static byte[] Template()
      {
         DocumentBuilder documentBuilder = new();
         documentBuilder.AddParagraph("[PlaceholderOne]");
         documentBuilder.AddParagraph("Non replaced text");
         documentBuilder.AddParagraph("[PlaceholderTwo]");
         byte[] template = documentBuilder.Build();
         return template;
      }

      [Fact]
      public void GivenTemplateDocumentWithPlaceholderTestAndNoPlaceholderValue_RemovePlaceholderText()
      {
         byte[] template = Template();

         ExampleDocument document = new();
         DocumentBuilder builderFromTemplate = DocumentBuilder.CreateFromTemplate(new MemoryStream(template), document);
         byte[] result = builderFromTemplate.Build();

         WordprocessingDocument createdDocument = WordprocessingDocument.Open(new MemoryStream(result), false);
         List<string> documentText = createdDocument.MainDocumentPart.Document.Body.Descendants<Text>()
            .Select(t => t.Text).ToList();

         Assert.Equal("", documentText[0]);
         Assert.Equal("Non replaced text", documentText[1]);
         Assert.Equal("", documentText[2]);
      }

      [Fact]
      public void
         GivenTemplateDocumentWithPlaceholderTestAndOnePlaceholderValue_PopulatePlaceholdersWithDocument()
      {
         byte[] template = Template();

         ExampleDocument document = new() { PlaceholderOne = "Meow" };
         DocumentBuilder builderFromTemplate = DocumentBuilder.CreateFromTemplate(new MemoryStream(template), document);
         byte[] result = builderFromTemplate.Build();

         WordprocessingDocument createdDocument = WordprocessingDocument.Open(new MemoryStream(result), false);
         List<string> documentText = createdDocument.MainDocumentPart.Document.Body.Descendants<Text>()
            .Select(t => t.Text).ToList();

         Assert.Equal("Meow", documentText[0]);
         Assert.Equal("Non replaced text", documentText[1]);
         Assert.Equal("", documentText[2]);
      }

      [Fact]
      public void GivenTemplateDocumentWithPlaceholderTestAndPlaceholderValues_PopulatePlaceholdersWithDocument()
      {
         byte[] template = Template();

         ExampleDocument document = new() { PlaceholderOne = "Meow", PlaceholderTwo = "Woof" };
         DocumentBuilder builderFromTemplate = DocumentBuilder.CreateFromTemplate(new MemoryStream(template), document);
         byte[] result = builderFromTemplate.Build();

         WordprocessingDocument createdDocument = WordprocessingDocument.Open(new MemoryStream(result), false);
         List<string> documentText = createdDocument.MainDocumentPart.Document.Body.Descendants<Text>()
            .Select(t => t.Text).ToList();

         Assert.Equal("Meow", documentText[0]);
         Assert.Equal("Non replaced text", documentText[1]);
         Assert.Equal("Woof", documentText[2]);
      }

      [Fact]
      public void GivenTemplateDocumentWithContentInFooter_PopulatesFooter()
      {
         DocumentBuilder documentBuilder = new();
         documentBuilder.AddFooter(fBuilder =>
         {
            fBuilder.AddParagraph(pBuilder => pBuilder.AddText("[PlaceholderOne]"));
            fBuilder.AddParagraph(pBuilder => pBuilder.AddText("Non replaced text"));
            fBuilder.AddParagraph(pBuilder => pBuilder.AddText("[PlaceholderTwo]"));
         });

         byte[] template = documentBuilder.Build();

         ExampleDocument document = new() { PlaceholderOne = "Meow", PlaceholderTwo = "Woof" };
         DocumentBuilder builderFromTemplate = DocumentBuilder.CreateFromTemplate(new MemoryStream(template), document);
         byte[] result = builderFromTemplate.Build();

         WordprocessingDocument createdDocument = WordprocessingDocument.Open(new MemoryStream(result), false);
         List<string> documentFooterText = createdDocument.MainDocumentPart.FooterParts
            .SelectMany(fp => fp.Footer.Descendants<Text>())
            .Select(t => t.Text).ToList();

         Assert.Equal("Meow", documentFooterText[0]);
         Assert.Equal("Non replaced text", documentFooterText[1]);
         Assert.Equal("Woof", documentFooterText[2]);
      }

      [Fact]
      public void GivenTemplateDocumentWithRichTextPlaceholder_PopulatesRichText()
      {
         DocumentBuilder documentBuilder = new();
         documentBuilder.AddParagraph("[PlaceholderRichText]");
         byte[] template = documentBuilder.Build();
         ExampleDocument document = new() { RichText = "<p>Some rich text</p><p>Goes here</p>" };

         using MemoryStream ms = new();
         ms.Write(template);

         DocumentBuilder builderFromTemplate = DocumentBuilder.CreateFromTemplate(ms, document);
         byte[] result = builderFromTemplate.Build();

         WordprocessingDocument createdDocument = WordprocessingDocument.Open(new MemoryStream(result), false);
         List<Paragraph> paragraphs = createdDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>().ToList();
         Assert.Equal(2, paragraphs.Count);
         Assert.Equal("Some rich text", paragraphs[0].InnerText);
         Assert.Equal("Goes here", paragraphs[1].InnerText);
      }

      private class ExampleDocument
      {
         [DocumentText("PlaceholderOne")]
         public string PlaceholderOne { get; set; }

         [DocumentText("PlaceholderTwo")]
         public string PlaceholderTwo { get; set; }

         [DocumentText("PlaceholderRichText", IsRichText = true)]
         public string RichText { get; set; }
      }
   }

   public class ReplacePlaceholderContentTests : DocumentBuilderTests
   {
      [Fact]
      public void GivenDocumentWithPlaceholderContent_AllowsYouToReplaceContent()
      {
         DocumentBuilder documentBuilder = new();
         documentBuilder.AddParagraph("Non replaced text");
         documentBuilder.AddParagraph("[PlaceholderOne]");
         documentBuilder.AddParagraph("Non replaced text");
         documentBuilder.AddParagraph("[PlaceholderTwo]");
         byte[] template = documentBuilder.Build();

         MemoryStream memoryStream = new();
         memoryStream.Write(template);
         DocumentBuilder builderFromTemplate = DocumentBuilder.CreateFromTemplate(memoryStream, new object());

         builderFromTemplate.ReplacePlaceholderWithContent("PlaceholderOne", builder =>
         {
            builder.AddParagraph("Meow");
            builder.AddParagraph("Woof");
            builder.AddParagraph("Quack");
         });

         builderFromTemplate.ReplacePlaceholderWithContent("PlaceholderTwo",
            builder => { builder.AddParagraph("Moo"); });
         byte[] result = builderFromTemplate.Build();

         WordprocessingDocument createdDocument = WordprocessingDocument.Open(new MemoryStream(result), false);
         List<Paragraph> createdParagraphs = createdDocument.MainDocumentPart.Document.Body
            .Descendants<Paragraph>()
            .ToList();

         Assert.Equal("Non replaced text", createdParagraphs[0].InnerText);
         Assert.Equal("Meow", createdParagraphs[1].InnerText);
         Assert.Equal("Woof", createdParagraphs[2].InnerText);
         Assert.Equal("Quack", createdParagraphs[3].InnerText);
         Assert.Equal("Non replaced text", createdParagraphs[4].InnerText);
         Assert.Equal("Moo", createdParagraphs[5].InnerText);
      }
   }
}
