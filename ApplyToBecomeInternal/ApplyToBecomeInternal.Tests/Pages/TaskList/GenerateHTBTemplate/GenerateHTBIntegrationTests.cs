using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GenerateHTBTemplate
{
	public class GenerateHTBIntegrationTests : BaseIntegrationTests
	{
		public GenerateHTBIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_between_task_list_and_generate_htb_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/generate-headteacher-board-template");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_between_preview_htb_template_and_generate_htb_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Generate project template");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/generate-headteacher-board-template");

			await NavigateAsync("Back to preview");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_display_error_summary_on_task_list_when_generate_button_clicked_if_no_htb_date_set()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory Board date");

			await NavigateAsync("Set an Advisory Board date before you generate your project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/head-teacher-board-date?return=%2FTaskList%2FPreviewHTBTemplate&fragment=head-teacher-board-date");
		}

		[Fact]
		public async Task Should_navigate_from_task_list_error_summary_to_headteacher_board_date_back_to_task_list()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory Board date");

			await NavigateAsync("Set an Advisory Board date before you generate your project template");

			await NavigateDataTestAsync("headteacher-board-date-back-link");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_from_task_list_error_summary_to_headteacher_board_date_and_confirm_back_to_task_list()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory Board date");

			await NavigateAsync("Set an Advisory Board date before you generate your project template");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
