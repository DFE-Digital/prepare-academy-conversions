using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Tests.Customisations;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GenerateHTBTemplate
{
	public class GenerateHTBIntegrationTests : BaseIntegrationTests
	{
		public GenerateHTBIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) 
		{
			_fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddDays(1), DateTime.Now.AddMonths(12)));
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_generate_htb_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/download-project-template");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_between_preview_htb_template_and_generate_htb_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Generate project template");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/download-project-template");

			await NavigateAsync("Back to preview");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_error_summary_on_task_list_when_generate_button_clicked_if_no_htb_date_set()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();

			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory board date");

			await NavigateAsync("Set an Advisory board date before you generate your project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date?return=%2FTaskList%2FIndex");
		}

		[Fact]
		public async Task Should_navigate_from_task_list_error_summary_to_headteacher_board_date_back_to_task_list()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory board date");

			await NavigateAsync("Set an Advisory board date before you generate your project template");

			await NavigateDataTestAsync("headteacher-board-date-back-link");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_from_task_list_error_summary_to_headteacher_board_date_and_confirm_back_to_task_list()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);
			var request = AddPatchProject(project, r => r.HeadTeacherBoardDate);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory board date");

			await NavigateAsync("Set an Advisory board date before you generate your project template");
			Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-day").Value = request.HeadTeacherBoardDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-month").Value = request.HeadTeacherBoardDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-year").Value = request.HeadTeacherBoardDate.Value.Year.ToString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
