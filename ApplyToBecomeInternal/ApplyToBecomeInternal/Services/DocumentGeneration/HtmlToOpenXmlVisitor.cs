using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Services.WordDocument
{
	public class HtmlToOpenXmlVisitor
	{
		private readonly MainDocumentPart _mainDocumentPart;
		private readonly ParagraphProperties _paragraphProperties;
		private readonly RunProperties _runProperties;
		private readonly List<AbstractNum> _abstractNums = new List<AbstractNum>();
		private readonly List<NumberingInstance> _numberingInstances = new List<NumberingInstance>();
		private readonly Stack<Run> _runs = new Stack<Run>();

		public HtmlToOpenXmlVisitor(MainDocumentPart mainDocumentPart, ParagraphProperties paragraphProperties, RunProperties runProperties)
		{
			_mainDocumentPart = mainDocumentPart;
			_paragraphProperties = paragraphProperties;
			_runProperties = runProperties;
		}

		public ICollection<OpenXmlElement> OpenXmlElements { get; } = new List<OpenXmlElement>();

		public void Visit(IHtmlBodyElement body)
		{
			body.Accept(this, false);

			foreach (var abstractNum in _abstractNums)
			{
				_mainDocumentPart.NumberingDefinitionsPart.Numbering.Append(abstractNum);
			}	
			foreach (var numberingInstance in _numberingInstances)
			{
				_mainDocumentPart.NumberingDefinitionsPart.Numbering.Append(numberingInstance);
			}
		}

		public void Visit(IHtmlOrderedListElement orderedList, bool hasRun)
		{
			AddNumberingDefinition(NumberFormatValues.Decimal, "%1.");
			orderedList.Accept(this, hasRun);
		}

		public void Visit(IHtmlUnorderedListElement unorderedList, bool hasRun)
		{
			AddNumberingDefinition(NumberFormatValues.Bullet, "•");
			unorderedList.Accept(this, hasRun);
		}

		public void Visit(IHtmlListItemElement listItem, bool hasRun)
		{
			var properties = _paragraphProperties.CloneNode(true);
			properties.AppendChild(new NumberingProperties(
				new NumberingLevelReference() { Val = 0 },
				new NumberingId() { Val = _abstractNums.Count }));
			OpenXmlElements.Add(new Paragraph(properties));
			listItem.Accept(this, hasRun);
		}

		public void Visit(IHtmlParagraphElement paragraph, bool hasRun)
		{
			OpenXmlElements.Add(new Paragraph(_paragraphProperties.CloneNode(true)));
			paragraph.Accept(this, hasRun);
		}

		public void Visit(IHtmlElement element, bool hasRun)
		{
			UseRun((run) =>
			{
				if (element.LocalName == "b")
				{
					run.RunProperties.Bold = new Bold();
				}
				else if (element.LocalName == "i")
				{
					run.RunProperties.Italic = new Italic();
				}
				else if (element.LocalName == "u")
				{
					run.RunProperties.Underline = new Underline
					{
						Val = UnderlineValues.Single
					};
				}
				element.Accept(this, true);
			}, hasRun);
		}

		public void Visit(IHtmlBreakRowElement _, bool hasRun)
		{
			UseRun((run) =>
			{
				run.Append(new Break());
			}, hasRun);
		}

		public void Visit(IText text, bool hasRun)
		{
			UseRun((run) =>
			{
				run.Append(new Text
				{
					Text = text.TextContent,
					Space = SpaceProcessingModeValues.Preserve
				});
			}, hasRun);
		}

		private void AddNumberingDefinition(NumberFormatValues numberFormatValues, string levelText)
		{
			_abstractNums.Add(
				new AbstractNum(
					new Level(
						new StartNumberingValue { Val = 1 },
						new NumberingFormat() { Val = numberFormatValues },
						new LevelJustification { Val = LevelJustificationValues.Left },
						new LevelText() { Val = levelText },
						new ParagraphProperties(
							new Indentation { Left = "720", Hanging = "360" }))
					{ LevelIndex = 0 })
				{ AbstractNumberId = _abstractNums.Count + 1 });
			_numberingInstances.Add(
				new NumberingInstance(
					new AbstractNumId() { Val = _numberingInstances.Count + 1 }
				)
				{ NumberID = _numberingInstances.Count + 1 });
		}

		private void UseRun(Action<Run> action, bool hasRun)
		{
			if (!hasRun)
			{
				_runs.Push(new Run(_runProperties.CloneNode(true)));
				OpenXmlElements.Last().Append(_runs.Peek());
			}
			action(_runs.Peek());
			if (!hasRun)
			{
				_runs.Pop();
			}
		}
	}
}
