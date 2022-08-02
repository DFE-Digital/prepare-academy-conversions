using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using FluentAssertions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;


namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class SummaryIntegrationTests : BaseIntegrationTests
	{
		public SummaryIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/summary");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}


		[Fact]
		public async Task Should_populate_summary_and_save()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			var request = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 01),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "bills need to be paid",
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
				ConversionProjectId = project.Id
			};

			_factory.AddPostWithJsonRequest("/conversion-project/advisory-board-decision", request, new AdvisoryBoardDecision());

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await SubmitThroughTheWizard(request);

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith("/project-list");
			Document.QuerySelector<IHtmlElement>("#notification-message").Text().Trim().Should().Be("Decision recorded");
			Document.QuerySelector<IHtmlElement>("#govuk-notification-banner-title").Text().Trim().Should().Be("Done");
		}

		[Fact]
		public async Task Should_display_selected_options()
		{
			var request = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 01),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "bills need to be paid",
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral
			};

			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await SubmitThroughTheWizard(request);

			Document.QuerySelector<IHtmlElement>("#decision").Text().Should()
				.Be("APPROVED WITH CONDITIONS");
			Document.QuerySelector<IHtmlElement>("#decision-made-by").Text().Should()
				.Be("Director General");
			Document.QuerySelector<IHtmlElement>("#condition-set").Text().Trim().Should()
				.Be("Yes");
			Document.QuerySelector<IHtmlElement>("#condition-details").Text().Trim().Should()
				.Be(request.ApprovedConditionsDetails);

			var dateWithoutWhitespace = Regex.Replace(Document.QuerySelector<IHtmlElement>("#decision-date").Text(), @"\s+", "");
			dateWithoutWhitespace.Should().Be("01012021");
		}

		[Theory]
		[InlineData(0, "Record the decision")]
		[InlineData(1, "Who made this decision?")]
		[InlineData(2, "Were any conditions set?")]
		[InlineData(3, "What conditions were set?")]
		[InlineData(4, "Date conversion was approved")]
		public async Task Should_go_back_to_choose_and_backlink_to_summary(int changeLinkIndex, string expectedTitle)
		{
			var request = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 01),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "bills need to be paid",
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral
			};

			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await SubmitThroughTheWizard(request);

			// Back to form
			await NavigateAsync("Change", changeLinkIndex);

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be(expectedTitle);

			// Back to summary
			await NavigateAsync("Back");

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be("Check your answers before recording this decision");
		}

		[Theory]
		[InlineData(0, "Record the decision")]
		[InlineData(1, "Who made this decision?")]
		[InlineData(2, "Were any conditions set?")]
		[InlineData(3, "What conditions were set?")]
		[InlineData(4, "Date conversion was approved")]
		public async Task Should_go_back_to_choose_and_submit_back_to_summary(int changeLinkIndex, string expectedTitle)
		{
			var request = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 01),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "bills need to be paid",
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral
			};

			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await SubmitThroughTheWizard(request);

			// Back to form
			await NavigateAsync("Change", changeLinkIndex);

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be(expectedTitle);

			// submit form
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be("Check your answers before recording this decision");
		}


		private async Task SubmitThroughTheWizard(AdvisoryBoardDecision request)
		{
			await SelectRadioAndSubmit(request.Decision.ToString().ToLower());
			await SelectRadioAndSubmit(request.DecisionMadeBy.ToString().ToLower());
			await SelectRadioAndSubmit("yes");
			await InputTextAreaAndSubmit(request.ApprovedConditionsDetails);
			await InputDateAndSubmit(request.AdvisoryBoardDecisionDate.Value);
		}

		private async Task SelectRadioAndSubmit(string enumAsString)
		{
			Document.QuerySelector<IHtmlInputElement>($"#{enumAsString}-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}

		private async Task InputTextAreaAndSubmit(string conditions)
		{
			Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea").Value = conditions;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}

		private async Task InputDateAndSubmit(DateTime date)
		{
			Document.QuerySelector<IHtmlInputElement>("#-day").Value = date.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#-month").Value = date.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#-year").Value = date.Year.ToString();
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}
	}
}
