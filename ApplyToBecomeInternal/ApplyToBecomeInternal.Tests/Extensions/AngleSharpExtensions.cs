using AngleSharp.Html.Dom;
using System.Linq;
using System.Threading.Tasks;

namespace AngleSharp.Dom
{
	public static class AngleSharpExtensions
	{
		public static async Task<IDocument> NavigateAsync(this IHtmlDocument document, string linkText)
		{
			var anchors = document.QuerySelectorAll("a");
			var link = anchors.Single(a => a.TextContent.Contains(linkText)) as IHtmlAnchorElement;
			return await link.NavigateAsync();
		}
	}
}
