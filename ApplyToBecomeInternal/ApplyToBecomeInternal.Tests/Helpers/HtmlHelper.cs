using AngleSharp;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using System.Net.Http;

namespace ApplyToBecomeInternal.Tests.Helpers
{
	internal static class HtmlHelper
	{
		public static IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
		{
			var config = AngleSharp.Configuration.Default
				.WithRequester(new HttpClientRequester(httpClient))
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

			return BrowsingContext.New(config);
		}
	}
}
