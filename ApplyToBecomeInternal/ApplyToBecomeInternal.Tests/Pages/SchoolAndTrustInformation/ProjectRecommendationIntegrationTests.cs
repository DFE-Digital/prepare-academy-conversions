using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolAndTrustInformation
{
	public class ProjectRecommendationIntegrationTests : BaseIntegrationTests
	{
		public ProjectRecommendationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_project_recommendation()
		{
			var (selected, toSelect) = RandomRadioButtons("project-recommendation", "Approve", "Defer", "Decline");
			
			var project = AddGetProject(p => p.RecommendationForProject = selected.Value);
			var request = AddPatchProject(project, r => r.RecommendationForProject, toSelect.Value);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeTrue();

			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked = true;

			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_project_recommendation()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}
	}
}
