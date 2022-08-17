using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Tests.PageObjects;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class AnyConditionsIntegrationTests : BaseIntegrationTests
	{
		public AnyConditionsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/any-conditions");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}

		[Fact]
		public async Task Should_persist_selected_decision()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/any-conditions");

			Document.QuerySelector<IHtmlInputElement>("#no-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			await OpenUrlAsync($"/task-list/{project.Id}/decision/any-conditions");

			var formElement = Document.QuerySelector<IHtmlInputElement>("#no-radio");

			formElement.IsChecked.Should().BeTrue();
		}

		[Fact]
		public async Task Submit_yes_should_redirect_to_what_conditions()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/any-conditions");

			Document.QuerySelector<IHtmlInputElement>("#yes-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith("/decision/what-conditions");
		}

		[Fact]
		public async Task Submit_no_should_redirect_to_decision_date()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			RecordDecisionWizard wizard = new RecordDecisionWizard(Context);

			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Approved);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
			await wizard.SetIsConditionalAndContinue(false);

			Document.Url.Should().EndWith("/decision/decision-date");
		}

		[Fact]
		public async Task Should_go_back_to_whodecided()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/any-conditions");

			await NavigateAsync("Back");

			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be("Who made this decision?");
		}

		[Fact]
		public async Task Should_display_error_when_nothing_selected()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/any-conditions");

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("[href='#ApprovedConditionsSet']").Text().Should()
				.Be("Please choose an option");
			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be("Were any conditions set?");
		}
	}
}
