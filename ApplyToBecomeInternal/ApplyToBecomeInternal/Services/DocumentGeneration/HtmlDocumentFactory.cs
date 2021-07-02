using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApplyToBecomeInternal.Services.WordDocument
{
	public static class HtmlDocumentFactory
	{
		private static string[] _tags = new[] { "ol", "ul", "p" };

		public static IHtmlDocument Create(string html)
		{
			var htmlParser = new HtmlParser();
			var sanitisedHtml = SanitiseHtml(html);
			var document = htmlParser.ParseDocument(sanitisedHtml);
			var groupedElements = GroupElements(document);
			foreach (INode child in groupedElements.SelectMany(l => l))
			{
				child.RemoveFromParent();
			}
			ReconstructDocumentBody(document, groupedElements);
			return document;
		}

		private static string SanitiseHtml(string html)
		{
			return Regex.Replace(html, @"<\/?(a|div|span)>", "");
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

		private static void ReconstructDocumentBody(IHtmlDocument document, List<List<INode>> newNodeList)
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
	}
}
