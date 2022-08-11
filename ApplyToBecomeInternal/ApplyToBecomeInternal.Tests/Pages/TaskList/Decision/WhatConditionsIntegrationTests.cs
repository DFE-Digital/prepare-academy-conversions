using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Tests.PageObjects;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;


namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class WhatConditionsIntegrationTests : BaseIntegrationTests
	{
		public WhatConditionsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}

		[Fact]
		public async Task Should_persist_condition_details()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			var conditionDetails = "Your school has no outstanding debts.";

			Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea").Value = conditionDetails;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			var formElement = Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea");

			formElement.Value.Should().Be(conditionDetails);
		}

		[Fact]
		public async Task Should_redirect_on_successful_submission()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var conditionDetails = "Your school has no outstanding debts.";

			RecordDecisionWizard wizard = new RecordDecisionWizard(Context);

			await wizard.StartFor(project.Id);
			await wizard.SetDecisionTo(AdvisoryBoardDecisions.Approved);
			await wizard.SetDecisionBy(DecisionMadeBy.Minister);
			await wizard.SetIsConditional(true);
			await wizard.SpecifyConditions(conditionDetails);

			Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/decision-date");
		}

		[Fact]
		public async Task Should_reload_form_when_no_decision_conditions_are_given()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			var conditionDetails = "";

			Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea").Value = conditionDetails;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();			

			Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/what-conditions");
		}

		[Fact]
		public async Task Should_go_back_to_anydecisions()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			await NavigateAsync("Back");

			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be("Were any conditions set?");
		}

		[Fact]
		public async Task Should_display_error_when_nothing_selected()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("[href='#ApprovedConditionsDetails']").Text().Should()
				.Be("Please enter the conditions for approval");
			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be("What conditions were set?");
		}
	}
}
