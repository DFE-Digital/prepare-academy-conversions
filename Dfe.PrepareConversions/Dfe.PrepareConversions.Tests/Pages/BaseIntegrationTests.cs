using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using Dfe.PrepareConversions.Data.Features;
using AutoFixture;
using FluentAssertions;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace Dfe.PrepareConversions.Tests.Pages
{
   public abstract partial class BaseIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>, IDisposable
	{
		protected readonly IntegrationTestingWebApplicationFactory _factory;
		protected readonly Fixture _fixture;
		private readonly PathFor _pathFor;

		protected BaseIntegrationTests(IntegrationTestingWebApplicationFactory factory)
		{
			_factory = factory;
         _fixture = new Fixture();

         var featureManager = new Mock<IFeatureManager>();
         featureManager.Setup(m => m.IsEnabledAsync(("UseAcademisation"))).ReturnsAsync(true);
         featureManager.Setup(m => m.IsEnabledAsync(("UseAcademisationApplication"))).ReturnsAsync(false);
         _pathFor = new PathFor(featureManager.Object);

			Context = CreateBrowsingContext(factory.CreateClient());
		}

      private static string BuildRequestAddress(string path)
      {
         return $"https://localhost{(path.StartsWith('/') ? path : $"/{path}")}";
      }

      protected async Task OpenAndConfirmPathAsync(string path, string expectedPath = null, string because = null)
      {
         await Context.OpenAsync(BuildRequestAddress(path));

         Document.Url.Should().Be(BuildRequestAddress(expectedPath ?? path), because: because ?? "navigation should be successful");
      }

		public async Task<IDocument> NavigateAsync(string linkText, int? index = null)
		{
			var anchors = Document.QuerySelectorAll("a");
			var link = (index == null
					? anchors.Single(a => a.TextContent.Contains(linkText))
					: anchors.Where(a => a.TextContent.Contains(linkText)).ElementAt(index.Value))
				as IHtmlAnchorElement;

			Assert.NotNull(link);
			return await link.NavigateAsync();
		}

		public async Task NavigateDataTestAsync(string dataTest)
		{
			var anchors = Document.QuerySelectorAll($"[data-test='{dataTest}']").First() as IHtmlAnchorElement;
			Assert.NotNull(anchors);

			await anchors.NavigateAsync();
		}

		protected static (RadioButton, RadioButton) RandomRadioButtons(string id, params string[] values)
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
				return position == 1 ? $"#{name}" : $"#{name}-{position}";
			}
		}

		protected class RadioButton
		{
			public string Value { get; set; }
			public string Id { get; set; }
		}

		private static IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
		{
			var config = AngleSharp.Configuration.Default
				.WithRequester(new HttpClientRequester(httpClient))
				.WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

			return BrowsingContext.New(config);
		}

		public IDocument Document => Context.Active;

		public IBrowsingContext Context { get; }

		public void Dispose()
		{
			_factory.Reset();
			GC.SuppressFinalize(this);
		}

      protected IEnumerable<IElement> ElementsWithText(string tag, string content)
      {
         return Document.QuerySelectorAll(tag).Where(t => t.TextContent.Contains(content));
      }

      protected IHtmlInputElement InputWithId(string inputId)
      {
         IHtmlInputElement inputElement = Document.QuerySelector<IHtmlInputElement>(inputId.StartsWith('#') ? inputId : $"#{inputId}");
         inputElement.Should().NotBeNull(because: $"element with ID {inputId} should be available on the page ({Document.Url})");
         return inputElement;
      }

      protected async Task ClickCommonSubmitButtonAsync()
      {
         IHtmlButtonElement buttonElement = Document.QuerySelector<IHtmlButtonElement>("button[data-cy=\"select-common-submitbutton\"]");

         buttonElement.Should()
            .NotBeNull(because: $"A button with the common submit button selector (data-cy=\"select-common-submitbutton\") is expected on this page ({Document.Url})");

         await buttonElement.SubmitAsync();
      }
   }
}