using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.AdmissionsVariationDetails;

public class AdmissionsVariationDetailsIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_the_first_page_with_existing_answers()
   {
      SignificantChangeProjectResponse project = BuildProject(701);
      project.AdmissionsVariation.AdmissionsVariationRequired = false;
      project.AdmissionsVariation.AdmissionsVariationSupportingEvidence = "Admissions policy";
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details");

      Document.QuerySelector("h1")!.TextContent.Trim().Should().Be("Admissions variation details");
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-required-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>("[data-test='admissions-variation-supporting-evidence']")!
         .Value.Should().Be("Admissions policy");
   }

   [Fact]
   public async Task Should_save_no_and_supporting_evidence_without_opening_the_second_page()
   {
      SignificantChangeProjectResponse project = BuildProject(702);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeAdmissionsVariation, project.Id),
         new SetSignificantChangeAdmissionsVariationCommand(false, "Admissions policy", null, null, null, null, null, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details");
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-required-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("[data-test='admissions-variation-supporting-evidence']")!.Value = "Admissions policy";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_not_save_yes_until_all_second_page_answers_are_completed()
   {
      SignificantChangeProjectResponse project = BuildProject(703);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details");
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-required-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("[data-test='admissions-variation-supporting-evidence']")!.Value = "Admissions policy";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"/significant-change/task-list/{project.Id}/admissions-variation-details/support");
      Document.QuerySelector("h1")!.TextContent.Trim().Should().Be("Admissions variation details");
      _factory.GetMockServerLogs(
         string.Format(PathFor.SetSignificantChangeAdmissionsVariation, project.Id),
         System.Net.Http.HttpMethod.Put).Should().BeEmpty();

      Document.QuerySelector<IHtmlInputElement>("#local-authority-supportive-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#pupil-place-planning-supportive-no")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"/significant-change/task-list/{project.Id}/admissions-variation-details/support");
      Document.QuerySelector("#AdmissionsTeamSupportive-error")!.TextContent.Should().Contain("Select an option");
      _factory.GetMockServerLogs(
         string.Format(PathFor.SetSignificantChangeAdmissionsVariation, project.Id),
         System.Net.Http.HttpMethod.Put).Should().BeEmpty();
   }

   [Fact]
   public async Task Should_save_all_answers_after_both_pages_are_completed()
   {
      SignificantChangeProjectResponse project = BuildProject(704);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeAdmissionsVariation, project.Id),
         new SetSignificantChangeAdmissionsVariationCommand(
            true,
            "Admissions policy",
            true,
            false,
            true,
            null,
            "Pupil place planning concerns",
            null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details");
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-required-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("[data-test='admissions-variation-supporting-evidence']")!.Value = "Admissions policy";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      Document.QuerySelector<IHtmlInputElement>("#local-authority-supportive-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#pupil-place-planning-supportive-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#admissions-team-supportive-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='pupil-place-planning-supportive-additional-information']")!
         .Value = "Pupil place planning concerns";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_render_a_conditional_textbox_for_each_no_option()
   {
      SignificantChangeProjectResponse project = BuildProject(707);
      project.AdmissionsVariation.AdmissionsVariationLocalAuthoritySupportive = false;
      project.AdmissionsVariation.AdmissionsVariationPupilPlacePlanningSupportive = false;
      project.AdmissionsVariation.AdmissionsVariationAdmissionsTeamSupportive = false;
      project.AdmissionsVariation.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation = "Local authority concern";
      project.AdmissionsVariation.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation = "Pupil place planning concern";
      project.AdmissionsVariation.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation = "Admissions team concern";
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details");
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-required-yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector("#local-authority-supportive-no")!.GetAttribute("aria-controls")
         .Should().Be("conditional-local-authority-supportive-no");
      Document.QuerySelector("#pupil-place-planning-supportive-no")!.GetAttribute("aria-controls")
         .Should().Be("conditional-pupil-place-planning-supportive-no");
      Document.QuerySelector("#admissions-team-supportive-no")!.GetAttribute("aria-controls")
         .Should().Be("conditional-admissions-team-supportive-no");
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='local-authority-supportive-additional-information']")
         .Should().NotBeNull();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='pupil-place-planning-supportive-additional-information']")
         .Should().NotBeNull();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='admissions-team-supportive-additional-information']")
         .Should().NotBeNull();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='local-authority-supportive-additional-information']")!
         .Value.Should().Be("Local authority concern");
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='pupil-place-planning-supportive-additional-information']")!
         .Value.Should().Be("Pupil place planning concern");
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='admissions-team-supportive-additional-information']")!
         .Value.Should().Be("Admissions team concern");
   }

   [Fact]
   public async Task Should_require_the_first_question()
   {
      SignificantChangeProjectResponse project = BuildProject(705);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details");
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"/significant-change/task-list/{project.Id}/admissions-variation-details");
      Document.QuerySelector("#AdmissionsVariationRequired-error")!.TextContent.Should().Contain("Select an option");
   }

   [Fact]
   public async Task Should_redirect_to_the_first_page_if_the_second_page_has_no_draft()
   {
      SignificantChangeProjectResponse project = BuildProject(706);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-details/support");

      Document.Url.Should().EndWith($"/significant-change/task-list/{project.Id}/admissions-variation-details");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority",
         AdmissionsVariation = new SignificantChangeAdmissionsVariationResponse()
      };
   }
}
