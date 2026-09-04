using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ConsultationDuration;

public class ConsultationDurationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_consultation_duration_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 701);
      project.ConsultationDuration.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No;
      project.ConsultationDuration.ConsultationDurationNotMetReason = "Consultation ran for two weeks only";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/consultation-duration");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Consultation duration");
      Document.QuerySelector<IHtmlInputElement>("#consultation-duration-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='consultation-duration-not-met-reason']")!.Value
         .Should().Be("Consultation ran for two weeks only");
   }

   [Fact]
   public async Task Should_save_yes_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 702);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeConsultationDuration, project.Id),
         new SetSignificantChangeConsultationDurationCommand(ConsultationDurationAnswer.Yes, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/consultation-duration");

      Document.QuerySelector<IHtmlInputElement>("#consultation-duration-yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_satisfactory_consultation_answer_without_a_reason()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 703);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeConsultationDuration, project.Id),
         new SetSignificantChangeConsultationDurationCommand(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/consultation-duration");

      Document.QuerySelector<IHtmlInputElement>("#consultation-duration-no-satisfactory")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_with_reason_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 704);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string reason = "Consultation ran for two weeks only";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeConsultationDuration, project.Id),
         new SetSignificantChangeConsultationDurationCommand(ConsultationDurationAnswer.No, reason),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/consultation-duration");

      Document.QuerySelector<IHtmlInputElement>("#consultation-duration-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='consultation-duration-not-met-reason']")!.Value = reason;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_is_selected_without_reason()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 705);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/consultation-duration");

      Document.QuerySelector<IHtmlInputElement>("#consultation-duration-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='consultation-duration-not-met-reason']")!.Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/consultation-duration");
      Document.QuerySelector<IHtmlElement>("#ConsultationDurationNotMetReason-error")!
         .TextContent.Should().Contain("Add a reason");
   }

    [Fact]
   public async Task Should_redirect_to_task_list_when_stakeholders_were_not_consulted()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 706, trustConsultedStakeholders: false);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync(
         $"/significant-change/task-list/{project.Id}/consultation-duration",
         $"/significant-change/task-list/{project.Id}",
         "the task is not available until stakeholders have been consulted");

      VerifyNullElement("Consultation duration");
   }

   private static SignificantChangeProjectResponse BuildProject(int id, bool? trustConsultedStakeholders = true)
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
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse
         {
            TrustConsultedStakeholders = trustConsultedStakeholders
         },
         ConsultationDuration = new SignificantChangeConsultationDurationResponse()
      };
   }
}