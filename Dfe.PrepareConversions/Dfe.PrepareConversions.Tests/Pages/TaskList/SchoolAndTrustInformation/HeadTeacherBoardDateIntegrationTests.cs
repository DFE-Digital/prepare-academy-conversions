using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Tests.Customisations;
using AutoFixture;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SchoolAndTrustInformation
{
	public class HeadTeacherBoardDateIntegrationTests : BaseIntegrationTests
	{
		public HeadTeacherBoardDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) 
		{
			_fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddDays(1), DateTime.Now.AddMonths(12)));
		}

		[Fact]
		public async Task Should_navigate_to_and_update_head_teacher_board_date()
		{
			var project = AddGetProject();
         var request = AddPatchConfiguredProject(project, x =>
         {
            x.HeadTeacherBoardDate = _fixture.Create<DateTime?>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateDataTestAsync("change-advisory-board-date");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

			Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-day").Value = request.HeadTeacherBoardDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-month").Value = request.HeadTeacherBoardDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#head-teacher-board-date-year").Value = request.HeadTeacherBoardDate.Value.Year.ToString();
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_head_teacher_board_date()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}
		
		[Fact]
		public async Task Should_show_the_advisory_board_schedule_link()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

			Document.QuerySelector("#advisory-board-meeting-schedules").Should().NotBeNull();
		}
	}
}