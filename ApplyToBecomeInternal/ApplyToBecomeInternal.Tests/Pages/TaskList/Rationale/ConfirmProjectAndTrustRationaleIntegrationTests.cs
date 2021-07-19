using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.Rationale
{
	public class ConfirmProjectAndTrustRationaleIntegrationTests : BaseIntegrationTests
	{
		public ConfirmProjectAndTrustRationaleIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_in_progress_and_display_rationale_when_rationale_populated()
		{
			var project = AddGetProject(p => p.RationaleSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#rationale-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#rationale-status").ClassName.Should().Contain("blue");

			await NavigateAsync("Rationale");

			Document.QuerySelector("#rationale-for-project").TextContent.Should().Be(project.RationaleForProject);
			Document.QuerySelector("#rationale-for-trust").TextContent.Should().Be(project.RationaleForTrust);
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_rationale_mark_as_complete_true()
		{
			var project = AddGetProject(project =>
			{
				project.RationaleSectionComplete = true;
			});
			AddPatchProject(project, r => r.RationaleSectionComplete, true);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#rationale-status").TextContent.Trim().Should().Be("Completed");

			await NavigateAsync("Rationale");

			Document.QuerySelector<IHtmlInputElement>("#rationale-complete").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_be_not_started_and_display_empty_when_rationale_not_prepopulated()
		{
			var project = AddGetProject(project =>
			{
				project.RationaleForProject = null;
				project.RationaleForTrust = null;
				project.RationaleSectionComplete = false;
			});
			AddPatchProject(project, r => r.RationaleSectionComplete, false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#rationale-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#rationale-status").ClassName.Should().Contain("grey");

			await NavigateAsync("Rationale");

			Document.QuerySelector("#rationale-for-project").TextContent.Should().Be("Empty");
			Document.QuerySelector("#rationale-for-trust").TextContent.Should().Be("Empty"); 
			Document.QuerySelector<IHtmlInputElement>("#rationale-complete").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_rationale_summary()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Rationale");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
