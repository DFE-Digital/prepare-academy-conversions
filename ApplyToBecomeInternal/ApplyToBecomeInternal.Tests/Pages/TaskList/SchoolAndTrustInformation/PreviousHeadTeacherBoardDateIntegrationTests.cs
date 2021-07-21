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
		public async Task Should_navigate_to_and_update_previous_head_teacher_board_date_question_when_user_selects_yes()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = "");
			AddPatchProject(project, r => r.PreviousHeadTeacherBoardDateQuestion, "Yes");

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateAsync("Change", 5);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeFalse();

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked = true;
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date");
		}

		[Fact]
		public async Task Should_navigate_to_and_update_previous_head_teacher_board_date_question_when_user_selects_no()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = "");
			AddPatchProject(project, r => r.PreviousHeadTeacherBoardDateQuestion, "No");

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateAsync("Change", 5);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeFalse();

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked = true;
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}

		// check radio button value displays correctly when previously selected

		// test around clicking no

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
