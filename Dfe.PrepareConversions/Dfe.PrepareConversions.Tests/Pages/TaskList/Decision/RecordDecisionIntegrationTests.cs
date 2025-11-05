using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using FluentAssertions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision;

public class RecordDecisionIntegrationTests : BaseIntegrationTests
{
   public RecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_persist_selected_decision()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);
      _factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{project.Id}",
         new AdvisoryBoardDecision { Decision = AdvisoryBoardDecisions.Approved });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      Document.QuerySelector<IHtmlInputElement>("#approved-radio")!.IsChecked = true;
      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      IHtmlInputElement formElement = Document.QuerySelector<IHtmlInputElement>("#approved-radio");

      formElement!.IsChecked.Should().BeTrue();
   }

   [Fact]
   public async Task Should_redirect_on_successful_submission()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      Document.QuerySelector<IHtmlInputElement>("#approved-radio")!.IsChecked = true;
      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/who-decided");
   }

   [Fact]
   public async Task Should_display_error_when_nothing_selected()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("[href='#AdvisoryBoardDecision']")!.Text().Should()
         .Be("Select a decision");
      Document.QuerySelector<IHtmlElement>("h1")!.Text().Trim().Should().Be("Record the decision");
   }

   [Fact]
   public async Task Should_go_back_to_task_list()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      await NavigateAsync("Back");

      Document.QuerySelector<IHtmlElement>("h1")!.Text().Trim().Should().Be(project.SchoolName);
      Document.Url.Should().EndWith($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_display_advisory_board_date_required_error_when_no_advisory_board_date()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.HeadTeacherBoardDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      Document.QuerySelector<IHtmlInputElement>("#approved-radio")!.IsChecked = true;

      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      IHtmlAnchorElement advisoryBoardDateErrorLink = Document.QuerySelector<IHtmlAnchorElement>($"[id='/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date?return={WebUtility.UrlEncode("/TaskList/Decision/RecordDecision-error-link")}']");

      advisoryBoardDateErrorLink.Should().NotBeNull();
      advisoryBoardDateErrorLink.Text.Should().Be("You must enter an advisory board date before you can record a decision.");
   }

   [Fact]
   public async Task Should_display_advisory_board_date_in_futur_error_when_advisory_board_date_is_in_future()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.HeadTeacherBoardDate = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);
         x.AssignedUser = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      Document.QuerySelector<IHtmlInputElement>("#approved-radio")!.IsChecked = true;

      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      IHtmlAnchorElement advisoryBoardDateErrorLink = Document.QuerySelector<IHtmlAnchorElement>($"[id='/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date?return={WebUtility.UrlEncode("/TaskList/Decision/RecordDecision-error-link")}']");

      advisoryBoardDateErrorLink.Should().NotBeNull();
      advisoryBoardDateErrorLink.Text.Should().Be("The advisory board date must be today or in the past.");
   }
}
