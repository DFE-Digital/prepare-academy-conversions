using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.PlanningPermission;

public class PlanningPermissionIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_planning_permission_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 801);
      project.PlanningPermission.PlanningPermissionAnswer = PlanningPermissionAnswer.No;
      project.PlanningPermission.AdditionalInformation = "Planning permission decision is pending";
      project.PlanningPermission.SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/planning/reference/12345";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/planning-permission");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Planning Permission");
      Document.QuerySelector<IHtmlInputElement>("#planning-permission-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='planning-permission-additional-information']")!.Value
         .Should().Be("Planning permission decision is pending");
      Document.QuerySelector<IHtmlInputElement>("[data-test='planning-permission-supporting-evidence']")!.Value
         .Should().Be("https://educationgovuk.sharepoint.com/sites/planning/reference/12345");
   }

   [Fact]
   public async Task Should_save_no_answer_with_additional_information_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 802);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string additionalInformation = "Planning permission is awaiting local authority approval";
      const string supportingEvidence = "https://educationgovuk.sharepoint.com/sites/planning/application/67890";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangePlanningPermission, project.Id),
         new SetSignificantChangePlanningPermissionCommand(PlanningPermissionAnswer.No, additionalInformation, supportingEvidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/planning-permission");

      Document.QuerySelector<IHtmlInputElement>("#planning-permission-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='planning-permission-additional-information']")!.Value = additionalInformation;
      Document.QuerySelector<IHtmlInputElement>("[data-test='planning-permission-supporting-evidence']")!.Value = supportingEvidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_not_applicable_answer_and_clear_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 803);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangePlanningPermission, project.Id),
         new SetSignificantChangePlanningPermissionCommand(
            PlanningPermissionAnswer.NotApplicable,
            null,
            "https://educationgovuk.sharepoint.com/sites/planning/not-required"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/planning-permission");

      Document.QuerySelector<IHtmlInputElement>("#planning-permission-not-applicable")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='planning-permission-additional-information']")!.Value = "This should be cleared";
      Document.QuerySelector<IHtmlInputElement>("[data-test='planning-permission-supporting-evidence']")!.Value =
         "https://educationgovuk.sharepoint.com/sites/planning/not-required";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_inline_error_for_supporting_evidence_from_an_unapproved_domain()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 804);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/planning-permission");

      Document.QuerySelector<IHtmlInputElement>("#planning-permission-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("[data-test='planning-permission-supporting-evidence']")!.Value =
         "https://other.sharepoint.com/sites/team/evidence";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector("#PlanningPermissionSupportingEvidence-error")!
         .TextContent.Should().Contain("Entry must be a valid gov uk SharePoint link");
      Document.QuerySelector<IHtmlInputElement>("[data-test='planning-permission-supporting-evidence']")!.Value
         .Should().Be("https://other.sharepoint.com/sites/team/evidence");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         PlanningPermission = new SignificantChangePlanningPermissionResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}