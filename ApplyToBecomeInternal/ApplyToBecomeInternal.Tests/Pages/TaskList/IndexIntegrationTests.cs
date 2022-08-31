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

namespace ApplyToBecomeInternal.Tests.Pages.TaskList
{
	public class IndexIntegrationTests : BaseIntegrationTests, IAsyncLifetime
	{
		private AcademyConversionProject _project;
		private RecordDecisionWizard _wizard;

		public IndexIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		public IHtmlElement DecisionMadeByElement => Document.QuerySelector<IHtmlElement>("#decision-made-by");
		public IHtmlElement DecisionElement => Document.QuerySelector<IHtmlElement>("#decision");
		public IHtmlElement ConditionsSetElement => Document.QuerySelector<IHtmlElement>("#condition-set");
		public IHtmlElement DecisionDateElement => Document.QuerySelector<IHtmlElement>("#decision-date");

		[Fact]
		public async Task Should_redirect_to_record_decision()
		{
			await OpenUrlAsync($"/task-list/{_project.Id}?rd=x");

			await NavigateAsync("Record a decision");

			Document.Url.Should().Contain($"/task-list/{_project.Id}/decision/record-decision");
		}

		[Fact]
		public async Task Should_show_approved_choices_from_session()
		{
			AdvisoryBoardDecision request = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "bills need to be paid",
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
				ConversionProjectId = _project.Id
			};

			await OpenUrlAsync($"/task-list/{_project.Id}/decision/record-decision");

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			await OpenUrlAsync($"/task-list/{_project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("APPROVED WITH CONDITIONS");
			DecisionMadeByElement.Text().Should()
				.Be("Director General");
			ConditionsSetElement.Text().Trim()
				.Should().Contain("Yes");
			ConditionsSetElement.Text().Trim()
				.Should().Contain(request.ApprovedConditionsDetails);

			DecisionDateElement.Text().Trim().Should()
				.Be($"01 January {DateTime.Today.Year}");
			Document.QuerySelector<IHtmlAnchorElement>("#record-decision-link").Text().Trim().Should()
				.Be("Change your decision");
		}

		[Fact]
		public async Task Should_show_deferred_choices_from_session()
		{
			await _wizard.StartFor(_project.Id);
			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);
			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
			await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(AdvisoryBoardDeferredReason.Other, "other explanation"));
			await _wizard.SetDecisionDateAndContinue(new DateTime(DateTime.Today.Year, 1, 1));

			await OpenUrlAsync($"/task-list/{_project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("Deferred");
			DecisionMadeByElement.Text().Should()
				.Be("Director General");
			Regex.Replace(Document.QuerySelector<IHtmlElement>("#deferred-reasons").Text().Trim(), @"\s+", string.Empty).Should()
				.Be("Other:otherexplanation");
			DecisionDateElement.Text().Trim().Should()
				.Be($"01 January {DateTime.Today.Year}");
		}

		[Fact]
		public async Task Should_show_declined_choices_from_session()
		{
			await _wizard.StartFor(_project.Id);
			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Other, "other explanation"));
			await _wizard.SetDecisionDateAndContinue(new DateTime(DateTime.Today.Year, 1, 1));

			await OpenUrlAsync($"/task-list/{_project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("Declined");
			DecisionMadeByElement.Text().Should()
				.Be("Director General");
			Regex.Replace(Document.QuerySelector<IHtmlElement>("#decline-reasons").Text().Trim(), @"\s+", string.Empty).Should()
				.Be("Other:otherexplanation");
			DecisionDateElement.Text().Trim().Should()
				.Be($"01 January {DateTime.Today.Year}");
		}

		[Fact]
		public async Task Should_show_choices_from_api()
		{
			AdvisoryBoardDecision response = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 02),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "The conditions are ......",
				DecisionMadeBy = DecisionMadeBy.OtherRegionalDirector,
				ConversionProjectId = _project.Id
			};

			_factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{_project.Id}", response);

			await OpenUrlAsync($"/task-list/{_project.Id}?rd=x");

			DecisionElement.Text().Should()
				.Be("APPROVED WITH CONDITIONS");
			DecisionMadeByElement.Text().Should()
				.Be("A different Regional Director");
			ConditionsSetElement.Text().Trim()
				.Should().Contain("Yes");
			ConditionsSetElement.Text().Trim()
				.Should().Contain(response.ApprovedConditionsDetails);
			DecisionDateElement.Text().Trim().Should()
				.Be("02 January 2021");
		}

		#region IAsyncLifetime implementation

		public Task InitializeAsync()
		{
			_project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			_wizard = new RecordDecisionWizard(Context);
			return Task.CompletedTask;
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		#endregion
	}
}