using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.GeneralInformation;

public class DistanceFromTrustHeadquartersIntegrationTests : BaseIntegrationTests
{
   public DistanceFromTrustHeadquartersIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_distance_to_trust_headquarters()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentResponse(project.Urn.ToString());
      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.DistanceFromSchoolToTrustHeadquarters)
            .With(r => r.DistanceFromSchoolToTrustHeadquartersAdditionalInformation)
            .With(r => r.Urn, project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information");
      await NavigateAsync("Change", 4);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/how-far-converting-school-from-trust");
      Document.QuerySelector<IHtmlInputElement>("#distance-to-trust-headquarters")!.Value.Should().Be(project.DistanceFromSchoolToTrustHeadquarters.ToSafeString());
      Document.QuerySelector<IHtmlTextAreaElement>("#distance-to-trust-headquarters-additional-information")!.TextContent.Should()
         .Be(project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);

      Document.QuerySelector<IHtmlInputElement>("#distance-to-trust-headquarters")!.Value = request.DistanceFromSchoolToTrustHeadquarters?.ToString("F1")!;
      Document.QuerySelector<IHtmlTextAreaElement>("#distance-to-trust-headquarters-additional-information")!.Value =
         request.DistanceFromSchoolToTrustHeadquartersAdditionalInformation;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information/how-far-converting-school-from-trust");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_distance_to_trust_headquarters()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information/how-far-converting-school-from-trust");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
   }

   [Fact]
   public async Task Should_display_validation_error_when_non_numeric_values_entered()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information/how-far-converting-school-from-trust");
      string invalidEntry = "abc";
      Document.QuerySelector<IHtmlInputElement>("#distance-to-trust-headquarters")!.Value = invalidEntry;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/how-far-converting-school-from-trust");

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should()
         .Contain("'Distance from the converting school to the trust or other schools in the trust' must be a valid format");

      Document.QuerySelector(".govuk-error-message").Should().NotBeNull();
      Document.QuerySelector("#distance-to-trust-headquarters-error")!.TextContent.Should()
         .Contain("'Distance from the converting school to the trust or other schools in the trust' must be a valid format");
   }

   [Fact]
   public async Task Should_display_miles_as_unit_of_distance()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information/how-far-converting-school-from-trust");

      Document.QuerySelector<IHtmlDivElement>("#distance-to-trust-headquarters-suffix")!.InnerHtml.Should().Be("miles");
   }
}
