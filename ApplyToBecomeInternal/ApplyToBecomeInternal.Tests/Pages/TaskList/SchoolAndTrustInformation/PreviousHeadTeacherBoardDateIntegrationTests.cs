using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.SchoolAndTrustInformation
{
	public class PreviousHeadTeacherBoardDateIntegrationTests : BaseIntegrationTests
	{
		public PreviousHeadTeacherBoardDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_previous_head_teacher_board_date_question()
		{
			var (selected, toSelect) = RandomRadioButtons("previous-head-teacher-board-date-question", "Yes", "No");
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = selected.Value);
			var request = AddPatchProject(project, r => r.PreviousHeadTeacherBoardDateQuestion, toSelect.Value);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateAsync("Change", 5);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").Value.Should().Be(project.PreviousHeadTeacherBoardDateQuestion);

			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked = true;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date");
		}

		// need to handle no

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_confirm_school_and_trust_information_dates()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}
	}
}
