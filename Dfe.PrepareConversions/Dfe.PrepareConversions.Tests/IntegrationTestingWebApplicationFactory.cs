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
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Dfe.PrepareConversions.Tests
{

   public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Startup>
   {
      private static int _currentPort = 5080;
      private static readonly object Sync = new();

      private readonly WireMockServer _mockApiServer;
      private readonly int _port;

      public IntegrationTestingWebApplicationFactory()
      {
         _port = AllocateNext();
         _mockApiServer = WireMockServer.Start(_port);
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
					.AddInMemoryCollection(new Dictionary<string, string> {
						{ "TramsApi:Endpoint", _mockApiServer.Url },
						{ "AcademisationApi:BaseUrl", _mockApiServer.Url },
						{ "AzureAd:AllowedRoles", string.Empty }, // Do not restrict access for integration test
						{ "ServiceLink:TransfersUrl", "https://an-external-service.com/" }
			   })
					.AddEnvironmentVariables();
			});

			var featureManager = new Mock<IFeatureManager>();
         featureManager.Setup(m => m.IsEnabledAsync(("UseAcademisation"))).ReturnsAsync(true);
         featureManager.Setup(m => m.IsEnabledAsync(("UseAcademisationApplication"))).ReturnsAsync(false);

         builder.ConfigureServices(services =>
			{
				services.AddAuthentication("Test");
				services.AddTransient<IAuthenticationSchemeProvider, MockSchemeProvider>();
				services.AddTransient<IUserRepository, TestUserRepository>();
				services.AddTransient(sp => featureManager.Object);
			});
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
               var scheme = new AuthenticationScheme(
                  "Test",
                  "Test",
                  typeof(MockAuthenticationHandler)
               );
               return Task.FromResult(scheme);
            }

            return base.GetSchemeAsync(name);
         }
      }

      public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
      {
         public MockAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
         )
            : base(options, logger, encoder, clock)
         {
         }

         protected override Task<AuthenticateResult> HandleAuthenticateAsync()
         {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Name") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
         }
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
            _mockApiServer.Stop();
         }
      }
   }
}
