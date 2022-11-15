using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using AutoFixture;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		protected readonly ITestOutputHelper _outputHelper;
		private readonly IBrowsingContext _browsingContext;
		protected readonly Fixture _fixture;

		private const int PageLoadTimeout = 30000;

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
				Stopwatch timer = Stopwatch.StartNew();
				await Task.WhenAny(new[] { Document.WaitForReadyAsync(), Task.Delay(PageLoadTimeout) });
				timer.Stop();
				if (timer.ElapsedMilliseconds >= PageLoadTimeout) _outputHelper.WriteLine("Page load timeout elapsed - page probably not loaded!");

				IHtmlCollection<IElement> anchors = Document.QuerySelectorAll("a");
				IHtmlAnchorElement link = (index == null
						? anchors.Single(a => a.TextContent.Contains(linkText))
						: anchors.Where(a => a.TextContent.Contains(linkText)).ElementAt(index.Value))
					as IHtmlAnchorElement;

				return await link.NavigateAsync();
			}
			catch
			{
				_outputHelper.WriteLine("NavigateAsync Exception!");
				throw;
			}
		}

		public async Task NavigateDataTestAsync(string dataTest)
		{
			try
			{
				await Task.WhenAny(new[] { Document.WaitForReadyAsync(), Task.Delay(PageLoadTimeout) });

				var anchors = Document.QuerySelectorAll($"[data-test='{dataTest}']").First() as IHtmlAnchorElement;
				await anchors.NavigateAsync();
			}
			catch
			{
				_outputHelper.WriteLine("NavigateDataTestAsync Exception!");
				throw;
			}
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
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
				.WithTemporaryCookies();

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