using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Services
{
	public class HtmlToOpenXmlVisitor
	{
		private readonly MainDocumentPart _mainDocumentPart;
		private readonly ParagraphProperties _paragraphProperties;
		private readonly RunProperties _runProperties;
		private readonly List<AbstractNum> _abstractNums = new List<AbstractNum>();
		private readonly List<NumberingInstance> _numberingInstances = new List<NumberingInstance>();
		private Stack<Run> _runs = new Stack<Run>();
		private static string[] _tags = new[] { "ol", "ul", "p" };

		public HtmlToOpenXmlVisitor(MainDocumentPart mainDocumentPart, ParagraphProperties paragraphProperties, RunProperties runProperties)
		{
			_mainDocumentPart = mainDocumentPart;
			_paragraphProperties = paragraphProperties;
			_runProperties = runProperties;
		}

		public ICollection<OpenXmlElement> OpenXmlElements { get; } = new List<OpenXmlElement>();

		public void Visit(IHtmlDocument document)
		{
			var groupedElements = GroupElements(document);
			foreach (INode child in groupedElements.SelectMany(l => l))
			{
				child.RemoveFromParent();
			}
			ReconstuctDocumentBody(document, groupedElements);

			VisitCore(document.Body);

			foreach (var abstractNum in _abstractNums)
			{
				_mainDocumentPart.NumberingDefinitionsPart.Numbering.Append(abstractNum);
			}
			foreach (var numberingInstance in _numberingInstances)
			{
				_mainDocumentPart.NumberingDefinitionsPart.Numbering.Append(numberingInstance);
			}
		}

		private static List<List<INode>> GroupElements(IHtmlDocument document)
		{
			var groupedElements = new List<List<INode>>();
			bool startNewList = true;
			foreach (INode child in document.Body.ChildNodes)
			{
				if (child is IElement element && !_tags.Contains(element.LocalName) || child is IText)
				{
					if (startNewList)
					{
						groupedElements.Add(new List<INode>());
						startNewList = false;
					}
					groupedElements.Last().Add(child);
				}
				else
				{
					groupedElements.Add(new List<INode> { child });
					startNewList = true;
				}
			}
			return groupedElements;
		}

		private static void ReconstuctDocumentBody(IHtmlDocument document, List<List<INode>> newNodeList)
		{
			foreach (var nodes in newNodeList)
			{
				if (nodes.Count == 1 && _tags.Contains(nodes.First().NodeName, StringComparer.OrdinalIgnoreCase))
				{
					document.Body.Append(nodes.First());
				}
				else
				{
					var paragraph = document.CreateElement("p");
					paragraph.AppendNodes(nodes.ToArray());
					document.Body.AppendNodes(paragraph);
				}
			}
		}

		private void VisitCore(INode node, bool hasRun = false)
		{
			foreach (var childNode in node.ChildNodes)
			{
				switch (childNode)
				{
					case IElement orderedList when orderedList.LocalName == "ol":
						VisitOrderedList(orderedList);
						break;
					case IElement unorderedList when unorderedList.LocalName == "ul":
						VisitUnorderedList(unorderedList);
						break;
					case IElement listItem when listItem.LocalName == "li":
						VisitListItem(listItem);
						break;
					case IElement paragraph when paragraph.LocalName == "p":
						VisitParagraph(paragraph);
						break;
					case IElement bold when bold.LocalName == "b":
						VisitBold(bold, hasRun);
						break;
					case IElement italic when italic.LocalName == "i":
						VisitItalic(italic, hasRun);
						break;
					case IElement underline when underline.LocalName == "u":
						VisitUnderline(underline, hasRun);
						break;
					case IText text:
						VisitText(text, hasRun);
						break;
					default:
						VisitCore(childNode);
						break;
				}
			}
		}

		private void VisitOrderedList(INode orderedList)
		{
			AddNumberingDefinition(NumberFormatValues.Decimal, "%1.");
			VisitCore(orderedList);
		}

		private void VisitUnorderedList(INode unorderedList)
		{
			AddNumberingDefinition(NumberFormatValues.Bullet, "•");
			VisitCore(unorderedList);
		}

		private void VisitListItem(INode listItem)
		{
			var properties = _paragraphProperties.CloneNode(true);
			properties.AppendChild(new NumberingProperties(
				new NumberingLevelReference() { Val = 0 },
				new NumberingId() { Val = _abstractNums.Count }));
			OpenXmlElements.Add(new Paragraph(properties));
			VisitCore(listItem);
		}

		private void VisitParagraph(INode paragraph)
		{
			OpenXmlElements.Add(new Paragraph(_paragraphProperties.CloneNode(true)));
			VisitCore(paragraph);
		}

		private void VisitBold(INode bold, bool hasRun)
		{
			UseRun((run) =>
			{
				run.RunProperties.Bold = new Bold();
				VisitCore(bold, true);
			}, hasRun);
		}

		private void VisitItalic(INode italic, bool hasRun)
		{
			UseRun((run) =>
			{
				run.RunProperties.Italic = new Italic();
				VisitCore(italic, true);
			}, hasRun);
		}

		private void VisitUnderline(INode underline, bool hasRun)
		{
			UseRun((run) =>
			{
				run.RunProperties.Underline = new Underline
				{
					Val = UnderlineValues.Single
				};
				VisitCore(underline, true);
			}, hasRun);
		}

		private void VisitText(IText text, bool hasRun)
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
						new LevelText() { Val = levelText })
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
