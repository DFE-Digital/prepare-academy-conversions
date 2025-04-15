using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.FeatureManagement;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Features
{
    public class ApiClientTests
    {
      private readonly Mock<IFeatureManager> _features;
      private readonly PathFor _pathFor;
      private readonly Mock<IDfeHttpClientFactory> _httpClientFactory;
      private readonly ApiClient _subject;

      public ApiClientTests()
      {
         _features = new Mock<IFeatureManager>();
         _pathFor = new PathFor(_features.Object);
         _httpClientFactory = new Mock<IDfeHttpClientFactory>();
         _subject = new ApiClient(_httpClientFactory.Object, _features.Object, _pathFor);
      }

      private static HttpClient GetMockHttpClient(string methodName, HttpStatusCode statusCode)
      {
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
             .Setup<Task<HttpResponseMessage>>(methodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
             .ReturnsAsync(new HttpResponseMessage { StatusCode = statusCode });

         var httpClient = new HttpClient(mockHttpMessageHandler.Object);
         httpClient.BaseAddress = new Uri("http://localhost");

         return httpClient;
      }

      [Fact]
      public async Task When_SetConversionPublicEqualityDuty_Returns_HttpResponseMessage()
      {
         // Arrange
         var httpClient = GetMockHttpClient("SendAsync", HttpStatusCode.OK);
         _httpClientFactory.Setup(s => s.CreateAcademisationClient()).Returns(httpClient);

         var id = 12345;
         var publicEqualityDutyImpact = "Likely";
         var publicEqualityDutyReduceImpactReason = "Some reason";
         var publicEqualityDutySectionComplete = true;

         var model = new SetConversionPublicEqualityDutyModel(id, publicEqualityDutyImpact, publicEqualityDutyReduceImpactReason, publicEqualityDutySectionComplete);

         // Act
         var response = await _subject.SetConversionPublicEqualityDuty(id, model);

         // Assert
         Assert.True(response.IsSuccessStatusCode, $"Actual status code: {response.StatusCode}.");
      }
    }
}
