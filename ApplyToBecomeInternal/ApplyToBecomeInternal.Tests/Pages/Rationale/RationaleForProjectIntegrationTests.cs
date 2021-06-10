using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.Rationale
{
	public class RationaleForProjectIntegrationTests : BaseIntegrationTests
	{
		public RationaleForProjectIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_display_rationale_for_project()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");

			Document.QuerySelector("#project-rationale").TextContent.Should().Be(project.Rationale.RationaleForProject);
		}

		[Fact]
		public async Task Should_navigate_to_rationale_for_project_from_rationale()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/rationale");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_rationale_from_rationale_for_project()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}

		[Fact]
		public async Task Should_update_rationale_for_project()
		{
			var (project, request) = AddGetAndPatchProject(r => r.RationaleForProject);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
			var textArea = Document.QuerySelector("#project-rationale") as IHtmlTextAreaElement;
			textArea.Value = request.RationaleForProject;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}
	}
}
