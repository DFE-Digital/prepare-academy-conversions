﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using AutoFixture;
using Dfe.PrepareConversions.Data.Features;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using Microsoft.FeatureManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages;

public abstract partial class BaseIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>, IDisposable
{
   protected readonly IntegrationTestingWebApplicationFactory _factory;
   protected readonly Fixture _fixture;
   private readonly PathFor _pathFor;

   protected BaseIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   {
      _factory = factory;
      _fixture = new Fixture();

      Mock<IFeatureManager> featureManager = new();
      featureManager.Setup(m => m.IsEnabledAsync("UseAcademisationApplication")).ReturnsAsync(true);
      _pathFor = new PathFor(featureManager.Object);

      Context = CreateBrowsingContext(factory.CreateClient());
      GetRoleCapabilities(["conversionRole", "transferRoles"]);
   }

   protected IDocument Document => Context.Active;

   protected IBrowsingContext Context { get; }


   public void Dispose()
   {
      _factory.Reset();
      GC.SuppressFinalize(this);
   }

   public static string BuildRequestAddress(string path)
   {
      return $"https://localhost{(path.StartsWith('/') ? path : $"/{path}")}";
   }

   protected async Task<IDocument> OpenAndConfirmPathAsync(string path, string expectedPath = null, string because = null)
   {
      var resultDocument = await Context.OpenAsync(BuildRequestAddress(path));

      Document.Url.Should().Be(BuildRequestAddress(expectedPath ?? path), because ?? "navigation should be successful");
      return resultDocument;
   }

   protected void VerifyElementDoesNotExist(string dataTest)
   {
      var anchors = Document.QuerySelectorAll($"[data-test='{dataTest}']").FirstOrDefault();
      Assert.Null(anchors);
   }

   protected void VerifyNullElement(string linkText)
   {
      var anchors = Document.QuerySelectorAll("a");
      var linkElement = anchors.SingleOrDefault(a => a.TextContent != null && a.TextContent.Contains(linkText));
      Assert.Null(linkElement); 
   }

   protected async Task NavigateAsync(string linkText, int? index = null)
   {
      IHtmlCollection<IElement> anchors = Document.QuerySelectorAll("a");
      var link = (index == null
            ? anchors.Single(a => a.TextContent.Contains(linkText))
            : anchors.Where(a => a.TextContent.Contains(linkText)).ElementAt(index.Value))
         as IHtmlAnchorElement;

      Assert.NotNull(link);
      await link.NavigateAsync();
   }

   protected static Task<IDocument> NavigateAsync(IDocument document, string linkText, int? index = null)
   {
      IHtmlCollection<IElement> anchors = document.QuerySelectorAll("a");
      IHtmlAnchorElement link = (index == null
            ? anchors.SingleOrDefault(a => a.TextContent.Contains(linkText))
            : anchors.Where(a => a.TextContent.Contains(linkText)).ElementAt(index.Value))
         as IHtmlAnchorElement;

      if (index == null && link == null)
      {
         throw new NullReferenceException($"Sequence Contains no matching element. Could not find <a> element with text '{linkText}'");
      }

      Assert.NotNull(link);
      return link.NavigateAsync();
   }

   protected async Task NavigateDataTestAsync(string dataTest)
   {
      IHtmlAnchorElement anchors = Document.QuerySelectorAll($"[data-test='{dataTest}']").First() as IHtmlAnchorElement;
      Assert.NotNull(anchors);

      await anchors.NavigateAsync();
   }

   protected void NavigateDataTestButton(string dataTest)
   {
      var buttons = Document.QuerySelectorAll($"[data-test='{dataTest}']").First() as IHtmlButtonElement;
      Assert.NotNull(buttons);

      buttons.DoClick();
   }

   protected IHtmlButtonElement GetDataTestButtonElement(string dataTest)
   {
      IHtmlButtonElement anchors = Document.QuerySelectorAll($"[data-test='{dataTest}']").FirstOrDefault() as IHtmlButtonElement;
      Assert.NotNull(anchors);

      return anchors;
   }

   protected static Task<IDocument> NavigateDataTestAsync(IDocument document, string dataTest)
   {
      IHtmlAnchorElement anchors = document.QuerySelectorAll($"[data-test='{dataTest}']").First() as IHtmlAnchorElement;
      Assert.NotNull(anchors);

      return anchors.NavigateAsync();
   }
   protected static void VerifyNullDataTest(IDocument document, string dataTest)
   {
      var anchor = document.QuerySelectorAll($"[data-test='{dataTest}']").FirstOrDefault();
      Assert.Null(anchor);
   }

   protected static (RadioButton, RadioButton) RandomRadioButtons(string id, params string[] values)
   {
      Dictionary<int, string> keyPairs = values.Select((v, i) => new KeyValuePair<int, string>(i, v)).ToDictionary(kv => kv.Key + 1, kv => kv.Value);
      int selectedPosition = new Random().Next(0, keyPairs.Count);
      KeyValuePair<int, string> selected = keyPairs.ElementAt(selectedPosition);
      keyPairs.Remove(selected.Key);
      KeyValuePair<int, string> toSelect = keyPairs.ElementAt(new Random().Next(0, keyPairs.Count));
      return (
         new RadioButton { Id = Id(id, selected.Key), Value = selected.Value },
         new RadioButton { Id = Id(id, toSelect.Key), Value = toSelect.Value });

      static string Id(string name, int position)
      {
         return position == 1 ? $"#{name}" : $"#{name}-{position}";
      }
   }

   private static IBrowsingContext CreateBrowsingContext(HttpClient httpClient)
   {
      IConfiguration config = AngleSharp.Configuration.Default
         .WithRequester(new HttpClientRequester(httpClient))
         .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true });

      return BrowsingContext.New(config);
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

      await buttonElement!.SubmitAsync();
   }

   protected static string CypressSelectorFor(string name)
   {
      return $"[data-cy='{name}']";
   }

   protected class RadioButton
   {
      public string Value { get; init; }
      public string Id { get; init; }
   }
}
