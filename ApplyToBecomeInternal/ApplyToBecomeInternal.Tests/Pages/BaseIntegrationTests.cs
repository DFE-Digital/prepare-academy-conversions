using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public abstract partial class BaseIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>, IDisposable
	{
		protected readonly IntegrationTestingWebApplicationFactory _factory;
		private readonly ITestOutputHelper _outputHelper;
		private readonly IBrowsingContext _browsingContext;
		protected readonly Fixture _fixture;

		protected BaseIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper outputHelper)
		{
			_factory = factory;
			_outputHelper = outputHelper;
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
			try
			{
				var anchors = Document.QuerySelectorAll("a");
				var link = (index == null
						? anchors.Single(a => a.TextContent.Contains(linkText))
						: anchors.Where(a => a.TextContent.Contains(linkText)).ElementAt(index.Value))
					as IHtmlAnchorElement;

				return await link.NavigateAsync();
			}
			catch
			{
				_outputHelper?.WriteLine(Document.TextContent);
				throw;
			}
		}

		public async Task NavigateDataTestAsync(string dataTest)
		{
			var anchors = Document.QuerySelectorAll($"[data-test='{dataTest}']").First() as IHtmlAnchorElement;
			await anchors.NavigateAsync();
		}

		protected (RadioButton, RadioButton) RandomRadioButtons(string id, params string[] values)
		{
			var keyPairs = values.Select((v, i) => new KeyValuePair<int, string>(i, v)).ToDictionary(kv => kv.Key + 1, kv => kv.Value);
			var selectedPosition = new Random().Next(0, keyPairs.Count);
			var selected = keyPairs.ElementAt(selectedPosition);
			keyPairs.Remove(selected.Key);
			var toSelect = keyPairs.ElementAt(new Random().Next(0, keyPairs.Count));
			return (
				new RadioButton { Id = Id(id, selected.Key), Value = selected.Value },
				new RadioButton { Id = Id(id, toSelect.Key), Value = toSelect.Value });

			static string Id(string name, int position)
			{
				if (position == 1)
				{
					return $"#{name}";
				}

				return $"#{name}-{position}";
			}
		}

		protected class RadioButton
		{
			public string Value { get; set; }
			public string Id { get; set; }
		}

		private IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
		{
			var config = AngleSharp.Configuration.Default
				.WithRequester(new HttpClientRequester(httpClient))
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

			return BrowsingContext.New(config);
		}

		public IDocument Document => _browsingContext.Active;

		public IBrowsingContext Context => _browsingContext;

		public void Dispose()
		{
			_factory.Reset();
		}
	}
}