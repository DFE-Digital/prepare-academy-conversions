using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision
{
	public class WhoDecidedIntegrationTests : BaseIntegrationTests
	{
		public WhoDecidedIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/who-decided");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}

		[Fact]
		public async Task Should_persist_who_decided()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/who-decided");

			Document.QuerySelector<IHtmlInputElement>("#directorgeneral-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			await OpenUrlAsync($"/task-list/{project.Id}/decision/who-decided");

			var formElement = Document.QuerySelector<IHtmlInputElement>("#directorgeneral-radio");

			formElement.IsChecked.Should().BeTrue();
		}

		[Fact]
		public async Task Should_go_back_to_recorddecision()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/who-decided");

			await NavigateAsync("Back");

			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be("Record the decision");			
		}

		[Fact]
		public async Task Should_display_error_when_nothing_selected()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/who-decided");

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("[href='#DecisionMadeBy']").Text().Should()
				.Be("Select who made the decision");
			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be("Who made this decision?");
		}

		[Fact]
		public async Task Should_redirect_to_decline_reasons_if_project_declined()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var wizard = new RecordDecisionWizard(Context);
			await wizard.StartFor(project.Id);
			await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
			await wizard.SetDecisionByAndContinue(DecisionMadeBy.RegionalDirectorForRegion);

			Document.Url.Should().EndWith("/decision/declined-reason", because: "reason should be the second question in the declined journey");
			Document.QuerySelector<IHtmlHeadingElement>(".govuk-fieldset__heading").TextContent.Trim()
				.Should().Be("Why was this project declined?", because: "the decline reason page is the expected next step");
		}
	}
}
