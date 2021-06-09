using AngleSharp;
using AngleSharp.Dom;
using ApplyToBecome.Data.Models;
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
	public class RationaleIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>
	{
		private readonly HttpClient _client;
		private readonly WireMockServer _server;
		private readonly IBrowsingContext _browsingContext;
		private readonly Fixture _fixture;

		public RationaleIntegrationTests(IntegrationTestingWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
			_server = factory.WMServer;
			_browsingContext = HtmlHelper.CreateBrowsingContext(_client);
			_fixture = new Fixture();
		}

		[Fact]
		public async Task Should_navigate_to_rationale_from_task_list()
		{
			var project = _fixture.Create<Project>();
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}");
			var document = await _browsingContext.GetDocumentAsync(response);

			var schoolPerformancePage = await document.NavigateAsync("Rationale");
			schoolPerformancePage.Url.Should().Be($"{document.Origin}/task-list/{id}/rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_task_list_from_rationale()
		{
			var project = _fixture.Create<Project>();
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}/rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			var taskList = await document.NavigateAsync("Back to task list");
			taskList.Url.Should().Be($"{document.Origin}/task-list/{id}");
		}

		[Fact]
		public async Task Should_display_rationale()
		{
			var project = _fixture.Create<Project>();
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}/rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			document.QuerySelector("#rationale-for-project").TextContent.Should().Be(project.Rationale.RationaleForProject);
			document.QuerySelector("#rationale-for-trust").TextContent.Should().Be(project.Rationale.RationaleForTrust);
		}

		[Fact]
		public async Task Should_display_empty_when_rationale_not_prepopulated()
		{
			var project = _fixture.Create<Project>();
			project.Rationale.RationaleForProject = null;
			project.Rationale.RationaleForTrust = null;
			var id = SetupMockServer(project);

			var response = await _client.GetAsync($"/task-list/{id}/rationale");
			var document = await _browsingContext.GetDocumentAsync(response);

			document.QuerySelector("#rationale-for-project-empty").TextContent.Should().Be("Empty");
			document.QuerySelector("#rationale-for-trust-empty").TextContent.Should().Be("Empty");
		}

		private int SetupMockServer(Project project)
		{
			_server
				.Given(Request.Create()
					.WithPath($"/conversion-projects/{project.Id}")
					.UsingGet())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonSerializer.Serialize(project)));

			return project.Id;
		}
	}
}
