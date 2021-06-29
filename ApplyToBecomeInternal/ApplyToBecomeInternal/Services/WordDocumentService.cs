using AngleSharp.Html.Parser;
using ApplyToBecomeInternal.Extensions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ApplyToBecomeInternal.Services
{
	public class WordDocumentService
	{
		public byte[] Create<TDocument>(string template, TDocument document)
		{
			using var ms = CreateMemoryStream(template);
			using var wordDoc = WordprocessingDocument.Open(ms, true);
			var texts = GetAllText();
			var properties = GetProperties<TDocument>();

			foreach (var text in texts)
			{
				var property = properties.FirstOrDefault(p => text.InnerText.Contains($"{p.GetCustomAttribute<DocumentTextAttribute>().Placeholder}", StringComparison.OrdinalIgnoreCase));
				if (property != null)
				{
					var replaced = false;
					var attribute = property.GetCustomAttribute<DocumentTextAttribute>();
					if (attribute.IsRichText)
					{
						var value = property.GetValue(document).ToStringOrDefault(attribute.Default);
						var htmlDocument = new HtmlParser().ParseDocument(value);
						if (htmlDocument.Body.ChildElementCount > 0)
						{
							var run = text.Parent as Run;
							var paragraph = run.Parent as Paragraph;
							var visitor = new HtmlToOpenXmlVisitor(wordDoc.MainDocumentPart, paragraph.ParagraphProperties, run.RunProperties);
							htmlDocument.Accept(visitor);
							var parent = paragraph.Parent;
							parent.RemoveChild(paragraph);
							foreach (var element in visitor.OpenXmlElements)
							{
								parent.Append(element);
							}
							replaced = true;
						}
					}
					if (!replaced)
					{
						text.Text = text.Text.Replace($"{attribute.Placeholder}", property.GetValue(document).ToStringOrDefault(attribute.Default), StringComparison.OrdinalIgnoreCase);
					}					
				}
			}

			wordDoc.Close();
			return ms.ToArray();

			IEnumerable<Text> GetAllText()
			{
				return wordDoc.MainDocumentPart.Document.Body.Descendants<Text>()
					.Concat(wordDoc.MainDocumentPart.FooterParts.SelectMany(fp => fp.Footer.Descendants<Text>()))
					.ToHashSet();
			}
		}

		private MemoryStream CreateMemoryStream(string template)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains(template, StringComparison.OrdinalIgnoreCase));
			using var templateStream = assembly.GetManifestResourceStream(resourceName);
			var ms = new MemoryStream();
			templateStream.CopyTo(ms);
			return ms;
		}

		private PropertyInfo[] GetProperties<TDocument>()
		{
			return typeof(TDocument).GetProperties()
				.Where(p => p.GetCustomAttribute<DocumentTextAttribute>() != null)
				.ToArray();
		}

		public class DocumentTextAttribute : Attribute
		{
			private readonly string _placeholder;

			public DocumentTextAttribute(string placeholder)
			{
				_placeholder = placeholder;
			}

			public string Placeholder => $"[{_placeholder}]";

			public string Default { get; set; }
			public bool IsRichText { get; set; }
		}
	}
}
