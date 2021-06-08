using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Tests.Helpers
{
	internal static class HtmlHelper
	{
		public static async Task<IHtmlDocument> GetDocumentAsync(HttpClient client, HttpResponseMessage response)
		{
			var requester = new HttpClientRequester(client);
			var config = AngleSharp.Configuration.Default.WithRequester(requester).WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

						var content = await response.Content.ReadAsStringAsync();
			var document = await BrowsingContext.New(config)
				.OpenAsync(ResponseFactory, CancellationToken.None);
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
