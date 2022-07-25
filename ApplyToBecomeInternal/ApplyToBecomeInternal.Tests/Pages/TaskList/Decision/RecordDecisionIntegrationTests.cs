using static ApplyToBecomeInternal.Tests.Helpers.WaitHelper;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class RecordDecisionIntegrationTests : BaseIntegrationTests
	{
		public RecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}
		
		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}

		[Fact]
		public async Task Should_persist_selected_decision()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");						

			Document.QuerySelector<IHtmlInputElement>("#deferred-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();			

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			var declinedRadioBtn = Document.QuerySelector<IHtmlInputElement>("#deferred-radio");

			declinedRadioBtn.IsChecked.Should().BeTrue();
		}
	}
}