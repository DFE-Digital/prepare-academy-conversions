using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.RisksAndIssues
{
	public class ConfirmRisksAndIssuesIntegrationTests : BaseIntegrationTests
	{
		public ConfirmRisksAndIssuesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_in_progress_and_display_risks_and_issues()
		{
			var project = AddGetProject(p => p.RisksAndIssuesSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#risks-and-issues-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#risks-and-issues-status").ClassName.Should().Contain("blue");

			await NavigateAsync("Risks and issues");

			Document.QuerySelector("#risks-and-issues").TextContent.Should().Be(project.RisksAndIssues);
			Document.QuerySelector("#equalities-impact-assessment-considered").TextContent.Should().Be(project.EqualitiesImpactAssessmentConsidered);
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_risks_and_issues_mark_as_complete_true()
		{
			var project = AddGetProject(project =>
			{
				project.RisksAndIssuesSectionComplete = true;
			});
			AddPatchProject(project, r => r.RisksAndIssuesSectionComplete, true);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#risks-and-issues-status").TextContent.Trim().Should().Be("Completed");

			await NavigateAsync("Risks and issues");

			Document.QuerySelector<IHtmlInputElement>("#risks-and-issues-complete").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

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
			AddPatchProject(project, r => r.RisksAndIssuesSectionComplete, false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#risks-and-issues-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#risks-and-issues-status").ClassName.Should().Contain("grey");

			await NavigateAsync("Risks and issues");

			Document.QuerySelector("#risks-and-issues").TextContent.Should().Be("Empty");
			Document.QuerySelector<IHtmlInputElement>("#risks-and-issues-complete").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-risks-issues");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_confirm_risks_and_issues()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Risks and issues");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
