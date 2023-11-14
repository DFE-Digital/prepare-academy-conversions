using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academies.Contracts.V4.Establishments;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SponsoredProject;

public class SearchTrustIntegrationTests : BaseIntegrationTests
{
   public SearchTrustIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_show_error_when_no_trust_provided()
   {
      await OpenAndConfirmPathAsync("/start-new-project/trust-name");

      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]")!.Text().Trim().Should()
         .Be("Enter the Trust name, UKPRN or Companies House number");
   }

   [Fact]
   public async Task Should_show_trust_not_found_error()
   {
      await OpenAndConfirmPathAsync("/start-new-project/trust-name");
      string trustName = "faketrustname";

      _factory.AddGetWithJsonResponse("/establishments",
         new List<EstablishmentDto> { new() });

      Document.QuerySelector<IHtmlInputElement>("#SearchQuery")!.Value = trustName;
      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]")!.Text().Trim().Should()
         .Be("We could not find any trusts matching your search criteria");
   }
}
