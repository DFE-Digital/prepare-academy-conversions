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

			_server
				.Given(Request.Create()
					.WithPath($"/conversion-projects/{project.Id}")
					.UsingPut())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json"));
					//.WithBody(JsonSerializer.Serialize(project)));

			return project.Id;
		}
	}
}
