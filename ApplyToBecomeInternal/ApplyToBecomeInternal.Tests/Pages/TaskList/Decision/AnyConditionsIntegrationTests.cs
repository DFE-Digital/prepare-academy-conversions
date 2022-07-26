using AngleSharp.Dom;
using AngleSharp.Html.Dom;
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

			var declinedRadioBtn = Document.QuerySelector<IHtmlInputElement>("#no-radio");

			declinedRadioBtn.IsChecked.Should().BeTrue();
		}
	}
}
