using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academies.Contracts.V4.Establishments;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SponsoredProject;

public class SearchSchoolIntegrationTests : BaseIntegrationTests
{
   public SearchSchoolIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_link_to_school_search()
   {
      await OpenAndConfirmPathAsync("/project-list");
      await NavigateAsync("Create a new conversion");

      Document.QuerySelector<IHtmlElement>("h1")!.Text().Trim().Should()
         .Be("Create a new conversion");
   }

   [Fact]
   public async Task Should_show_error_when_no_school_provided()
   {
      await OpenAndConfirmPathAsync("/start-new-project/school-name");

      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]")!.Text().Trim().Should()
         .Be("Enter the school name or URN");
   }

   [Fact]
   public async Task Should_show_school_not_found_error()
   {
      await OpenAndConfirmPathAsync("/start-new-project/school-name");
      string schoolName = "fakeschoolname";

      _factory.AddGetWithJsonResponse("/establishments",
         new List<EstablishmentDto> { new() });

      Document.QuerySelector<IHtmlInputElement>("#SearchQuery")!.Value = schoolName;
      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]")!.Text().Trim().Should()
        .Be("We could not find any schools matching your search criteria");
   }

   [Fact]
   public async Task Should_school_not_found_error_when_ukprn_is_incorrect()
   {
      await OpenAndConfirmPathAsync("/start-new-project/school-name");
      string schoolName = "fakeschoolname(22222)";

      _factory.AddGetWithJsonResponse("/establishments",
         new List<EstablishmentDto> { new() });

      Document.QuerySelector<IHtmlInputElement>("#SearchQuery")!.Value = schoolName;
      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]")!.Text().Trim().Should()
        .Be("We could not find a school matching your search criteria");
   }
}
