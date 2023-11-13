using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Tests.Customisations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SponsoredProject;

public class SummaryIntegrationTests : BaseIntegrationTests
{
   public SummaryIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_render_summary_page_with_school_and_trust(EstablishmentResponse establishment, TrustSummaryResponse trustSummaryResponse)
   {
      establishment.OfstedLastInspection = DateTime.Now.ToString("dd-mm-yyyy");
      string ukprn = trustSummaryResponse.Data[0].Ukprn;
      _factory.AddGetWithJsonResponse($"/v4/establishment/urn/{establishment.Urn}", establishment);
      _factory.AddGetWithJsonResponse("/v4/trusts*", trustSummaryResponse);

      await OpenAndConfirmPathAsync($"/start-new-project/check-school-trust-details?ukprn={ukprn}&urn={establishment.Urn}");

      Document.QuerySelector<IHtmlElement>("[data-cy=school-name]")!.Text().Trim().Should()
         .Be(establishment.Name);
      Document.QuerySelector<IHtmlElement>("[data-cy=trust-name]")!.Text().Trim().Should()
         .Be(trustSummaryResponse.Data[0].Name);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_submit_and_redirect_to_listing(EstablishmentResponse establishment,
                                                           List<TrustSummary> trustSummaryResponse,
                                                           TrustDetail trustDetail)
   {
      establishment.OfstedLastInspection = DateTime.Now.ToString("dd-mm-yyyy");
      establishment.OpenDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
      string ukprn = trustSummaryResponse[0].Ukprn;
      _factory.AddGetWithJsonResponse($"/v4/establishment/urn/{establishment.Urn}", establishment);
      _factory.AddGetWithJsonResponse("/v4/trusts*", trustSummaryResponse);

      await OpenAndConfirmPathAsync($"/start-new-project/check-school-trust-details?ukprn={ukprn}&urn={establishment.Urn}");

      _factory.AddGetWithJsonResponse(@$"/v4/trusts/bulk*", trustDetail);
      _factory.AddAnyPostWithJsonRequest("/legacy/project/sponsored-conversion-project", "");

      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.Url.Should().Contain("project-list");
      Document.QuerySelector<IHtmlElement>("h1")!.Text().Should().Be("Manage an academy conversion");
   }
}
