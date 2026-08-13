using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.StakeholderConsultation;

public class StakeholderConsultationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_consult_stakeholders_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 501);
      project.StakeholderConsultation.TrustConsultedStakeholders = false;
      project.StakeholderConsultation.TrustConsultedStakeholdersNotConsultedReason = "Consultation is planned after trustee meeting";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-consultation");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Stakeholder consultation");
      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-stakeholders-not-consulted-reason']")!.Value
         .Should().Be("Consultation is planned after trustee meeting");
   }

   [Fact]
   public async Task Should_save_yes_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 502);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeStakeholderConsultation, project.Id),
         new SetSignificantChangeStakeholderConsultationCommand(true, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-consultation");

      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_with_reason_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 503);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string reason = "The trust is scheduling stakeholder consultation in the next phase";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeStakeholderConsultation, project.Id),
         new SetSignificantChangeStakeholderConsultationCommand(false, reason),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-consultation");

      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-stakeholders-not-consulted-reason']")!.Value = reason;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_is_selected_without_reason()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 504);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-consultation");

      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-stakeholders-not-consulted-reason']")!.Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/stakeholder-consultation");
      Document.QuerySelector<IHtmlElement>("#TrustConsultedStakeholdersNotConsultedReason-error")!
         .TextContent.Should().Contain("Add a reason");
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
         Status = "pre decision",
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse()
      };
   }
}
