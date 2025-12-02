using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Services.Person;
using Dfe.PrepareConversions.Tests.Pages.ProjectAssignment;
using GovUK.Dfe.PersonsApi.Client;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
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

public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>
{
   private static int _currentPort = 5080;
   private static readonly object Sync = new();

   private readonly WireMockServer _mockApiServer;

   public IntegrationTestingWebApplicationFactory()
   {
      int port = AllocateNext();
      _mockApiServer = WireMockServer.Start(port);
      _mockApiServer.LogEntriesChanged += EntriesChanged;
   }

   public ITestOutputHelper DebugOutput { get; set; }

   public IUserRepository UserRepository { get; private set; }

   public IEnumerable<ILogEntry> GetMockServerLogs(string path, HttpMethod verb = null)
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
               { "ServiceLink:TransfersUrl", "https://an-external-service.com/" },
               { "EnableDistributedCache", "false" }
            })
            .AddEnvironmentVariables();
      });

      Mock<IFeatureManager> featureManager = new();
      featureManager.Setup(m => m.IsEnabledAsync("UseAcademisationApplication")).ReturnsAsync(true);
      featureManager.Setup(m => m.IsEnabledAsync("ShowDirectedAcademyOrders")).ReturnsAsync(true);

      UserRepository = new TestUserRepository();

      builder.ConfigureServices(services =>
      {
         services.AddAuthentication("Test");
         services.AddTransient<IAuthenticationSchemeProvider, MockSchemeProvider>();
         services.AddScoped(x => UserRepository);
         services.AddTransient(_ => featureManager.Object);

         // Remove and replace Redis
         ServiceDescriptor descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDistributedCache));
         if (descriptor != null) services.Remove(descriptor);

         services.AddScoped<IEstablishmentsClient, MockEstablishmentsClient>();
         services.AddScoped<IPersonApiEstablishmentsService, PersonApiEstablishmentsService>();

         services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
      });
   }


   public void AddGetWithJsonResponse<TResponseBody>(string path, TResponseBody responseBody)
   {
      _mockApiServer
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
      _mockApiServer
         .Given(Request.Create()
            .WithPath(path)
            .WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
            .UsingPatch())
         .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonConvert.SerializeObject(responseBody)));
   }

   public void AddApiCallWithBodyDelegate<TResponseBody>(string path, Func<IBodyData, bool> bodyDelegate, TResponseBody responseBody, HttpMethod verb = null)
   {
      _mockApiServer
         .Given(Request.Create()
            .WithPath(path)
            .WithBody(bodyDelegate)
            .UsingMethod(verb == null ? HttpMethod.Post.ToString() : verb.ToString()))
         .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonConvert.SerializeObject(responseBody)));
   }

   public void AddPutWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
   {
      _mockApiServer
         .Given(Request.Create()
            .WithPath(path)
            .WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
            .UsingPut())
         .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonConvert.SerializeObject(responseBody)));
   }

   public void AddPostWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
   {
      _mockApiServer
         .Given(Request.Create()
            .WithPath(path)
            .WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
            .UsingPost())
         .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonConvert.SerializeObject(responseBody)));
   }

   public void AddAnyPostWithJsonRequest<TResponseBody>(string path, TResponseBody responseBody)
   {
      _mockApiServer
         .Given(Request.Create()
            .WithPath(path)
            .UsingPost())
         .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(JsonConvert.SerializeObject(responseBody)));
   }

   public void AddErrorResponse(string path, string method)
   {
      _mockApiServer
         .Given(Request.Create()
            .WithPath(path)
            .UsingMethod(method))
         .RespondWith(Response.Create()
            .WithStatusCode(500));
   }

   public void Reset()
   {
      _mockApiServer.Reset();
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

   public class MockSchemeProvider : AuthenticationSchemeProvider
   {
      public MockSchemeProvider(IOptions<AuthenticationOptions> options)
         : base(options)
      {
      }

      protected MockSchemeProvider(
         IOptions<AuthenticationOptions> options,
         IDictionary<string, AuthenticationScheme> schemes
      )
         : base(options, schemes)
      {
      }

      public override Task<AuthenticationScheme> GetSchemeAsync(string name)
      {
         if (name == "Test")
         {
            AuthenticationScheme scheme = new(
               "Test",
               "Test",
               typeof(MockAuthenticationHandler)
            );
            return Task.FromResult(scheme);
         }

         return base.GetSchemeAsync(name);
      }
   }

   public class MockAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder)
      : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
   {
      protected override Task<AuthenticateResult> HandleAuthenticateAsync()
      {
         List<Claim> claims = [new(ClaimTypes.Name, "Name"), new(ClaimTypes.Role, "conversionRole"), new(ClaimTypes.Role, "transferRoles")];
         ClaimsIdentity identity = new(claims, "Test");
         ClaimsPrincipal principal = new(identity);
         AuthenticationTicket ticket = new(principal, "Test");

         return Task.FromResult(AuthenticateResult.Success(ticket));
      }
   }

   public class MockEstablishmentsClient : IEstablishmentsClient
   {
      public Task<ObservableCollection<AcademyGovernance>> GetAllPersonsAssociatedWithAcademyByUrnAsync(int urn)
      {
         throw new NotImplementedException();
      }

      public Task<ObservableCollection<AcademyGovernance>> GetAllPersonsAssociatedWithAcademyByUrnAsync(int urn, CancellationToken cancellationToken)
      {
         throw new NotImplementedException();
      }

      public Task<MemberOfParliament> GetMemberOfParliamentBySchoolUrnAsync(int urn)
      {
         throw new NotImplementedException();
      }

      public Task<MemberOfParliament> GetMemberOfParliamentBySchoolUrnAsync(int urn, CancellationToken cancellationToken)
      {
         throw new NotImplementedException();
      }
   }
}