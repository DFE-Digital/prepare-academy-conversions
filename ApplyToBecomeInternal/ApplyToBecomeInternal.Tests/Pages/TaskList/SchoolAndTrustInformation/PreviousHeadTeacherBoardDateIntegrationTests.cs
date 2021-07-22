using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Tests.Customisations;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.SchoolAndTrustInformation
{
	public class PreviousHeadTeacherBoardDateIntegrationTests : BaseIntegrationTests
	{
		public PreviousHeadTeacherBoardDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
			_fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddMonths(-2), DateTime.Now.AddDays(-1)));
		}

		[Fact]
		public async Task Should_navigate_to_and_update_previous_head_teacher_board_date_and_question_when_user_selects_yes()
		{
			var project = AddGetProject(p =>
			{
				p.PreviousHeadTeacherBoardDateQuestion = null;
				p.PreviousHeadTeacherBoardDate = null;
			});
			AddPatchProject(project, r => r.PreviousHeadTeacherBoardDateQuestion, "Yes");
			var secondPatchRequest = AddPatchProject(project, r => r.PreviousHeadTeacherBoardDate);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateAsync("Change", 5);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeFalse();

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked = true;
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-day").Value.Should().Be("");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-month").Value.Should().Be("");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-year").Value.Should().Be("");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-day").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-month").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-year").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Year.ToString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}

		[Fact]
		public async Task Should_navigate_to_and_update_previous_head_teacher_board_date_question_when_user_selects_no()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = null);
			AddPatchProjectMany(project, composer => composer
				.With(r => r.PreviousHeadTeacherBoardDateQuestion, "No")
				.With(r => r.PreviousHeadTeacherBoardDate, default(DateTime)));

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

		[Fact]
		public async Task Should_display_question_value_and_navigate_back_to_confirm_page_from_previous_head_teacher_board_date_question()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = "Yes");

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeFalse();

			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}

		[Fact]
		public async Task Should_display_date_and_navigate_back_to_previous_head_teacher_board_date_question_from_previous_head_teacher_board_date()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-day").Value.Should().Be(project.PreviousHeadTeacherBoardDate.Value.Day.ToString());
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-month").Value.Should().Be(project.PreviousHeadTeacherBoardDate.Value.Month.ToString());
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-year").Value.Should().Be(project.PreviousHeadTeacherBoardDate.Value.Year.ToString());

			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-head-teacher-board-date-question");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}
	}
}
