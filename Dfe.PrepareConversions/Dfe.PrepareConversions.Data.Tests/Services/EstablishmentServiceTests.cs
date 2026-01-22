using AutoFixture.Xunit2;
using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class EstablishmentServiceTests
{
   //[Theory]
   //[AutoMoqData]
   //public async Task GetEstablishmentByUrn_ReturnsEstablishment_WhenApiReturnsSuccess(
   //   EstablishmentDto expectedResponse,
   //   Mock<ILogger<EstablishmentService>> mockLogger
   //)
   //{
   //   //// Arrange
   //   //var mockFactory = new Mock<IDfeHttpClientFactory>();
   //   //var mockLogger = new Mock<ILogger<EstablishmentService>>();
   //   //var mockHttpClientService = new Mock<IHttpClientService>();

   //   //var expected = new EstablishmentDto { /* set properties */ };
   //   //var handler = new MockHttpMessageHandler(expected, HttpStatusCode.OK);
   //   //var httpClient = new HttpClient(handler);

   //   //mockFactory.Setup(f => f.CreateTramsClient()).Returns(httpClient);

   //   //var httpClient = GetMockHttpClient("GetAsync", HttpStatusCode.OK);
   //   //var mockFactory = new Mock<IDfeHttpClientFactory>();

   //   //mockFactory.Setup(f => f.CreateTramsClient()).Returns(httpClient);

   //   var ukprn = "fake-ukprn";
   //   var url = $"/v4/establishment/{ukprn}";

   //   var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
   //   mockHttpMessageHandler.Protected()
   //       .Setup<Task<HttpResponseMessage>>("SendAsync", url, ItExpr.IsAny<CancellationToken>())
   //       .ReturnsAsync(new HttpResponseMessage { 
   //          StatusCode = HttpStatusCode.OK,
   //          Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedResponse))
   //          //Content = new StringContent("{ \"name\": \"Test School\", \"urn\": \"123456\" }")
   //       });

   //   var httpClient = new HttpClient(mockHttpMessageHandler.Object);
   //   httpClient.BaseAddress = new Uri("http://localhost");

   //   var mockFactory = new Mock<IDfeHttpClientFactory>();
   //   mockFactory.Setup(f => f.CreateTramsClient()).Returns(httpClient);

   //   var mockHttpClientService = new Mock<IHttpClientService>();

   //   var service = new EstablishmentService(mockFactory.Object, mockLogger.Object, mockHttpClientService.Object);

   //   // Act
   //   var result = await service.GetEstablishmentByUrn("123456");

   //   Assert.Equal(expectedResponse, result);
   //}

   //private static HttpClient GetMockHttpClient(string methodName, HttpStatusCode statusCode)
   //{
   //   var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
   //   mockHttpMessageHandler.Protected()
   //       .Setup<Task<HttpResponseMessage>>(methodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
   //       .ReturnsAsync(new HttpResponseMessage { StatusCode = statusCode });

   //   var httpClient = new HttpClient(mockHttpMessageHandler.Object);
   //   httpClient.BaseAddress = new Uri("http://localhost");

   //   return httpClient;
   //}

   [Theory]
   [AutoMoqData]
   public async Task EstablishmentService_GetEstablishmentByUkprn_Should_return_Establishment_given_ukprn(
      [Frozen] Mock<IHttpClientService> httpService,
      EstablishmentDto expectedResponse,
      Mock<ILogger<EstablishmentService>> logger,
      MockHttpMessageHandler mockHandler)
   {
      string ukprn = "fake-ukprn";

      httpService.Setup(m => m.Get<EstablishmentDto>(It.IsAny<HttpClient>(), $"/v4/establishment/{ukprn}"))
         .ReturnsAsync(new ApiResponse<EstablishmentDto>(HttpStatusCode.OK, expectedResponse));

      EstablishmentService sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), logger.Object, httpService.Object);

      EstablishmentDto dto = await sut.GetEstablishmentByUkprn(ukprn);

      Assert.Equal(expectedResponse, dto);
   }

   [Theory]
   [AutoMoqData]
   public async Task EstablishmentService_GetEstablishmentByUrn_Should_return_Establishment_given_urn(
      [Frozen] Mock<IHttpClientService> httpService,
      EstablishmentDto expectedResponse,
      Mock<ILogger<EstablishmentService>> logger,
      MockHttpMessageHandler mockHandler)
   {
      string urn = "fake-urn";

      httpService.Setup(m => m.Get<EstablishmentDto>(It.IsAny<HttpClient>(), $"/v4/establishment/urn/{urn}"))
         .ReturnsAsync(new ApiResponse<EstablishmentDto>(HttpStatusCode.OK, expectedResponse));

      EstablishmentService sut = new(new DfeHttpClientFactory(new MockHttpClientFactory(mockHandler), new CorrelationContext()), logger.Object, httpService.Object);

      EstablishmentDto dto = await sut.GetEstablishmentByUrn(urn);

      Assert.Equal(expectedResponse, dto);
   }

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
