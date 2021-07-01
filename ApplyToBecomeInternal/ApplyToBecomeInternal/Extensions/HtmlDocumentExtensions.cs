using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Services.WordDocument;

namespace ApplyToBecomeInternal.Extensions
{
	public static class HtmlDocumentExtensions
	{
		public static void Accept(this IHtmlDocument document, HtmlToOpenXmlVisitor visitor)
		{
			visitor.Visit((IHtmlBodyElement)document.Body);
		}

		public static void Accept(this IElement element, HtmlToOpenXmlVisitor visitor, bool hasRun)
		{
			foreach (var node in element.ChildNodes)
			{
				visitor.Visit((dynamic)node, hasRun);
			}
		}
	}
}