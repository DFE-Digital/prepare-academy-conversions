using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class DeclineReasonIntegrationTests : BaseIntegrationTests, IAsyncLifetime
	{
		private AcademyConversionProject _project;
		private RecordDecisionWizard _wizard;

		public DeclineReasonIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		public async Task InitializeAsync()
		{
			_project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			_wizard = new RecordDecisionWizard(Context);

			await _wizard.StartFor(_project.Id);
			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.RegionalDirectorForRegion);

			Document.Url.Should().EndWith("/decision/declined-reason");
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		private string PageHeading => Document.QuerySelector("h1").TextContent.Trim();
		private IElement ErrorSummary => Document.QuerySelector(".govuk-error-summary");


		[Fact]
		public void Should_display_the_selected_school_name()
		{
			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(_project.SchoolName);
		}

		[Fact]
		public async Task Should_return_to_the_who_made_this_decision_page_when_back_link_is_clicked()
		{
			await NavigateAsync("Back");

			PageHeading.Should().Be("Who made this decision?");
		}

		[Theory]
		[InlineData(AdvisoryBoardDeclinedReasons.Finance)]
		[InlineData(AdvisoryBoardDeclinedReasons.Governance)]
		[InlineData(AdvisoryBoardDeclinedReasons.Performance)]
		[InlineData(AdvisoryBoardDeclinedReasons.ChoiceOfTrust)]
		[InlineData(AdvisoryBoardDeclinedReasons.Other)]
		public async Task Should_persist_the_selected_decline_reasons(AdvisoryBoardDeclinedReasons reason)
		{
			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(reason, $"{reason} explanation"));

			await NavigateAsync("Back");

			CheckBoxFor(reason).IsChecked.Should().BeTrue();
			ExplanationFor(reason).Should().Contain($"{reason} explanation");
		}

		[Fact]
		public async Task Should_continue_to_the_decline_date_page_on_submit()
		{
			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Other, "other reasons"));

			PageHeading.Should().Be("Date conversion was declined");
		}

		[Fact]
		public async Task Should_not_allow_progress_if_none_of_the_options_are_selected()
		{
			CheckBoxFor(AdvisoryBoardDeclinedReasons.Finance).IsChecked = false;
			CheckBoxFor(AdvisoryBoardDeclinedReasons.Performance).IsChecked = false;
			CheckBoxFor(AdvisoryBoardDeclinedReasons.Governance).IsChecked = false;
			CheckBoxFor(AdvisoryBoardDeclinedReasons.ChoiceOfTrust).IsChecked = false;
			CheckBoxFor(AdvisoryBoardDeclinedReasons.Other).IsChecked = false;

			await _wizard.ClickSubmitButton();

			PageHeading.Should().Be("Why was this project declined?");

			ErrorSummary.Should().NotBeNull();
			ErrorSummary.TextContent.Should().Contain("There is a problem");
		}

		[Theory]
		[InlineData(AdvisoryBoardDeclinedReasons.Finance)]
		[InlineData(AdvisoryBoardDeclinedReasons.Performance)]
		[InlineData(AdvisoryBoardDeclinedReasons.Governance)]
		[InlineData(AdvisoryBoardDeclinedReasons.ChoiceOfTrust)]
		[InlineData(AdvisoryBoardDeclinedReasons.Other)]
		public async Task Should_require_a_reason_for_the_selected_option(AdvisoryBoardDeclinedReasons reason)
		{
			CheckBoxFor(reason).IsChecked = true;
			ExplanationFor(reason).Should().BeNullOrWhiteSpace();

			await _wizard.ClickSubmitButton();

			PageHeading.Should().Be("Why was this project declined?");
			ErrorSummary.Should().NotBeNull();
		}

		[Theory]
		[InlineData(AdvisoryBoardDeclinedReasons.Finance)]
		[InlineData(AdvisoryBoardDeclinedReasons.Performance)]
		[InlineData(AdvisoryBoardDeclinedReasons.Governance)]
		[InlineData(AdvisoryBoardDeclinedReasons.ChoiceOfTrust)]
		[InlineData(AdvisoryBoardDeclinedReasons.Other)]
		public async Task Should_clear_the_reason_for_an_option_if_it_is_no_longer_selected(AdvisoryBoardDeclinedReasons reason)
		{
			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(reason, $"{reason} explanation"));
			await NavigateAsync("Back");

			CheckBoxFor(reason).IsChecked.Should().BeTrue();
			ExplanationFor(reason).Should().NotBeEmpty();

			CheckBoxFor(reason).IsChecked = false;

			await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(ReasonOtherThan(reason), "Something else"));
			await NavigateAsync("Back");

			PageHeading.Should().Be("Why was this project declined?");
			ExplanationFor(reason).Should().BeNullOrWhiteSpace();
		}

		private AdvisoryBoardDeclinedReasons ReasonOtherThan(AdvisoryBoardDeclinedReasons reason)
		{
			return Enum.GetValues(typeof(AdvisoryBoardDeclinedReasons))
				.Cast<AdvisoryBoardDeclinedReasons>()
				.Except(new[] { reason })
				.First();
		}

		private IHtmlInputElement CheckBoxFor(AdvisoryBoardDeclinedReasons reason)
		{
			return Document.QuerySelector<IHtmlInputElement>($"#declined-reasons-{reason.ToString().ToLowerInvariant()}");
		}

		private string ExplanationFor(AdvisoryBoardDeclinedReasons reason)
		{
			return Document.QuerySelector<IHtmlTextAreaElement>($"#reason-{reason.ToString().ToLowerInvariant()}")?.TextContent;
		}
	}
}