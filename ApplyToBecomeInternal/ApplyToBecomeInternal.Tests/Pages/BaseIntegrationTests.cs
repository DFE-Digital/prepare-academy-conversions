using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using AutoFixture;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public abstract partial class BaseIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>
	{
		private readonly IntegrationTestingWebApplicationFactory _factory;
		private readonly IBrowsingContext _browsingContext;
		protected readonly Fixture _fixture;

		protected BaseIntegrationTests(IntegrationTestingWebApplicationFactory factory)
		{
			_factory = factory;
			var httpClient = factory.CreateClient();
			_browsingContext = CreateBrowsingContext(httpClient);
			_fixture = new Fixture();
		}

		public async Task<IDocument> OpenUrlAsync(string url)
		{
			return await _browsingContext.OpenAsync($"http://localhost{url}");
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

		private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
		{
			var config = AngleSharp.Configuration.Default
				.WithRequester(new HttpClientRequester(httpClient))
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

			return BrowsingContext.New(config);
		}

		public IDocument Document => _browsingContext.Active;
	}
}
