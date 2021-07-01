using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ApplyToBecomeInternal.Tests
{
	public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>, IDisposable
	{
		private static int _currentPort = 5080;
		private static readonly object _sync = new object();

		private readonly WireMockServer _server;
		private readonly int _port;

		public IntegrationTestingWebApplicationFactory()
		{
			_port = AllocateNext();
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
					.WithBody(JsonConvert.SerializeObject(responseBody)));
		}



		public void AddPatchWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
		{
			_server
				.Given(Request.Create()
					.WithPath(path)
					.WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
					.UsingPatch())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		public void AddPostWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
		{
			_server
				.Given(Request.Create()
					.WithPath(path)
					.WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
					.UsingPost())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		public void AddErrorResponse(string path, string method)
		{
			_server
				.Given(Request.Create()
					.WithPath(path)
					.UsingMethod(method))
				.RespondWith(Response.Create()
					.WithStatusCode(500));
		}

		private static int AllocateNext()
		{
			lock (_sync)
			{
				var next = _currentPort;
				_currentPort++;
				return next;
			}
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
