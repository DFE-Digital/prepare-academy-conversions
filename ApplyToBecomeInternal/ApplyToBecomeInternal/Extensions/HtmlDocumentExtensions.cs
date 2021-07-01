using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Services;

namespace ApplyToBecomeInternal.Extensions
{
	public static class HtmlDocumentExtensions
	{
		public static void Accept(this IHtmlDocument document, HtmlToOpenXmlVisitor visitor)
		{
			visitor.Visit(document);
		}
	}
}