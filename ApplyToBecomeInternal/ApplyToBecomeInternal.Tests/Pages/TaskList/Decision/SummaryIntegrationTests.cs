using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
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
		public async Task Should_redirect_to_tasklist()
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

			Document.QuerySelector<IHtmlElement>("h2").Text().Trim().Should()
				.Be("Task list");
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
		public async Task Should_populate_summary_and_create_new_decision()
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

			Document.Url.Should().EndWith($"/task-list/{project.Id}?rd=true");
			Document.QuerySelector<IHtmlElement>("#notification-message").Text().Trim().Should().Be("Decision recorded");
			Document.QuerySelector<IHtmlElement>("#govuk-notification-banner-title").Text().Trim().Should().Be("Done");
		}

		[Fact]
		public async Task Should_populate_summary_and_save_existing_decision()
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

			_factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{project.Id}", request);
			_factory.AddPutWithJsonRequest("/conversion-project/advisory-board-decision", request, new AdvisoryBoardDecision());

			await OpenUrlAsync($"/task-list/{project.Id}");
			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith($"/task-list/{project.Id}?rd=true");
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

			Document.QuerySelector<IHtmlElement>("#decision-date").Text().Trim().Should()
			.Be("01 January 2021");
		}

		[Fact]
		public async Task Should_display_declined_for_declined_projects()
		{
			AcademyConversionProject project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);

			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
			await wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "Finance reason"));
			await wizard.SetDecisionDateAndContinue(DateTime.Today);

			Document.QuerySelector("#decision").TextContent.Trim().Should().Be("Declined");
		}

		[Fact]
		public async Task Should_show_the_selected_decline_reasons_and_details()
		{
			AcademyConversionProject project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);

			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
			await wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "Finance detail"),
				Tuple.Create(AdvisoryBoardDeclinedReasons.ChoiceOfTrust, "Choice of trust detail"));
			await wizard.SetDecisionDateAndContinue(DateTime.Today);

			string declineReasonSummary = Document.QuerySelector("#decline-reasons").TextContent;

			declineReasonSummary.Should().Contain("Finance:", because: "finance reason was selected");
			declineReasonSummary.Should().Contain("Finance detail", because: "Finance reason detail was provided");
			declineReasonSummary.Should().Contain("Choice of trust:", because: "Choice of trust reason was selected");
			declineReasonSummary.Should().Contain("Choice of trust detail", because: "Choice of trust reason detail was provided");

			declineReasonSummary.Should().NotContain("Performance", because: "Performance was not selected");
			declineReasonSummary.Should().NotContain("Governance", because: "Governance was not selected");
			declineReasonSummary.Should().NotContain("Other", because: "Other was not selected");
		}

		[Fact]
		public async Task Should_show_the_selected_deferred_reasons_and_details()
		{
			AcademyConversionProject project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);

			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
			await wizard.SetDeferredReasonsAndContinue(
				Tuple.Create(AdvisoryBoardDeferredReason.PerformanceConcerns, "Performance detail"),
				Tuple.Create(AdvisoryBoardDeferredReason.Other, "Other"),
				Tuple.Create(AdvisoryBoardDeferredReason.AdditionalInformationNeeded, "additional info"),
				Tuple.Create(AdvisoryBoardDeferredReason.AwaitingNextOftedReport, "Ofsted"));
			await wizard.SetDecisionDateAndContinue(DateTime.Today);

			string declineReasonSummary = Document.QuerySelector("#deferred-reasons").TextContent;

			declineReasonSummary.Should().Contain("Additional information needed:");
			declineReasonSummary.Should().Contain("Awaiting next ofsted report:");
			declineReasonSummary.Should().Contain("Performance concerns:");
			declineReasonSummary.Should().Contain("Other:");
		}

		[Fact]
		public async Task Should_not_display_conditions_details_for_declined_projects()
		{
			AcademyConversionProject project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);

			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.OtherRegionalDirector);
			await wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "finance reasons"));
			await wizard.SetDecisionDateAndContinue(DateTime.Today);

			string summaryContent = Document.QuerySelector(".govuk-summary-list").TextContent;

			summaryContent.Should().NotContain("Where any conditions set", because: "declined projects are not conditional");
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
		[InlineData(0, "Record the decision", "Who made this decision?")]
		[InlineData(1, "Who made this decision?", "Were any conditions set?")]
		[InlineData(2, "Were any conditions set?", "What conditions were set?")]
		[InlineData(3, "What conditions were set?", "Date conversion was approved")]
		[InlineData(4, "Date conversion was approved", "Check your answers before recording this decision")]
		public async Task Should_go_back_to_choose_and_submit_back_to_summary(int changeLinkIndex, string changePageTitle, string nextPageTitle)
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

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be(changePageTitle);

			// submit form
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be(nextPageTitle);
		}
	}
}
