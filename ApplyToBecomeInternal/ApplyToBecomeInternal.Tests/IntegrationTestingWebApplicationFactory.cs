using ApplyToBecomeInternal.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

		public WireMockServer WMServer => _server;

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
