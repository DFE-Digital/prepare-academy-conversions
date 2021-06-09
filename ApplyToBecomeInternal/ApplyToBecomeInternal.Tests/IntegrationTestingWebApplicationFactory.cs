using ApplyToBecomeInternal.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ApplyToBecomeInternal.Tests
{
	public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>, IDisposable
	{
		private readonly WireMockServer _server;
		private readonly int _port;

		public IntegrationTestingWebApplicationFactory()
		{
			_port = PortHelper.AllocateNext();
			_server = WireMockServer.Start(_port);
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureAppConfiguration(config =>
			{
				var projectDir = Directory.GetCurrentDirectory();
				var configPath = Path.Combine(projectDir, "appsettings.json");

				config.Sources.Clear();
				config
					.AddJsonFile(configPath)
					.AddInMemoryCollection(new Dictionary<string, string>
					{
						{ "TramsApi:Endpoint", $"http://localhost:{_port}" }
					})
					.AddEnvironmentVariables();
			});
		}

		public void AddGetWithJsonResponse<TResponseBody>(string path, TResponseBody responseBody)
		{
			_server
				.Given(Request.Create()
					.WithPath(path)
					.UsingGet())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonSerializer.Serialize(responseBody)));
		}

		public void AddPutWithJsonRequest<TRequestBody>(string path, TRequestBody requestBody)
		{
			_server
				.Given(Request.Create()
					.WithPath(path)
					.WithBody(JsonSerializer.Serialize(requestBody))
					.UsingPut())
				.RespondWith(Response.Create()
					.WithStatusCode(204)
					.WithHeader("Content-Type", "application/json"));
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				_server.Stop();
			}
		}
	}
}
