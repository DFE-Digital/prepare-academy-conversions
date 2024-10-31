using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages;

public class SecurityTests : BaseSecurityIntegrationTests
{
   public SecurityTests(SecurityIntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Check_Auth()
   {
      var _anonymousPages = new[] { "/Diagnostics", "/public/maintenance", "/public/accessibility", "/public/cookie-preferences", "/access-denied" };
      // Arrange
      var client = _factory.CreateClient(
          new WebApplicationFactoryClientOptions
          {
             AllowAutoRedirect = false
          });

      // Act

      var _endpointSources = _factory.Services.GetService<IEnumerable<EndpointDataSource>>();

      var endpoints = _endpointSources
                        .SelectMany(es => es.Endpoints)
                        .OfType<RouteEndpoint>()
                        .Where(es => 
                        !es.RoutePattern.RawText.Contains("MicrosoftIdentity") && 
                        !es.RoutePattern.RawText.Equals("/") &&
                        !es.Metadata.Any(m => m is RouteNameMetadata && ((RouteNameMetadata)m).RouteName == "default"));


      foreach (var endpoint in endpoints)
      {
         var route = "/" + endpoint.RoutePattern.RawText.Replace("Index", "").Trim('/');

         var isAnonymousPage = _anonymousPages.Contains(route);

         var hasAuthorizeMetadata = endpoint.Metadata.Any(m => m is AuthorizeAttribute);
         var hasAnonymousMetadata = endpoint.Metadata.Any(m => m is AllowAnonymousAttribute);
         var authorizeAttribute = endpoint.Metadata.OfType<AuthorizeAttribute>().FirstOrDefault();

         if (isAnonymousPage)
         {
            Assert.True(hasAnonymousMetadata, $"Page {route} should be anonymous.");
         }
         else
         {

            Assert.True(hasAuthorizeMetadata, $"Page {route} should be protected.");

         }
      }
   }
}
