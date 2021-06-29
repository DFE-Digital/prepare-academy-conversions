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
		private Stack<Run> _runs = new Stack<Run>();
		private int _currentNumberingId = 0;
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

		public void VisitOrderedList(INode orderedList)
		{
			_currentNumberingId++;
			var numbering = _mainDocumentPart.NumberingDefinitionsPart.Numbering;
			numbering.Append(
				new AbstractNum(
					new Level(
						new StartNumberingValue { Val = 1 },
						new NumberingFormat { Val = NumberFormatValues.Decimal }) 
					{ LevelIndex = 0 })
				{ AbstractNumberId = _currentNumberingId });
			numbering.Append(
				new NumberingInstance(
					new AbstractNumId() { Val = _currentNumberingId }
				)
				{ NumberID = _currentNumberingId });
			VisitCore(orderedList);
		}

		public void VisitUnorderedList(INode unorderedList)
		{
			_currentNumberingId++;
			var numbering = _mainDocumentPart.NumberingDefinitionsPart.Numbering;
			numbering.Append(
				new AbstractNum(
					new Level(
						new NumberingFormat() { Val = NumberFormatValues.Bullet },
						new LevelText() { Val = "•" })
					{ LevelIndex = 0 })
				{ AbstractNumberId = _currentNumberingId });
			numbering.Append(
				new NumberingInstance(
					new AbstractNumId() { Val = _currentNumberingId }
				)
				{ NumberID = _currentNumberingId });

			VisitCore(unorderedList);
		}

		public void VisitListItem(INode listItem)
		{
			_runs.Push(new Run(_runProperties.CloneNode(true)));
			var properties = _paragraphProperties.CloneNode(true);
			properties.AppendChild(new NumberingProperties(
				new NumberingLevelReference() { Val = 0 },
				new NumberingId() { Val = _currentNumberingId }));
			OpenXmlElements.Add(new Paragraph(properties, _runs.Peek()));
			VisitCore(listItem);
		}

		private void VisitParagraph(INode paragraph)
		{
			OpenXmlElements.Add(new Paragraph(_paragraphProperties.CloneNode(true)));
			VisitCore(paragraph);
		}

		public void VisitBold(INode bold, bool hasRun)
		{
			if (!hasRun)
			{
				AddRun();
			}
			_runs.Peek().RunProperties.Bold = new Bold();
			VisitCore(bold, true);
			if (!hasRun)
			{
				_runs.Pop();
			}
		}

		public void VisitItalic(INode italic, bool hasRun)
		{
			if (!hasRun)
			{
				AddRun();
			}
			_runs.Peek().RunProperties.Italic = new Italic();
			VisitCore(italic, true);
			if (!hasRun)
			{
				_runs.Pop();
			}
		}

		public void VisitUnderline(INode underline, bool hasRun)
		{
			if (!hasRun)
			{
				AddRun();
			}
			_runs.Peek().RunProperties.Underline = new Underline
			{
				Val = UnderlineValues.Single
			};
			VisitCore(underline, true);
			if (!hasRun)
			{
				_runs.Pop();
			}
		}

		private void VisitText(IText text, bool hasRun)
		{
			if (!hasRun)
			{
				AddRun();
			}
			_runs.Peek().Append(new Text
				{
					Text = text.TextContent,
					Space = SpaceProcessingModeValues.Preserve
				});
			if (!hasRun)
			{
				_runs.Pop();
			}
		}

		private void AddRun()
		{
			_runs.Push(new Run(_runProperties.CloneNode(true)));
			OpenXmlElements.Last().Append(_runs.Peek());
		}
	}
}
