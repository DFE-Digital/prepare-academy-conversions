using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.Rationale
{
	public class RationaleForProjectIntegrationTests : BaseIntegrationTests
	{
		public RationaleForProjectIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output) { }

		[Fact]
		public async Task Should_display_rationale_for_project()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");

			Document.QuerySelector("#project-rationale").TextContent.Should().Be(project.RationaleForProject);
		}

		[Fact]
		public async Task Should_navigate_to_rationale_for_project_from_rationale()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_rationale_from_rationale_for_project()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");
		}

		[Fact]
		public async Task Should_update_rationale_for_project()
		{
			var project = AddGetProject();
         var request = AddPatchConfiguredProject(project, x =>
         {
            x.RationaleForProject = _fixture.Create<string>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
			var textArea = Document.QuerySelector<IHtmlTextAreaElement>("#project-rationale");
			textArea.Value = request.RationaleForProject;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}
	}
}
