using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.Establishment;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.InvoluntaryProject;

public class SearchSchoolIntegrationTests : BaseIntegrationTests
{
   public SearchSchoolIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact(Skip = "Disabled for now.")]
   public async Task Should_link_to_school_search()
   {
      await OpenAndConfirmPathAsync("/project-list");
      await NavigateAsync("Start a new involuntary conversion project");

      Document.QuerySelector<IHtmlElement>("h1")!.Text().Trim().Should()
         .Be("Which school is involved?");
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
   public async Task Should_show_no_error()
   {
      await OpenAndConfirmPathAsync("/start-new-project/school-name");
      string schoolName = "fakeschoolname";

      _factory.AddGetWithJsonResponse("/establishments",
         new List<EstablishmentResponse> { new() });

      Document.QuerySelector<IHtmlInputElement>("#SearchQuery")!.Value = schoolName;
      await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]").Should().BeNull();
   }
}
