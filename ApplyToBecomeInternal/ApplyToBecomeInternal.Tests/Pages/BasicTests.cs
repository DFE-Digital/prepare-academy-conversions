using ApplyToBecome.Data.Models;
using AutoFixture;
using System.Net.Http;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class BasicTests : IClassFixture<IntegrationTestingWebApplicationFactory>
	{
		private readonly HttpClient _client;
		private readonly WireMockServer _server;

		public BasicTests(IntegrationTestingWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
			_server = WireMockServer.Start(5080);
		}

		[Theory]
		[InlineData("/project-list")]
		public async Task Should_be_success_result_on_get(string url)
		{
			SetupMockServer();

			var response = await _client.GetAsync(url);

			response.EnsureSuccessStatusCode();
			Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
		}

		private void SetupMockServer()
		{
			var projects = new Fixture().CreateMany<Project>();
			var body = System.Text.Json.JsonSerializer.Serialize(projects);
			_server
				.Given(Request.Create()
					.WithPath("/conversion-projects")
					.UsingGet())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(body));
		}
	}
}
