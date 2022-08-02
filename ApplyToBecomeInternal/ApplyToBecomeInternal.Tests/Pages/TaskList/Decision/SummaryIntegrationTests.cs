using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Tests.PageObjects;
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
		public async Task Should_redirect_to_recorddecision()
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

			_factory.AddPostWithJsonRequest("/conversion-project/advisory-board-decision", request, "");

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			await OpenUrlAsync($"/task-list/{project.Id}/decision/summary");

			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should()
				.Be("Record the decision");
		}

		[Fact]
		public async Task Should_display_selected_schoolname()
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

			_factory.AddPostWithJsonRequest("/conversion-project/advisory-board-decision", request, "");

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			Document.QuerySelector<IHtmlElement>("#selection-span").Text().Should()
				.Be(project.SchoolName);
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

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith($"/task-list/{project.Id}");
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

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

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

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

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

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			// Back to form
			await NavigateAsync("Change", changeLinkIndex);

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be(expectedTitle);

			// submit form
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be("Check your answers before recording this decision");
		}		
	}
}
