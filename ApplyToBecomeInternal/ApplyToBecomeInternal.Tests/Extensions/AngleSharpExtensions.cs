using AngleSharp.Html.Dom;
using AngleSharp.Io;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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

		public static async Task<IHtmlDocument> GetDocumentAsync(this IBrowsingContext browsingContext, HttpResponseMessage response)
		{
			var content = await response.Content.ReadAsStringAsync();
			var document = await browsingContext.OpenAsync(ResponseFactory, CancellationToken.None);

			return (IHtmlDocument)document;

			void ResponseFactory(VirtualResponse htmlResponse)
			{
				htmlResponse
					.Address(response.RequestMessage.RequestUri)
					.Status(response.StatusCode);

				MapHeaders(response.Headers);
				MapHeaders(response.Content.Headers);

				htmlResponse.Content(content);

				void MapHeaders(HttpHeaders headers)
				{
					foreach (var header in headers)
					{
						foreach (var value in header.Value)
						{
							htmlResponse.Header(header.Key, value);
						}
					}
				}
			}
		}
	}
}
