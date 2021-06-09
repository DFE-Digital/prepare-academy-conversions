using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using static ApplyToBecome.Data.Services.ProjectsService;

namespace ApplyToBecomeInternal.Tests.Pages.Rationale
{
	public class RationaleForProjectIntegrationTests : BaseIntegrationTests
	{
		public RationaleForProjectIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_display_rationale_for_project()
		{
			var (project, _) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");

			Document.QuerySelector("#project-rationale").TextContent.Should().Be(project.Rationale.RationaleForProject);
		}

		[Fact]
		public async Task Should_navigate_to_rationale_for_project_from_rationale()
		{
			var (project, _) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{project.Id}/rationale");

			var rationaleForProjectPage = await NavigateAsync("Change", 0);
			rationaleForProjectPage.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_rationale_from_rationale_for_project()
		{
			var (project, _) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");

			var rationalePage = await NavigateAsync("Back");
			rationalePage.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}

		[Fact]
		public async Task Should_update_rationale_for_project()
		{
			var (project, request) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/project-rationale");
			BrowsingContext.Active.QuerySelector("#project-rationale").TextContent.Insert(0, request.RationaleForProject);
			await BrowsingContext.Active.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}

		private (Project, UpdateAcademyConversionProjectRequest) SetupMockServer()
		{
			var project = Factory.AddGetProject();
			var request = Factory.AddPutProject(project.Id);
			return (project, request);
		}
	}
}
