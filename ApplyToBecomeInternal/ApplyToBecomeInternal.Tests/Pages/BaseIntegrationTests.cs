using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Tests.Helpers;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public abstract class BaseIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>
	{
		protected readonly IntegrationTestingWebApplicationFactory Factory;
		protected readonly HttpClient HttpClient;
		protected readonly IBrowsingContext BrowsingContext;

		protected BaseIntegrationTests(IntegrationTestingWebApplicationFactory factory)
		{
			Factory = factory;
			HttpClient = factory.CreateClient();
			BrowsingContext = HtmlHelper.CreateBrowsingContext(HttpClient);
		}

		public async Task<IDocument> OpenUrlAsync(string url)
		{
			return await BrowsingContext.OpenAsync($"http://localhost{url}");
		}

		public async Task<IDocument> NavigateAsync(string linkText, int? index = null)
		{
			var anchors = Document.QuerySelectorAll("a");
			var link = (index == null
				? anchors.Single(a => a.TextContent.Contains(linkText))
				: anchors.Where(a => a.TextContent.Contains(linkText)).ElementAt(index.Value))
				as IHtmlAnchorElement;
			
			return await link.NavigateAsync();
		}

		public IDocument Document => BrowsingContext.Active;
	}
}
