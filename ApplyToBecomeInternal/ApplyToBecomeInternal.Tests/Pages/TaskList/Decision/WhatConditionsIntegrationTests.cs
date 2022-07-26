using AngleSharp.Dom;
using AngleSharp.Html.Dom;
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
		public async Task Should_persist_selected_decision()
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
	}
}
