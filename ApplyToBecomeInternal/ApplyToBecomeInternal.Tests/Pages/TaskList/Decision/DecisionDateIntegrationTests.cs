using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class DecisionDateIntegrationTests : BaseIntegrationTests
	{
		public DecisionDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{

		}

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/decision-date");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}

		[Fact]
		public async Task Should_persist_approval_date()
		{
			var expectedDay = "1";
			var expectedMonth = "1";
			var expectedYear = "2022";
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/decision-date");

			Document.QuerySelector<IHtmlInputElement>("#-day").Value = expectedDay;
			Document.QuerySelector<IHtmlInputElement>("#-month").Value = expectedMonth;
			Document.QuerySelector<IHtmlInputElement>("#-year").Value = expectedYear;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			await OpenUrlAsync($"/task-list/{project.Id}/decision/decision-date");

			var day = Document.QuerySelector<IHtmlInputElement>("#-day");
			var month = Document.QuerySelector<IHtmlInputElement>("#-month");
			var year = Document.QuerySelector<IHtmlInputElement>("#-year");

			expectedDay.Should().Be(expectedDay);
			expectedMonth.Should().Be(expectedMonth);
			expectedYear.Should().Be(expectedYear);
		}

		[Fact]
		public async Task Should_not_redirect_if_future_date()
		{			
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			var tomorrow = DateTime.UtcNow.AddDays(1);
			await OpenUrlAsync($"/task-list/{project.Id}/decision/decision-date");

			Document.QuerySelector<IHtmlInputElement>("#-day").Value = $"{tomorrow.Day}";
			Document.QuerySelector<IHtmlInputElement>("#-month").Value = $"{tomorrow.Month}";
			Document.QuerySelector<IHtmlInputElement>("#-year").Value = $"{tomorrow.Year}";
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/decision-date");
		}

		[Fact]
		public async Task Should_not_redirect_if_no_date_set()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			var tomorrow = DateTime.UtcNow.AddDays(1);
			await OpenUrlAsync($"/task-list/{project.Id}/decision/decision-date");
			
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/decision-date");
		}

		[Fact]
		public async Task Should_redirect_onSubmit()
		{
			var expectedDay = "1";
			var expectedMonth = "1";
			var expectedYear = "2022";
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/decision-date");

			Document.QuerySelector<IHtmlInputElement>("#-day").Value = expectedDay;
			Document.QuerySelector<IHtmlInputElement>("#-month").Value = expectedMonth;
			Document.QuerySelector<IHtmlInputElement>("#-year").Value = expectedYear;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith($"/task-list/{project.Id}");
		}
	}
}
