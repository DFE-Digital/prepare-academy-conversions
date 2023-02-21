using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision
{
	public class AnyConditionsIntegrationTests : BaseIntegrationTests, IAsyncLifetime
	{
		private AcademyConversionProject _project;
		private RecordDecisionWizard _wizard;


		public AnyConditionsIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output)
		{
		}

		private string PageHeading => Document.QuerySelector<IHtmlElement>("h1")?.Text().Trim();

		public async Task InitializeAsync()
		{
			_project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			_wizard = new RecordDecisionWizard(Context);

			await _wizard.StartFor(_project.Id);
			await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Approved);
			await _wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);

			Document.Url.Should().EndWith("any-conditions");
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		[Fact]
		public void Should_display_selected_school_name()
		{
			string selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span")?.Text();

			selectedSchool.Should().Be(_project.SchoolName);
		}

		[Fact]
		public async Task Should_persist_selected_decision()
		{
			const string details = "Some things need attention";

			await _wizard.SetIsConditionalAndContinue(true, details);

			await OpenUrlAsync($"/task-list/{_project.Id}/decision/any-conditions");

			bool isConditional = Document.QuerySelector<IHtmlInputElement>("#yes-radio").IsChecked;
			string conditionDetails = Document.QuerySelector<IHtmlTextAreaElement>("#ApprovedConditionsDetails")?.Text().Trim();

			isConditional.Should().BeTrue();
			conditionDetails.Should().Be(details);
		}

		[Fact]
		public async Task Submit_yes_should_redirect_to_decision_date()
		{
			await _wizard.SetIsConditionalAndContinue(true, "Reasons");

			Document.Url.Should().EndWith("/decision/decision-date");
		}

		[Fact]
		public async Task Submit_no_should_redirect_to_decision_date()
		{
			await _wizard.SetIsConditionalAndContinue(false, default);

			Document.Url.Should().EndWith("/decision/decision-date");
		}

		[Fact]
		public async Task Should_go_back_to_who_decided()
		{
			await NavigateAsync("Back");

			PageHeading.Should().Be("Who made this decision?");
		}

		[Fact]
		public async Task Should_display_error_when_nothing_selected()
		{
			await _wizard.ClickSubmitButton();

			Document.QuerySelector<IHtmlElement>("[href='#ApprovedConditionsSet']")?.Text()
				.Should().Be("Select whether any conditions were set");

			PageHeading.Should().Be("Were any conditions set?");
		}

		[Fact]
		public async Task Should_display_error_when_no_details_provided_for_conditional_approvals()
		{
			await _wizard.SetIsConditionalAndContinue(true, string.Empty);

			Document.QuerySelector<IHtmlElement>("[href='#ApprovedConditionsSet']")?.Text()
				.Should().Be("Select whether any conditions were set");

			PageHeading.Should().Be("Were any conditions set?");
		}
	}
}