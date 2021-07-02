using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Services.WordDocument;
using AutoFixture.Xunit2;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Services
{
	public class HtmlToOpenXmlVisitorTests : IDisposable
	{
		private readonly MemoryStream _ms;
		private readonly WordprocessingDocument _wordDoc;
		private readonly HtmlToOpenXmlVisitor _visitor;

		public HtmlToOpenXmlVisitorTests()
		{
			_ms = CreateBlankDocumentStream();
			_wordDoc = WordprocessingDocument.Open(_ms, true);

			var body = _wordDoc.MainDocumentPart.Document.Body;
			var paragraph = body.GetFirstChild<Paragraph>();
			var run = paragraph.GetFirstChild<Run>();

			_visitor = new HtmlToOpenXmlVisitor(_wordDoc.MainDocumentPart, paragraph.ParagraphProperties, run.RunProperties);
		}

		[Theory, AutoData]
		public void Should_be_a_paragraph_with_text_when_paragraph_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<p>{text}</p>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			var paragraph = _wordDoc.MainDocumentPart.Document.Body.FirstChild as Paragraph;
			element.FirstChild.Should().BeEquivalentTo(paragraph.ParagraphProperties);
			var run = paragraph.LastChild as Run;
			element.LastChild.FirstChild.Should().BeEquivalentTo(run.RunProperties);
			element.LastChild.LastChild.InnerText.Should().Be(text);
		}

		[Theory, AutoData]
		public void Should_be_a_paragraph_with_two_text_and_a_break_when_line_break_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<p>{text}<br>{text}</p>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			var childElements = element.ChildElements.ToArray();
			childElements[1].LastChild.Should().BeOfType<Text>();
			childElements[2].LastChild.Should().BeOfType<Break>();
			childElements[3].LastChild.Should().BeOfType<Text>();
		}

		[Theory, AutoData]
		public void Should_be_mutliple_paragraphs_when_multiple_paragraph_tag_present(string text, string text2)
		{
			var document = HtmlDocumentFactory.Create($"<p>{text}</p><p>{text2}</p>");

			document.Accept(_visitor);

			_visitor.OpenXmlElements.Count.Should().Be(2);
			var element = _visitor.OpenXmlElements.Last();
			element.LastChild.LastChild.InnerText.Should().Be(text2);
		}

		[Theory, AutoData]
		public void Should_be_bold_when_bold_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<p><b>{text}</b></p>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			element.LastChild.GetFirstChild<RunProperties>().Bold.Should().NotBeNull();
		}

		[Theory, AutoData]
		public void Should_be_italic_when_italic_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<p><b>{text}</b><i>{text}</i></p>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			element.LastChild.GetFirstChild<RunProperties>().Italic.Should().NotBeNull();
		}

		[Theory, AutoData]
		public void Should_be_underlined_when_underline_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<p><b>{text}</b><i>{text}</i><u>{text}</u></p>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			element.LastChild.GetFirstChild<RunProperties>().Underline.Should().NotBeNull();
			element.LastChild.GetFirstChild<RunProperties>().Underline.Val.Should().Be(UnderlineValues.Single);
		}

		[Theory, AutoData]
		public void Should_apply_all_formatting_when_multiple_tags_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<p><b><i><u>{text}</u></i></b></p>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			element.LastChild.GetFirstChild<RunProperties>().Bold.Should().NotBeNull();
			element.LastChild.GetFirstChild<RunProperties>().Italic.Should().NotBeNull();
			element.LastChild.GetFirstChild<RunProperties>().Underline.Should().NotBeNull();
			element.LastChild.GetFirstChild<RunProperties>().Underline.Val.Should().Be(UnderlineValues.Single);
		}

		[Theory, AutoData]
		public void Should_be_numbering_when_ordered_list_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<ol><li>{text}</li></ol>");

			document.Accept(_visitor);

			var numbering = _wordDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering;
			var abstractNum = numbering.GetFirstChild<AbstractNum>();
			var numberingInstance = numbering.GetFirstChild<NumberingInstance>();

			abstractNum.Should().NotBeNull();
			numberingInstance.Should().NotBeNull();
		}

		[Theory, AutoData]
		public void Should_be_numbering_when_unordered_list_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<ul><li>{text}</li></ul>");

			document.Accept(_visitor);

			var numbering = _wordDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering;
			var abstractNum = numbering.GetFirstChild<AbstractNum>();
			var numberingInstance = numbering.GetFirstChild<NumberingInstance>();

			abstractNum.Should().NotBeNull();
			numberingInstance.Should().NotBeNull();
		}

		[Theory, AutoData]
		public void Should_contain_paragraphs_when_list_items_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<ul><li>{text}</li></ul>");

			document.Accept(_visitor);

			var listItemProperties = _visitor.OpenXmlElements.First().GetFirstChild<ParagraphProperties>();
			var paragraphNumberingProperties = listItemProperties.GetFirstChild<NumberingProperties>();
			var numberingLevelReference = paragraphNumberingProperties.GetFirstChild<NumberingLevelReference>();
			var numberingId = paragraphNumberingProperties.GetFirstChild<NumberingId>();

			numberingLevelReference.Should().NotBeNull();
			numberingId.Should().NotBeNull();
		}

		[Theory, AutoData]
		public void Should_be_a_numbering_paragraph_with_a_break_when_line_break_tag_present(string text)
		{
			var document = HtmlDocumentFactory.Create($"<ul><li>{text}<br>{text}</li></ul>");

			document.Accept(_visitor);

			var element = _visitor.OpenXmlElements.Last();
			var childElements = element.ChildElements.ToArray();
			childElements[1].LastChild.Should().BeOfType<Text>();
			childElements[2].LastChild.Should().BeOfType<Break>();
			childElements[3].LastChild.Should().BeOfType<Text>();
		}

		[Theory, AutoData]
		public void Should_handle_markup_when_tags_are_missing(string text)
		{
			var document = HtmlDocumentFactory.Create($"{text}<p>{text}<p><b>{text}</b>");

			document.Accept(_visitor);

			var elements = _visitor.OpenXmlElements;
			elements.Count.Should().Be(3);
			elements.First().GetFirstChild<Run>().GetFirstChild<RunProperties>().Bold.Should().BeNull();
			elements.Last().GetFirstChild<Run>().GetFirstChild<RunProperties>().Bold.Should().NotBeNull();
		}

		[Theory, AutoData]
		public void Should_handle_complex_markup(string text)
		{
			var html = @$"<i>{text}</i> {text} <u>{text}</u> {text}<p>{text} <b>{text}</b> {text}</p><p>{text} <b>{text}</b> {text}</p>";
			var document = HtmlDocumentFactory.Create(html);

			document.Accept(_visitor);

			var elements = _visitor.OpenXmlElements;
			elements.Count.Should().Be(3);
		}

		private static MemoryStream CreateBlankDocumentStream()
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "blank.docx");
			using var templateStream = File.OpenRead(filePath);
			var ms = new MemoryStream();
			templateStream.CopyTo(ms);
			return ms;
		}

		public void Dispose()
		{
			_wordDoc.Close();
			_ms.Dispose();
		}
	}
}
