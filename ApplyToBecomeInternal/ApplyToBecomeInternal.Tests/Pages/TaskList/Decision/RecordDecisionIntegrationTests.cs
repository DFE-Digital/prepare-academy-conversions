using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
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

			Document.QuerySelector<IHtmlInputElement>("#approved-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();			

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			var formElement = Document.QuerySelector<IHtmlInputElement>("#approved-radio");

			formElement.IsChecked.Should().BeTrue();
		}

		[Fact]
		public async Task Should_redirect_on_succesful_submission()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			Document.QuerySelector<IHtmlInputElement>("#approved-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();

			Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/who-decided");
		}

		[Fact]
		public async Task Should_go_back_to_tasklist()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await NavigateAsync("Back to task list");

			Document.QuerySelector<IHtmlElement>("h1").Text().Trim().Should().Be(project.SchoolName);
			Document.Url.Should().EndWith($"/task-list/{project.Id}");
		}
	}
}