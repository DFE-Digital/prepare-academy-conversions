using Dfe.PrepareConversions.DocumentGeneration.Helpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.DocumentGeneration.Tests.Helpers;

public class HtmlToElementsTests
{
   private readonly DocumentBuilder _builder;

   public HtmlToElementsTests()
   {
      _builder = new DocumentBuilder();
   }

   private static void AssertBold(Run run)
   {
      Assert.NotNull(run.RunProperties.Bold);
   }

   private static void AssertUnderline(Run run)
   {
      Assert.NotNull(run.RunProperties.Underline);
   }

   private static void AssertItalic(Run run)
   {
      Assert.NotNull(run.RunProperties.Italic);
   }

   private static void AssertParagraphHasNumberingId(Int32Value numberId, Paragraph paragraph)
   {
      Assert.Equal(numberId, paragraph.ParagraphProperties.NumberingProperties.NumberingId.Val);
   }


   [Theory]
   [InlineData(null)]
   [InlineData("")]
   public void GivenEmptyOrNull_BuildsNothing(string text)
   {
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, text);

      Assert.Empty(res);
   }

   [Theory]
   [InlineData("Meow woof quack")]
   [InlineData("Moo quack cluck")]
   [InlineData("     ")]
   public void GivenParagraph_GeneratesParagraph(string text)
   {
      string html = $"<p>{text}</p>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Single(res);
      Paragraph para = Assert.IsType<Paragraph>(res[0]);
      Assert.Equal(text, para.InnerText);
   }

   [Fact]
   public void GivenParagraphWithMixedFormatting_GeneratesParagraphs()
   {
      const string html = "<p>Meow <b>Woof <u>Quack <i>Moo</i></u></b></p>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Single(res);
      Paragraph addedParagraph = Assert.IsType<Paragraph>(res[0]);
      List<Run> addedRuns = addedParagraph.Descendants<Run>().ToList();

      Assert.Equal("Meow Woof Quack Moo", addedParagraph.InnerText);
      AssertBold(addedRuns[1]);
      AssertBold(addedRuns[2]);
      AssertUnderline(addedRuns[2]);
      AssertBold(addedRuns[3]);
      AssertUnderline(addedRuns[3]);
      AssertItalic(addedRuns[3]);
   }

   [Fact]
   public void GivenParagraphContainsABreak_GenerateParagraphWithNewline()
   {
      const string html = "<p>Meow<br>Woof</p>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Single(res);
      OpenXmlElement para = res[0];
      List<Text> textElements = para.Descendants<Text>().ToList();

      Assert.Equal("MeowWoof", para.InnerText);
      Assert.Equal("Meow", textElements[0].Text);
      Assert.Single(para.Descendants<Break>());
      Assert.Equal("Woof", textElements[1].Text);
   }

   private static void AssertListIsType(WordprocessingDocument document, int listIndex, NumberFormatValues expectedFormat)
   {
      List<AbstractNum> abstractNums = document.MainDocumentPart.NumberingDefinitionsPart.Numbering.Descendants<AbstractNum>().ToList();
      NumberFormatValues numberFormatValues = abstractNums[listIndex].Descendants<NumberingFormat>().First().Val.Value;
      Assert.Equal(expectedFormat, numberFormatValues);
   }

   [Fact]
   public void GivenOrderedListWithSingleListItem_RendersNumberedList()
   {
      const string html = "<ol><li>Meow</li></ol>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Single(res);
      Assert.Equal("Meow", res[0].InnerText);
      AssertListIsType(_builder.GetCurrentDocument(), 0, NumberFormatValues.Decimal);
   }

   [Fact]
   public void GivenOrderedListWithSeveralListItems_RendersNumberedList()
   {
      const string html = "<ol><li>Meow</li><li>Woof</li><li>Quack</li></ol>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);
      WordprocessingDocument document = _builder.GetCurrentDocument();
      Int32Value numberId = _builder.GetCurrentDocument().MainDocumentPart.NumberingDefinitionsPart.Numbering.Descendants<NumberingInstance>().First().NumberID;

      Assert.Equal(3, res.Count);
      AssertListIsType(document, 0, NumberFormatValues.Decimal);

      Paragraph paragraphOne = Assert.IsType<Paragraph>(res[0]);
      Assert.Equal("Meow", paragraphOne.InnerText);
      AssertParagraphHasNumberingId(numberId, paragraphOne);

      Paragraph paragraphTwo = Assert.IsType<Paragraph>(res[1]);
      Assert.Equal("Woof", paragraphTwo.InnerText);
      AssertParagraphHasNumberingId(numberId, paragraphTwo);

      Paragraph paragraphThree = Assert.IsType<Paragraph>(res[2]);
      Assert.Equal("Quack", paragraphThree.InnerText);
      AssertParagraphHasNumberingId(numberId, paragraphThree);
   }

   [Fact]
   public void GivenOrderedListWithSeveralListItemsWithMixedFormatting_RendersNumberedList()
   {
      const string html =
         "<ol><li><b>Me<i>ow</i></b></li><li>W<u>oo</u>f</li><li><b><u><i>Quack</b></u></i></li></ol>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);
      WordprocessingDocument document = _builder.GetCurrentDocument();

      Assert.Equal(3, res.Count);
      AssertListIsType(document, 0, NumberFormatValues.Decimal);
      Assert.Equal("Meow", res[0].InnerText);
      Assert.Equal("Woof", res[1].InnerText);
      Assert.Equal("Quack", res[2].InnerText);

      Paragraph paraOne = Assert.IsType<Paragraph>(res[0]);
      List<Run> paraOneRuns = paraOne.Descendants<Run>().ToList();
      Assert.Equal(2, paraOneRuns.Count);
      Assert.Equal("Me", paraOneRuns[0].InnerText);
      AssertBold(paraOneRuns[0]);
      Assert.Equal("ow", paraOneRuns[1].InnerText);
      AssertBold(paraOneRuns[1]);
      AssertItalic(paraOneRuns[1]);

      Paragraph paraTwo = Assert.IsType<Paragraph>(res[1]);
      List<Run> paraTwoRuns = paraTwo.Descendants<Run>().ToList();
      Assert.Equal(3, paraTwoRuns.Count);
      Assert.Equal("W", paraTwoRuns[0].InnerText);
      Assert.Equal("oo", paraTwoRuns[1].InnerText);
      AssertUnderline(paraTwoRuns[1]);
      Assert.Equal("f", paraTwoRuns[2].InnerText);

      Paragraph paraThree = Assert.IsType<Paragraph>(res[2]);
      List<Run> paraThreeRuns = paraThree.Descendants<Run>().ToList();
      Assert.Single(paraThreeRuns);
      Assert.Equal("Quack", paraThreeRuns[0].InnerText);
      AssertBold(paraThreeRuns[0]);
      AssertItalic(paraThreeRuns[0]);
      AssertUnderline(paraThreeRuns[0]);
   }

   [Fact]
   public void GivenUnorderedListWithSingleListItem_RendersBulletList()
   {
      const string html = "<ul><li>Meow</li></ul>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Single(res);
      AssertListIsType(_builder.GetCurrentDocument(), 0, NumberFormatValues.Bullet);
      Paragraph paragraph = Assert.IsType<Paragraph>(res[0]);
      Assert.Equal("Meow", paragraph.InnerText);
   }

   [Fact]
   public void GivenUnorderedListWithSeveralListItems_RendersBulletList()
   {
      const string html = "<ul><li>Meow</li><li>Woof</li><li>Quack</li></ul>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Equal(3, res.Count);
      AssertListIsType(_builder.GetCurrentDocument(), 0, NumberFormatValues.Bullet);

      Paragraph paraOne = Assert.IsType<Paragraph>(res[0]);
      Assert.Equal("Meow", paraOne.InnerText);
      Paragraph paraTwo = Assert.IsType<Paragraph>(res[1]);
      Assert.Equal("Woof", paraTwo.InnerText);
      Paragraph paraThree = Assert.IsType<Paragraph>(res[2]);
      Assert.Equal("Quack", paraThree.InnerText);
   }

   [Fact]
   public void GivenUnorderedListWithSeveralListItemsWithMixedFormatting_RendersBulletList()
   {
      const string html =
         "<ul><li><b>Me<i>ow</i></b></li><li>W<u>oo</u>f</li><li><b><u><i>Quack</b></u></i></li></ul>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);
      WordprocessingDocument document = _builder.GetCurrentDocument();

      Assert.Equal(3, res.Count);
      AssertListIsType(document, 0, NumberFormatValues.Bullet);
      Assert.Equal("Meow", res[0].InnerText);
      Assert.Equal("Woof", res[1].InnerText);
      Assert.Equal("Quack", res[2].InnerText);

      Paragraph paraOne = Assert.IsType<Paragraph>(res[0]);
      List<Run> paraOneRuns = paraOne.Descendants<Run>().ToList();
      Assert.Equal(2, paraOneRuns.Count);
      Assert.Equal("Me", paraOneRuns[0].InnerText);
      AssertBold(paraOneRuns[0]);
      Assert.Equal("ow", paraOneRuns[1].InnerText);
      AssertBold(paraOneRuns[1]);
      AssertItalic(paraOneRuns[1]);

      Paragraph paraTwo = Assert.IsType<Paragraph>(res[1]);
      List<Run> paraTwoRuns = paraTwo.Descendants<Run>().ToList();
      Assert.Equal(3, paraTwoRuns.Count);
      Assert.Equal("W", paraTwoRuns[0].InnerText);
      Assert.Equal("oo", paraTwoRuns[1].InnerText);
      AssertUnderline(paraTwoRuns[1]);
      Assert.Equal("f", paraTwoRuns[2].InnerText);

      Paragraph paraThree = Assert.IsType<Paragraph>(res[2]);
      List<Run> paraThreeRuns = paraThree.Descendants<Run>().ToList();
      Assert.Single(paraThreeRuns);
      Assert.Equal("Quack", paraThreeRuns[0].InnerText);
      AssertBold(paraThreeRuns[0]);
      AssertItalic(paraThreeRuns[0]);
      AssertUnderline(paraThreeRuns[0]);
   }

   [Fact]
   public void GivenNoTopLevelElements_RendersItInParagraph()
   {
      const string html = "Meow<b>Woof<u>Quack<i>Moo</i></u></b>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Single(res);
      Paragraph paragraph = Assert.IsType<Paragraph>(res[0]);
      Assert.Equal("MeowWoofQuackMoo", paragraph.InnerText);
      List<Run> runs = paragraph.Descendants<Run>().ToList();

      AssertBold(runs[1]);
      AssertBold(runs[2]);
      AssertUnderline(runs[2]);
      AssertBold(runs[3]);
      AssertUnderline(runs[3]);
      AssertItalic(runs[3]);
   }

   [Fact]
   public void GivenStartOfHtmlDoesntContainTopLevelElement_RendersItInAParagraph()
   {
      const string html = "Meow<b>Woof<u>Quack<i>Moo</i></u></b><p>Second paragraph</p>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Equal(2, res.Count);
      Assert.Equal("MeowWoofQuackMoo", res[0].InnerText);
      Assert.Equal("Second paragraph", res[1].InnerText);
   }

   [Fact]
   public void GivenTextNotContainedWithinParagraphMidText_RendersItInAParagraph()
   {
      const string html = "<p>Opening paragraph</p>Meow<b>Woof<u>Quack<i>Moo</i></u></b><p>Second paragraph</p>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Equal("Opening paragraph", res[0].InnerText);
      Assert.Equal("MeowWoofQuackMoo", res[1].InnerText);
      Assert.Equal("Second paragraph", res[2].InnerText);
   }

   [Fact]
   public void GivenDivsAsParagraphs_RenderAsParagraphs()
   {
      const string html = "<div>Meow</div><div>Woof</div>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);

      Assert.Equal(2, res.Count);
      Assert.Equal("Meow", res[0].InnerText);
      Assert.Equal("Woof", res[1].InnerText);
   }

   [Fact]
   public void GivenDivsWithNestedElements_RenderNestedElements()
   {
      const string html = "<div>meow</div><div><ul><li>woof</li></ul></div>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);
      Assert.Equal(2, res.Count);
      AssertListIsType(_builder.GetCurrentDocument(), 0, NumberFormatValues.Bullet);

      Paragraph paraOne = Assert.IsType<Paragraph>(res[0]);
      Assert.Equal("meow", paraOne.InnerText);

      Paragraph paraTwo = Assert.IsType<Paragraph>(res[1]);
      Assert.Equal("woof", paraTwo.InnerText);
   }

   [Fact]
   public void GivenSpanWithAttributesAroundText_RenderParagraphWithoutThem()
   {
      const string html = "<div><span style='color:#ff69b4'>meow</span></div>";
      List<OpenXmlElement> res = HtmlToElements.Convert(_builder, html);
      Assert.Single(res);
      Assert.Equal("meow", res[0].InnerText);
   }
}
