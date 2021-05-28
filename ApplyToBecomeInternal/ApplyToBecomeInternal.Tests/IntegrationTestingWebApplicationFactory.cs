using ApplyToBecome.Data;
using ApplyToBecomeInternal.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ApplyToBecomeInternal.Tests
{
	public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureAppConfiguration(config =>
			{
				var projectDir = Directory.GetCurrentDirectory();
				var configPath = Path.Combine(projectDir, "appsettings.json");

				config.Sources.Clear();
				config
					.AddJsonFile(configPath)
					.AddEnvironmentVariables();
			});

			builder.ConfigureTestServices(services =>
			{
				services.AddHttpClient<IProjects, ProjectsService>(client =>
				{
					var serviceProvider = services.BuildServiceProvider();
					using (var scope = serviceProvider.CreateScope())
					{
						var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
						var tramsApiOptions = config.GetSection(TramsApiOptions.Name).Get<TramsApiOptions>();
						client.BaseAddress = new Uri(tramsApiOptions.Endpoint);
						client.DefaultRequestHeaders.Add("ApiKey", tramsApiOptions.ApiKey);
					}
				});
			});
		}
	}
}
