using GovUK.Dfe.CoreLibs.Testing.Authorization;
using GovUK.Dfe.CoreLibs.Testing.Authorization.Helpers;
using GovUK.Dfe.CoreLibs.Testing.Mocks.WebApplicationFactory;
using GovUK.Dfe.CoreLibs.Testing.Results;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages;

public class PageSecurityTests
{
   private readonly AuthorizationTester _validator;
   private static readonly Lazy<IEnumerable<RouteEndpoint>> _endpoints = new(InitializeEndpoints);
   private const bool _globalAuthorizationEnabled = true;

   public PageSecurityTests()
   {
      _validator = new AuthorizationTester(_globalAuthorizationEnabled);
   }

   [Theory]
   [MemberData(nameof(GetPageSecurityTestData))]
   public void ValidatePageSecurity(string route, string expectedSecurity)
   {
      ValidationResult result = _validator.ValidatePageSecurity(route, expectedSecurity, _endpoints.Value);
      Assert.Null(result.Message);
   }

   public static IEnumerable<object[]> GetPageSecurityTestData()
   {
      string configFilePath = "ExpectedSecurityConfig.json";
      IEnumerable<object[]> pages = EndpointTestDataProvider.GetPageSecurityTestDataFromFile(configFilePath, _endpoints.Value, _globalAuthorizationEnabled);
      return pages;
   }

   private static IEnumerable<RouteEndpoint> InitializeEndpoints()
   {
      // Using a temporary factory to access the EndpointDataSource for lazy initialization
      CustomWebApplicationFactory<Startup> factory = new();
      EndpointDataSource endpointDataSource = factory.Services.GetRequiredService<EndpointDataSource>();

      IEnumerable<RouteEndpoint> endpoints = endpointDataSource.Endpoints
         .OfType<RouteEndpoint>()
         .Where(x => !x.RoutePattern.RawText.Contains("MicrosoftIdentity") &&
                     !x.RoutePattern.RawText.Equals("/") &&
                     !x.Metadata.Any(m => m is RouteNameMetadata && ((RouteNameMetadata)m).RouteName == "default"));

      return endpoints;
   }
}