using PactNet;
using Dfe.PrepareConversions.Data.Features;
using Microsoft.FeatureManagement;
using Moq;
using System.Net;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using RichardSzalay.MockHttp;
using Dfe.Academisation.CorrelationIdMiddleware;
using static System.Net.WebRequestMethods;

namespace Dfe.PrepareConversions.Contracts.Tests
{
   public class AcademisationApiContractTests
   {
      private readonly IPactBuilderV3 pactBuilder;
      private readonly Mock<IFeatureManager> featureManager;
      private readonly PathFor pathFor;

      public AcademisationApiContractTests()
      {
         featureManager = new();
         featureManager.Setup(m => m.IsEnabledAsync("UseAcademisationApplication")).ReturnsAsync(true);
         pathFor = new PathFor(featureManager.Object);

         var pact = Pact.V3(
            "prepare-academy-conversions",
            "academies-academisation-api",
            new PactConfig());

         pactBuilder = pact.WithHttpInteractions();
      }

      [Fact]
      public async Task GetApplicationByReference_ShouldReturnApplication()
      {
         var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(DfeHttpClientFactory.AcademisationClientName) == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always)).CreateClient(DfeHttpClientFactory.AcademisationClientName));
         var mockCorrelationId = Guid.NewGuid();
         var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

         var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

         // Arrange
         pactBuilder
            .UponReceiving("A GET request for an Application by Reference")
            .Given("An application exists")
               .WithRequest(HttpMethod.Get, "/application/999999/applicationReference")
               //.WithHeader("x-api-key", "foobar")
               .WithHeader("x-correlationid", mockCorrelationId.ToString())
               .WithHeader("Accept", PactNet.Matchers.Match.Regex(".*", "application/json"))
               //.WithHeader("Content-Type", "application/json; charset=utf-8")
            .WillRespond()
               .WithStatus(HttpStatusCode.OK)
               .WithHeader("Content-Type", "application/json; charset=utf-8")
               .WithJsonBody(new
               {
                  foo = "bar"
               });

         await pactBuilder.VerifyAsync(async ctx =>
         {
            // Act
            sut.CreateAcademisationClient().BaseAddress = ctx.MockServerUri;
            var client = new ApiClient(sut, featureManager.Object, pathFor);
            var res = await client.GetApplicationByReferenceAsync("999999");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
         });
      }
   }
}