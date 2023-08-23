using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision;

public class RecordDecisionIntegrationTests : BaseIntegrationTests
{
   public RecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_display_selected_schoolname()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      string selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span")!.Text();

      selectedSchool.Should().Be(project.SchoolName);
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
}
