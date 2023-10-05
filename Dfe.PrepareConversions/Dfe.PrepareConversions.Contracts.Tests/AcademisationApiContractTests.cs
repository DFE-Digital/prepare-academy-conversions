using PactNet;
using Dfe.PrepareConversions.Data.Features;
using Microsoft.FeatureManagement;
using Moq;
using System.Net;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using RichardSzalay.MockHttp;
using Dfe.Academisation.CorrelationIdMiddleware;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Xunit.Abstractions;
using PactNet.Infrastructure.Outputters;

namespace Dfe.PrepareConversions.Contracts.Tests
{
   public class AcademisationApiContractTests
   {
      private readonly IPactBuilderV3 pactBuilder;
      private readonly Mock<IFeatureManager> featureManager;
      private readonly PathFor pathFor;
      private readonly ITestOutputHelper _output;

      public AcademisationApiContractTests(ITestOutputHelper output)
      {
         featureManager = new();
         featureManager.Setup(m => m.IsEnabledAsync("UseAcademisationApplication")).ReturnsAsync(true);
         pathFor = new PathFor(featureManager.Object);
         _output = output;

         var config = new PactConfig
         {
            PactDir = @"../../../pacts",
            Outputters = new List<IOutput>
            { 
               new XunitOutput(_output),
               new ConsoleOutput()
            },
            DefaultJsonSettings = new JsonSerializerSettings
            {
               ContractResolver = new CamelCasePropertyNamesContractResolver(),
               Converters = new JsonConverter[] { new StringEnumConverter() }
            },
            LogLevel = PactLogLevel.Debug
         };

         var pact = Pact.V3(
            "prepare-academy-conversions",
            "academies-academisation-api",
            config);

         pactBuilder = pact.WithHttpInteractions();
      }

      [Fact]
      public async Task GetApplicationByReference_ShouldReturnApplication()
      {
         var responseBody = new List<object>()
         {
            new { foo = "bar" }
         };

         // Arrange
         pactBuilder
            .UponReceiving("A GET request for an Application Reference by ID")
               .Given("An application with ID 999999 exists")
               .WithRequest(HttpMethod.Get, "/application/999999/applicationReference")
               //.WithHeader("x-api-key", "foobar")
               //.WithHeader("x-correlationid", PactNet.Matchers.Match.Include(mockCorrelationId.ToString()))
               //.WithHeader("Accept", PactNet.Matchers.Match.Regex(".*", "application/json"))
               //.WithHeader("Content-Type", "application/json; charset=utf-8")
            .WillRespond()
               .WithStatus(HttpStatusCode.OK)
               .WithHeader("Content-Type", "application/json; charset=utf-8")
               .WithJsonBody(responseBody[0]);

         await pactBuilder.VerifyAsync(async ctx =>
         {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(x => x.CreateClient(DfeHttpClientFactory.AcademisationClientName)).Returns(Mock.Of<HttpClient>(x => x.BaseAddress == ctx.MockServerUri));
            
            var mockCorrelationId = Guid.NewGuid();
            var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

            var sut = new DfeHttpClientFactory(mockHttpClientFactory.Object, mockCorrelationContext);

            // Act
            var client = new ApiClient(sut, featureManager.Object, pathFor);
            var res = await client.GetApplicationByReferenceAsync("999999");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
         });
      }
   }
}