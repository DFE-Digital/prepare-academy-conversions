using AutoFixture.Xunit2;
using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class TrustsRespositoryTests
{
   [Theory]
   [AutoMoqData]
   public async Task SearchTrusts_ReturnsTrusts(
      [Frozen] Mock<IHttpClientService> httpService,
      TrustSummaryResponse expectedResponse,
      MockHttpMessageHandler mockHandler,
      string name)
   {
      // Arrange
      TrustsRepository sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), httpService.Object);
      httpService.Setup(m => m.Get<TrustSummaryResponse>(It.IsAny<HttpClient>(), It.IsAny<string>()))
         .ReturnsAsync(new ApiResponse<TrustSummaryResponse>(HttpStatusCode.OK, expectedResponse));

      // Act
      TrustSummaryResponse results = await sut.SearchTrusts(name);

      // Assert
      Assert.Equivalent(expectedResponse, results);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_throw_exception(
      [Frozen] Mock<IHttpClientService> httpService,
      TrustSummaryResponse expectedResponse,
      MockHttpMessageHandler mockHandler,
      string name)
   {
      // Arrange
      TrustsRepository sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), httpService.Object);
      httpService.Setup(m => m.Get<TrustSummaryResponse>(It.IsAny<HttpClient>(), It.IsAny<string>()))
         .ReturnsAsync(new ApiResponse<TrustSummaryResponse>(HttpStatusCode.InternalServerError, expectedResponse));

      // Act
      ApiResponseException ex = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SearchTrusts(name));

      // Assert
      Assert.Equal("Request to Api failed | StatusCode - InternalServerError", ex.Message);
   }
}
