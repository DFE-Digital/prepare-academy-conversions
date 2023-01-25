using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision
{
	public class ChangeDecisionOptionsIntegrationTests : BaseIntegrationTests, IAsyncLifetime
	{
		private AcademyConversionProject _project;
		private RecordDecisionWizard _wizard;

		public ChangeDecisionOptionsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		private string PageHeading => Document.QuerySelector<IHtmlElement>("h1")?.Text().Trim();

		public Task InitializeAsync()
		{
			_project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			_wizard = new RecordDecisionWizard(Context);
			return Task.CompletedTask;
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		[Fact]
		public async Task Should_not_allow_the_user_to_return_directly_to_the_summary_after_changing_the_decision()
		{
			AdvisoryBoardDecision decision = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = DateTime.Today,
				ApprovedConditionsSet = false,
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral
			};

			await _wizard.StartFor(_project.Id);
			await _wizard.SubmitThroughTheWizard(decision);

			await NavigateAsync("Change", 0);

			PageHeading.Should().Be("Record the decision");
			Document.Url.Should().Contain("obl=");

			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);

			Document.Url.Should().NotContain("obl=");
		}

		[Fact]
		public async Task Should_pass_the_obl_parameter_when_changing_whom_made_the_decision()
		{
			AdvisoryBoardDecision decision = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
				ApprovedConditionsSet = false,
				AdvisoryBoardDecisionDate = DateTime.Today
			};

			await _wizard.StartFor(_project.Id);
			await _wizard.SubmitThroughTheWizard(decision);
			await NavigateAsync("Change", 1);

			PageHeading.Should().Be("Who made this decision?");
			Document.Url.Should().Contain("obl=");

			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);

			Document.Url.Should().Contain("obl=");
		}

		[Fact]
		public async Task Should_pass_on_the_obl_parameter_when_changing_whether_the_accepted_decision_has_conditions()
		{
			AdvisoryBoardDecision decision = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
				ApprovedConditionsSet = false,
				AdvisoryBoardDecisionDate = DateTime.Today
			};

			await _wizard.StartFor(_project.Id);
			await _wizard.SubmitThroughTheWizard(decision);
			await NavigateAsync("Change", 2);

			PageHeading.Should().Be("Were any conditions set?");
			Document.Url.Should().Contain("obl=");

			await _wizard.SetIsConditionalAndContinue(true, "something");

			Document.Url.Should().Contain("obl=");
		}

		[Fact]
		public async Task Should_pass_on_the_obl_parameter_when_changing_declined_reasons()
		{
			await _wizard.StartFor(_project.Id);
			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Other, "other"));
			await _wizard.SetDecisionDateAndContinue(DateTime.Today);
			await NavigateAsync("Change", 2);

			PageHeading.Should().Be("Why was this project declined?");
			Document.Url.Should().Contain("obl=");

			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.ChoiceOfTrust, "trust"));

			Document.Url.Should().Contain("obl=");
		}

		[Fact]
		public async Task Should_pass_on_the_obl_parameter_when_changing_deferred_reasons()
		{
			await _wizard.StartFor(_project.Id);
			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);
			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
			await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(AdvisoryBoardDeferredReason.Other, "other"));
			await _wizard.SetDecisionDateAndContinue(DateTime.Today);
			await NavigateAsync("Change", 2);

			PageHeading.Should().Be("Why was this project deferred?");
			Document.Url.Should().Contain("obl=");

			await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, "Ofsted"));

			Document.Url.Should().Contain("obl=");
		}
	}
}
