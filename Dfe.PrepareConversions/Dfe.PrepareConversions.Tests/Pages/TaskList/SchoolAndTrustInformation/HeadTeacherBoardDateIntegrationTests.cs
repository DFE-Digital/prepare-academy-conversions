using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Customisations;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class HeadTeacherBoardDateIntegrationTests : BaseIntegrationTests
{
   public HeadTeacherBoardDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
      _fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddDays(1), DateTime.Now.AddMonths(12)));
   }

   [Fact]
   public async Task Should_navigate_to_and_update_head_teacher_board_date()
   {
      AcademyConversionProject project = AddGetProject();
      UpdateAcademyConversionProject request = AddPatchConfiguredProject(project, x =>
      {
         x.HeadTeacherBoardDate = _fixture.Create<DateTime?>();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");
      await NavigateDataTestAsync("change-advisory-board-date");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

      Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-day")!.Value = request.HeadTeacherBoardDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-month")!.Value = request.HeadTeacherBoardDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-year")!.Value = request.HeadTeacherBoardDate?.Year.ToString()!;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_head_teacher_board_date()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }

   [Fact]
   public async Task Should_show_the_advisory_board_schedule_link()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

      Document.QuerySelector("#advisory-board-meeting-schedules").Should().NotBeNull();
   }
}
