using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Tests.Customisations;
using FluentAssertions;
using System;
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
   public async Task Should_render_summary_page_with_school_and_trust(EstablishmentDto establishment, TrustDtoResponse TrustDtoResponse)
   {
      establishment.OfstedLastInspection = DateTime.Now.ToString("dd-mm-yyyy");
      string ukprn = TrustDtoResponse.Data[0].Ukprn;
      _factory.AddGetWithJsonResponse($"/v4/establishment/urn/{establishment.Urn}", establishment);
      _factory.AddGetWithJsonResponse("/v4/trusts*", TrustDtoResponse);

      await OpenAndConfirmPathAsync($"/start-new-project/check-school-trust-details?ukprn={ukprn}&urn={establishment.Urn}");

      Document.QuerySelector<IHtmlElement>("[data-cy=school-name]")!.Text().Trim().Should()
         .Be(establishment.Name);
      Document.QuerySelector<IHtmlElement>("[data-cy=trust-name]")!.Text().Trim().Should()
         .Be(TrustDtoResponse.Data[0].Name);
   }

}
