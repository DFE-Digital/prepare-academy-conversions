using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Tests.Pages.ProjectAssignment;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Util;
using Xunit.Abstractions;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Dfe.PrepareConversions.Tests;

public class SecurityIntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>
{
   private static int _currentPort = 5080;
   private static readonly object Sync = new();

   private readonly WireMockServer _mockApiServer;

   public SecurityIntegrationTestingWebApplicationFactory()
   {
      int port = AllocateNext();
      _mockApiServer = WireMockServer.Start(port);
      _mockApiServer.LogEntriesChanged += EntriesChanged;
   }

   public ITestOutputHelper DebugOutput { get; set; }

   public IUserRepository UserRepository { get; private set; }

   public IEnumerable<LogEntry> GetMockServerLogs(string path, HttpMethod verb = null)
   {
      IRequestBuilder requestBuilder = Request.Create().WithPath(path);
      if (verb is not null) requestBuilder.UsingMethod(verb.Method);
      return _mockApiServer.FindLogEntries(requestBuilder);
   }

   private void EntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
   {
      DebugOutput?.WriteLine($"API Server change: {JsonConvert.SerializeObject(e)}");
   }

   protected override void ConfigureWebHost(IWebHostBuilder builder)
   {
      builder.ConfigureAppConfiguration(config =>
      {
         string projectDir = Directory.GetCurrentDirectory();
         string configPath = Path.Combine(projectDir, "appsettings.json");

         config.Sources.Clear();
         config
            .AddJsonFile(configPath)
            .AddInMemoryCollection(new Dictionary<string, string>
            {
               { "TramsApi:Endpoint", _mockApiServer.Url },
               { "AcademisationApi:BaseUrl", _mockApiServer.Url },
               { "AzureAd:AllowedRoles", string.Empty }, // Do not restrict access for integration test
               { "ServiceLink:TransfersUrl", "https://an-external-service.com/" }
            })
            .AddEnvironmentVariables();
      });

      Mock<IFeatureManager> featureManager = new();
      featureManager.Setup(m => m.IsEnabledAsync("UseAcademisationApplication")).ReturnsAsync(true);
      featureManager.Setup(m => m.IsEnabledAsync("ShowDirectedAcademyOrders")).ReturnsAsync(true);

      UserRepository = new TestUserRepository();

      builder.ConfigureServices(services =>
      {
         services.AddScoped(x => UserRepository);
         services.AddTransient(_ => featureManager.Object);
      });
   }

   private static int AllocateNext()
   {
      lock (Sync)
      {
         int next = _currentPort;
         _currentPort++;
         return next;
      }
   }

   protected override void Dispose(bool disposing)
   {
      base.Dispose(disposing);

      if (disposing)
      {
         _mockApiServer.Stop();
      }
   }
   public void Reset()
   {
      _mockApiServer.Reset();
   }

}
