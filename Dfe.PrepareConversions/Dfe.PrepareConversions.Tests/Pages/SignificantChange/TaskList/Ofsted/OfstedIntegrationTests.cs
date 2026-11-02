using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted;

public class OfstedIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_existing_inspection_date()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 921);
      project.OfstedInspection.InspectionDate = new DateTime(2025, 9, 15);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/ofsted");

      Document.QuerySelector<IHtmlInputElement>("#inspection-date-day")!.Value.Should().Be("15");
      Document.QuerySelector<IHtmlInputElement>("#inspection-date-month")!.Value.Should().Be("9");
      Document.QuerySelector<IHtmlInputElement>("#inspection-date-year")!.Value.Should().Be("2025");
   }

   [Fact]
   public async Task Should_save_inspection_date_and_yes_answer_then_return_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 922);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);
      DateTime inspectionDate = new(2025, 9, 15);
      const string supportingEvidence = "School improvement plan";

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeOfstedInspection, project.Id),
         new SetSignificantChangeOfstedInspectionCommand(inspectionDate, true, null, supportingEvidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/ofsted");

      InputWithId("inspection-date-day").Value = "15";
      InputWithId("inspection-date-month").Value = "9";
      InputWithId("inspection-date-year").Value = "2025";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().Contain($"/significant-change/task-list/{project.Id}/ofsted-inspection-question");
      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim()
         .Should().Be("Is the academy rated good or outstanding in leadership and management and quality of education?");
      Document.QuerySelector(".govuk-hint")!.TextContent
         .Should().Contain("The most recent Ofsted inspection was 15 September 2025");

      Document.QuerySelector<IHtmlInputElement>("#ofsted-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#AdditionalInformation")!.Value = "This is cleared for Yes";
      Document.QuerySelector<IHtmlInputElement>("[data-test='ofsted-supporting-evidence']")!.Value = supportingEvidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_with_additional_information_and_supporting_evidence()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 926);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);
      DateTime inspectionDate = new(2025, 9, 15);
      const string additionalInformation = "Improvement actions are underway";
      const string supportingEvidence = "School improvement plan";

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeOfstedInspection, project.Id),
         new SetSignificantChangeOfstedInspectionCommand(
            inspectionDate,
            false,
            additionalInformation,
            supportingEvidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/ofsted");

      InputWithId("inspection-date-day").Value = "15";
      InputWithId("inspection-date-month").Value = "9";
      InputWithId("inspection-date-year").Value = "2025";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector<IHtmlInputElement>("#ofsted-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#AdditionalInformation")!.Value = additionalInformation;
      Document.QuerySelector<IHtmlInputElement>("[data-test='ofsted-supporting-evidence']")!.Value = supportingEvidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_require_a_standards_answer_before_saving()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 923);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/ofsted");

      InputWithId("inspection-date-day").Value = "15";
      InputWithId("inspection-date-month").Value = "9";
      InputWithId("inspection-date-year").Value = "2025";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().Contain($"/significant-change/task-list/{project.Id}/ofsted-inspection-question");
      Document.QuerySelector<IHtmlElement>("#MeetsOfstedStandards-error")!
         .TextContent.Should().Contain("Select an option");
   }

   [Fact]
   public async Task Should_redirect_to_inspection_date_when_question_is_opened_without_a_date()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 924);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync(
         $"/significant-change/task-list/{project.Id}/ofsted-inspection-question",
         $"/significant-change/task-list/{project.Id}/ofsted");
   }

   [Fact]
   public async Task Should_show_validation_error_for_a_partial_inspection_date()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 925);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/ofsted");

      InputWithId("inspection-date-day").Value = "15";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"/significant-change/task-list/{project.Id}/ofsted");
      Document.QuerySelector("[data-cy='error-summary']")!.TextContent.Should().Contain("Date must include a");
   }

   [Theory]
   [InlineData(2024, 9, 18, "Is the academy rated good or outstanding?")]
   [InlineData(2024, 9, 19, "Is the academy rated good or outstanding in leadership and management and quality of education?")]
   [InlineData(2025, 11, 9, "Is the academy rated good or outstanding in leadership and management and quality of education?")]
   [InlineData(2025, 11, 10, "Is the academy meeting expected standard, strong standard or exceptional in all evaluation areas?")]
   public async Task Should_show_question_appropriate_to_inspection_date(int year, int month, int day, string expectedTitle)
   {
      SignificantChangeProjectResponse project = BuildProject(id: 927);
      project.OfstedInspection.InspectionDate = new DateTime(year, month, day);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/ofsted");
      await Context.OpenAsync(BuildRequestAddress(
         $"/significant-change/task-list/{project.Id}/ofsted-inspection-question"));

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be(expectedTitle);
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
         LocalAuthorityName = "Test local authority",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         OfstedInspection = new SignificantChangeOfstedInspectionResponse()
      };
   }
}
