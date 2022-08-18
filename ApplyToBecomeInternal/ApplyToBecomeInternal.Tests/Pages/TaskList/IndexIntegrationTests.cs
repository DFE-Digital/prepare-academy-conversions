using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList
{
	public class IndexIntegrationTests : BaseIntegrationTests
	{
		public IndexIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		public IHtmlElement DecisionMadeByElement => Document.QuerySelector<IHtmlElement>("#decision-made-by");
		public IHtmlElement DecisionElement => Document.QuerySelector<IHtmlElement>("#decision");
		public IHtmlElement ConditionsSetElement => Document.QuerySelector<IHtmlElement>("#condition-set");
		public IHtmlElement DecisionDateElement => Document.QuerySelector<IHtmlElement>("#decision-date");
		public IHtmlElement ConditionDetailsElement => Document.QuerySelector<IHtmlElement>("#condition-details");

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}

		[Fact]
		public async Task Should_redirect_to_record_decision()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			await NavigateAsync("Record a decision");

			Document.Url.Should().Contain($"/task-list/{project.Id}/decision/record-decision");
		}

		[Fact]
		public async Task Should_show_approved_choices_from_session()
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

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("APPROVED WITH CONDITIONS");
			DecisionMadeByElement.Text().Should()
				.Be("Director General");
			ConditionsSetElement.Text().Trim().Should()
				.Be("Yes");
			ConditionDetailsElement.Text().Trim().Should()
				.Be(request.ApprovedConditionsDetails);
			DecisionDateElement.Text().Trim().Should()
				.Be("01 January 2021");
			Document.QuerySelector<IHtmlAnchorElement>("#record-decision-link").Text().Trim().Should()
			   .Be("Change your decision");
		}

		[Fact]
		public async Task Should_show_deferred_choices_from_session()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);
			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
			await wizard.SetDeferredReasonsAndContinue(Tuple.Create(AdvisoryBoardDeferredReason.Other, "other explanation"));
			await wizard.SetDecisionDateAndContinue(new DateTime(2021, 1, 1));			

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("Deferred");
			DecisionMadeByElement.Text().Should()
				.Be("Director General");
			Regex.Replace(Document.QuerySelector<IHtmlElement>("#deferred-reasons").Text().Trim(), @"\s+", string.Empty).Should()
				.Be("Other:otherexplanation");
			DecisionDateElement.Text().Trim().Should()
				.Be("01 January 2021");			
		}

		[Fact]
		public async Task Should_show_declined_choices_from_session()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);
			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
			await wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Other, "other explanation"));
			await wizard.SetDecisionDateAndContinue(new DateTime(2021, 1, 1));

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("Declined");
			DecisionMadeByElement.Text().Should()
				.Be("Director General");
			Regex.Replace(Document.QuerySelector<IHtmlElement>("#decline-reasons").Text().Trim(), @"\s+", string.Empty).Should()
				.Be("Other:otherexplanation");
			DecisionDateElement.Text().Trim().Should()
				.Be("01 January 2021");
		}

		[Fact]
		public async Task Should_show_choices_from_api()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var response = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 02),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "The conditions are ......",
				DecisionMadeBy = DecisionMadeBy.OtherRegionalDirector,
				ConversionProjectId = project.Id
			};

			_factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{project.Id}", response);

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("APPROVED WITH CONDITIONS");
			DecisionMadeByElement.Text().Should()
				.Be("A different Regional Director");
			ConditionsSetElement.Text().Trim().Should()
				.Be("Yes");
			ConditionDetailsElement.Text().Trim().Should()
				.Be(response.ApprovedConditionsDetails);
			DecisionDateElement.Text().Trim().Should()
				.Be("02 January 2021");
		}
	}
}
