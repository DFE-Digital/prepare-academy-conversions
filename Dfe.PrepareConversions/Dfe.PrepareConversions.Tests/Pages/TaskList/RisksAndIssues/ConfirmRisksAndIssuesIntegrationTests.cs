using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.RisksAndIssues;

public class ConfirmRisksAndIssuesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_be_in_progress_and_display_risks_and_issues()
   {
      var project = AddGetProject(p => p.RisksAndIssuesSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#risks-and-issues-status")!.TextContent.Trim().Should().Be("In Progress");
      Document.QuerySelector("#risks-and-issues-status")!.ClassName.Should().Contain("blue");

      await NavigateAsync("Risks and issues");

      Document.QuerySelector("#risks-and-issues")!.TextContent.Should().Be(project.RisksAndIssues);
   }

   [Fact]
   public async Task Should_be_completed_and_checked_when_risks_and_issues_mark_as_complete_true()
   {
      var project = AddGetProject(project =>
      {
         project.RisksAndIssuesSectionComplete = true;
      });
      AddPatchConfiguredProject(project, x =>
      {
         x.RisksAndIssuesSectionComplete = true;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#risks-and-issues-status")!.TextContent.Trim().Should().Be("Completed");

      await NavigateAsync("Risks and issues");

      Document.QuerySelector<IHtmlInputElement>("#risks-and-issues-complete")!.IsChecked.Should().BeTrue();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_be_not_started_and_display_empty_when_risks_and_issues_not_prepopulated()
   {
      var project = AddGetProject(project =>
      {
         project.RisksAndIssues = null;
         project.RisksAndIssuesSectionComplete = false;
      });
      AddPatchConfiguredProject(project, x =>
      {
         x.RisksAndIssuesSectionComplete = false;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#risks-and-issues-status")!.TextContent.Trim().Should().Be("Not Started");
      Document.QuerySelector("#risks-and-issues-status")!.ClassName.Should().Contain("grey");

      await NavigateAsync("Risks and issues");

      Document.QuerySelector("#risks-and-issues")!.TextContent.Should().Be("Empty");
      Document.QuerySelector<IHtmlInputElement>("#risks-and-issues-complete")!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      var project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-risks-issues");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_confirm_risks_and_issues()
   {
      var project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Risks and issues");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_not_have_change_link_if_project_read_only()
   {
      var project = AddGetProject(isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Risks and issues");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");

      VerifyElementDoesNotExist("change-risks-and-issues");

      Document.QuerySelector("#risks-and-issues-complete").Should().BeNull();
      Document.QuerySelector("#confirm-and-continue-button").Should().BeNull();
   }
}
