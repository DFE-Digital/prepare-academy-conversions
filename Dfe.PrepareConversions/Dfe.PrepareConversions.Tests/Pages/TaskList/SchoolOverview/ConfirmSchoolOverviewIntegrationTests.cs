using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolOverview;

public class ConfirmSchoolOverviewIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_be_in_progress_and_display_school_overview()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);
      EstablishmentDto establishment = AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-overview-status")!.TextContent.Trim().Should().Be("In Progress");
      Document.QuerySelector("#school-overview-status")!.ClassName.Should().Contain("blue");

      await NavigateAsync("School overview");

      Document.QuerySelector("#school-phase")!.TextContent.Should().Be(establishment.PhaseOfEducation.Name);
      Document.QuerySelector("#age-range")!.TextContent.Should().Be($"{establishment.StatutoryLowAge} to {establishment.StatutoryHighAge}");
      Document.QuerySelector("#school-type")!.TextContent.Should().Be(establishment.EstablishmentType.Name);
      Document.QuerySelector("#number-on-roll")!.TextContent.Should().Be(establishment.Census.NumberOfPupils);
      Document.QuerySelector("#percentage-school-full")!.TextContent.Should().Be(AsPercentageOf(establishment.Census.NumberOfPupils, establishment.SchoolCapacity));
      Document.QuerySelector("#capacity")!.TextContent.Should().Be(establishment.SchoolCapacity);
      Document.QuerySelector("#published-admission-number")!.TextContent.Should().Be(project.PublishedAdmissionNumber);
      Document.QuerySelector("#percentage-free-school-meals")!.TextContent.Should().Be($"{establishment.Census.PercentageFsm}%");
      Document.QuerySelector("#part-of-pfi")!.TextContent.Should().Be(project.PartOfPfiScheme);
      Document.QuerySelector("#viability-issues")!.TextContent.Should().Be(project.ViabilityIssues);
      Document.QuerySelector("#financial-deficit")!.TextContent.Should().Be(project.FinancialDeficit);
      Document.QuerySelector("#diocesan-multi-academy-trust")!.TextContent.Should().Be($"Yes, {establishment.Diocese.Name}");

      // Waiting for calculation to be done in TRAMS API so no data pulled through currently
      Document.QuerySelector("#distance-to-trust-headquarters")!.TextContent.Should().Be($"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles");
      Document.QuerySelector("#distance-to-trust-headquarters-additional-text")!.TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
      Document.QuerySelector("#parliamentary-constituency")!.TextContent.Should().Be(establishment.ParliamentaryConstituency.Name);
      Document.QuerySelector("#member-of-parliament-name-and-party")!.TextContent.Should().Be(project.MemberOfParliamentNameAndParty);
   }

   [Fact]
   public async Task Should_display_distance_additional_information_given_no_distance()
   {
      AcademyConversionProject project = AddGetProject(p => p.DistanceFromSchoolToTrustHeadquarters = null);

      project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation.Should().NotBeNullOrWhiteSpace();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("School overview");

      Document.QuerySelector("#distance-to-trust-headquarters-additional-text")!.TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
   }
   [Fact]
   public async Task Should_display_annex_b_help_when_sponsored()
   {
      AcademyConversionProject project = AddGetProject(p => p.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("School overview");
      var test = Document.QuerySelector("[data-test=annex-b-help]").TextContent;
      Document.QuerySelector("[data-test=annex-b-help]").TextContent.Should().Be("Some of the information has come from school's application form.");
   }
   [Fact]
   public async Task Should_display_change_on_pfi_scheme()
   {
      AcademyConversionProject project = AddGetProject(p => p.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("School overview");

      Document.QuerySelector("[data-test=change-part-of-pfi]")!.TextContent.Trim().Should().Be("Change");
   }

   [Fact]
   public async Task Should_be_completed_and_checked_when_school_overview_complete()
   {
      AcademyConversionProject project = AddGetProject(project => project.SchoolOverviewSectionComplete = true);
      AddGetEstablishmentDto(project.Urn.ToString());
      AddPatchConfiguredProject(project, x =>
      {
         x.SchoolOverviewSectionComplete = true;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-overview-status")!.TextContent.Trim().Should().Be("Completed");

      await NavigateAsync("School overview");

      Document.QuerySelector<IHtmlInputElement>("#school-overview-complete")!.IsChecked.Should().BeTrue();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_be_not_started_and_display_empty_when_school_overview_not_prepopulated()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.PublishedAdmissionNumber = null;
         project.PartOfPfiScheme = "No";
         project.PfiSchemeDetails = null;
         project.NumberOfPlacesFundedFor = null;
         project.NumberOfResidentialPlaces = null;
         project.NumberOfFundedResidentialPlaces = null;
         project.ViabilityIssues = null;
         project.FinancialDeficit = null;
         project.DistanceFromSchoolToTrustHeadquarters = null;
         project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation = null;
         project.SchoolOverviewSectionComplete = false;
         project.MemberOfParliamentNameAndParty = null;
      });
      AddGetEstablishmentDto(project.Urn.ToString(), true);
      AddPatchConfiguredProject(project, x =>
      {
         x.SchoolOverviewSectionComplete = false;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-overview-status")!.TextContent.Trim().Should().Be("Not Started");
      Document.QuerySelector("#school-overview-status")!.ClassName.Should().Contain("grey");

      await NavigateAsync("School overview");

      Document.QuerySelector("#school-phase")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#age-range")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#school-type")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#number-on-roll")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#percentage-school-full")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#capacity")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#published-admission-number")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#percentage-free-school-meals")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#part-of-pfi")!.TextContent.Should().Be("No");
      Document.QuerySelector("#viability-issues")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#financial-deficit")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#diocesan-multi-academy-trust")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#distance-to-trust-headquarters")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#parliamentary-constituency")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#member-of-parliament-name-and-party")!.TextContent.Should().Be("Empty");
      Document.QuerySelector<IHtmlInputElement>("#school-overview-complete")!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString());
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-overview");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Back_link_should_navigate_from_school_overview_to_task_list()
   {
      var project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("School overview");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Theory]
   [InlineData("change-published-admission-number",
      "change-viability-issues",
      "change-financial-deficit",
      "change-part-of-pfi",
      "change-distance-to-trust-headquarters",
      "change-member-of-parliament-name-and-party")]
   public async Task Should_not_have_change_link_if_project_read_only(params string[] elements)
   {
      var project = AddGetProject(isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("School overview");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
      foreach (var element in elements)
      {
         VerifyElementDoesNotExist(element);
      }
      Document.QuerySelector("#school-overview-complete").Should().BeNull();
      Document.QuerySelector("#confirm-and-continue-button").Should().BeNull();
   }

   private static string AsPercentageOf(string numberOfPupils, string schoolCapacity)
   {
      int? a = int.Parse(numberOfPupils);
      int? b = int.Parse(schoolCapacity);
      return a.AsPercentageOf(b);
   }
}
