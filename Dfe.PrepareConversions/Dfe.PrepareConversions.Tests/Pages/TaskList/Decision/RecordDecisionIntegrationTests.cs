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
   public async Task Should_show_dao_not_issued_option_for_sponsored_projects()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.SchoolOverviewSectionComplete = false;
         p.AcademyTypeAndRoute = "Sponsored";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      Document.QuerySelector<IHtmlInputElement>("#daonotissued-radio").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_not_show_dao_not_issued_option_for_non_sponsored_projects()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.SchoolOverviewSectionComplete = false;
         p.AcademyTypeAndRoute = "Converter";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      Document.QuerySelector<IHtmlInputElement>("#daonotissued-radio").Should().BeNull();
   }
}
