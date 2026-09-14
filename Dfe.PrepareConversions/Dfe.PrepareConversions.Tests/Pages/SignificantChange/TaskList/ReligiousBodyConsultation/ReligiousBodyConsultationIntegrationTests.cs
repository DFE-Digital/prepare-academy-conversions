using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;

public class ReligiousBodyConsultationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_religious_body_consultation_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 701);
      project.ReligiousBodyConsultation.TrustConsultedReligiousBody = false;
      project.ReligiousBodyConsultation.TrustConsultedReligiousBodyNotConsultedReason = "Consultation is planned after trustee meeting";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-body-consultation");

      Document.QuerySelector<IHtmlHeadingElement>("h1").TextContent.Trim().Should().Be("Religious body consultation");
      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-religious-body-no").IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-religious-body-not-consulted-reason']").Value
         .Should().Be("Consultation is planned after trustee meeting");
   }

   [Fact]
   public async Task Should_save_yes_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 702);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeReligiousBodyConsultation, project.Id),
         new SetSignificantChangeReligiousBodyConsultationCommand(true, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-body-consultation");

      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-religious-body-yes").IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_with_reason_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 703);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string reason = "The trust is scheduling religious body consultation in the next phase";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeReligiousBodyConsultation, project.Id),
         new SetSignificantChangeReligiousBodyConsultationCommand(false, reason),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-body-consultation");

      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-religious-body-no").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-religious-body-not-consulted-reason']").Value = reason;
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_is_selected_without_reason()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 704);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-body-consultation");

      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-religious-body-no").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-religious-body-not-consulted-reason']").Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/religious-body-consultation");
      Document.QuerySelector<IHtmlElement>("#TrustConsultedReligiousBodyNotConsultedReason-error")
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
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse(),
         ReligiousBodyConsultation = new SignificantChangeReligiousBodyConsultationResponse()
      };
   }
}
