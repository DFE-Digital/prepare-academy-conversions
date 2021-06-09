using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Tests.Helpers;
using AutoFixture;
using FluentAssertions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.Rationale
{
	public class RationaleForProjectIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>
	{
		private readonly HttpClient _client;
		private readonly WireMockServer _server;
		private readonly IBrowsingContext _browsingContext;
		private readonly Fixture _fixture;

		public RationaleForProjectIntegrationTests(IntegrationTestingWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
			_server = factory.WMServer;
			_browsingContext = HtmlHelper.CreateBrowsingContext(_client);
			_fixture = new Fixture();
		}

		[Fact]
		public async Task Should_display_rationale_for_project()
		{
			var project = _fixture.Create<Project>();
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}/confirm-project-trust-rationale/project-rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			document.QuerySelector("#project-rationale").TextContent.Should().Be(project.Rationale.RationaleForProject);
		}

		[Fact]
		public async Task Should_navigate_to_rationale_for_project_from_rationale()
		{
			var project = _fixture.Create<Project>();
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}/rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			var rationaleForProjectPage = await document.NavigateAsync("Change", 0);
			rationaleForProjectPage.Url.Should().Be($"{document.Origin}/task-list/{id}/confirm-project-trust-rationale/project-rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_rationale_from_rationale_for_project()
		{
			var project = _fixture.Create<Project>();
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}/confirm-project-trust-rationale/project-rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			var rationalePage = await document.NavigateAsync("Back");
			rationalePage.Url.Should().Be($"{document.Origin}/task-list/{id}/rationale");
		}

		[Fact]
		public async Task Should_update_rationale_for_project()
		{
			var project = _fixture.Create<Project>();

			var updateAcademyConversionProjectRequest = _fixture
				.Build<ProjectsService.UpdateAcademyConversionProjectRequest>()
				.With(r => r.Id, project.Id)
				.Create();
			var id = SetupMockServer(project, updateAcademyConversionProjectRequest);

			var response = await _client.GetAsync($"/task-list/{id}/confirm-project-trust-rationale/project-rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			document.QuerySelector("#project-rationale").TextContent.Insert(0, updateAcademyConversionProjectRequest.RationaleForProject);
			((IHtmlElement)document.QuerySelector("button")).DoClick();

			// this isn't right - need to get the new document
			document.Url.Should().Be($"{document.Origin}/task-list/{id}/rationale");
		}

		private int SetupMockServer(Project project, ProjectsService.UpdateAcademyConversionProjectRequest request = null)
		{
			_server
				.Given(Request.Create()
					.WithPath($"/conversion-projects/{project.Id}")
					.UsingGet())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonSerializer.Serialize(project)));

			_server
				.Given(Request.Create()
					.WithPath($"/conversion-projects/{project.Id}")
					.WithBody(JsonSerializer.Serialize(request))
					.UsingPut())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json"));

			return project.Id;
		}
	}
}
