using AutoFixture.Xunit2;
using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class EstablishmentServiceTests
{
   [Theory]
   [AutoMoqData]
   public async Task Should_return_expected_result(
      [Frozen] Mock<IHttpClientService> httpService,
      IList<EstablishmentSearchResponse> expectedResponse,
      Mock<ILogger<EstablishmentService>> logger,
      MockHttpMessageHandler mockHandler,
      string searchString)
   {
      EstablishmentService sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), logger.Object, httpService.Object);
      httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
         .ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.OK, expectedResponse));

      IEnumerable<EstablishmentSearchResponse> result = await sut.SearchEstablishments(searchString);

      Assert.Equal(expectedResponse, result);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_search_by_urn(
      [Frozen] Mock<IHttpClientService> httpService,
      IEnumerable<EstablishmentSearchResponse> expectedResponse,
      Mock<ILogger<EstablishmentService>> logger,
      MockHttpMessageHandler mockHandler,
      int urn)
   {
      // Arrange
      EstablishmentService sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), logger.Object, httpService.Object);
      httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
         .ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.OK, expectedResponse));

      // Act
      await sut.SearchEstablishments(urn.ToString());

      // Assert
      httpService.Verify(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(
         It.IsAny<HttpClient>(), $"/v4/establishments?urn={urn}"), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_search_by_name(
      [Frozen] Mock<IHttpClientService> httpService,
      IEnumerable<EstablishmentSearchResponse> expectedResponse,
      Mock<ILogger<EstablishmentService>> logger,
      MockHttpMessageHandler mockHandler,
      string name)
   {
      // Arrange
      EstablishmentService sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), logger.Object, httpService.Object);
      httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
         .ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.OK, expectedResponse));

      // Act
      await sut.SearchEstablishments(name);

      // Assert
      httpService.Verify(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(
         It.IsAny<HttpClient>(), $"/v4/establishments?name={name}"), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_throw_exception(
      [Frozen] Mock<IHttpClientService> httpService,
      IEnumerable<EstablishmentSearchResponse> expectedResponse,
      Mock<ILogger<EstablishmentService>> logger,
      MockHttpMessageHandler mockHandler,
      string name)
   {
      // Arrange
      EstablishmentService sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), logger.Object, httpService.Object);
      httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
         .ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.InternalServerError, expectedResponse));

      // Act
      ApiResponseException ex = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SearchEstablishments(name));

      // Assert
      Assert.Equal("Request to Api failed | StatusCode - InternalServerError", ex.Message);
   }
}
