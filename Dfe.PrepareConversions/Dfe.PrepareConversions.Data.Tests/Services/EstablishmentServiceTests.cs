using AutoFixture.Xunit2;
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

namespace Dfe.PrepareConversions.Data.Tests.Services
{
	public class EstablishmentServiceTests
	{
		[Theory, AutoMoqData]
		public async Task Should_return_expected_result(
		   [Frozen] Mock<IHttpClientService> httpService,
		   IEnumerable<EstablishmentSearchResponse> expectedResponse,
		   Mock<ILogger<EstablishmentService>> logger,
		   MockHttpMessageHandler mockHandler,
		   string searchString)
		{
			var sut = new EstablishmentService(new MockHttpClientFactory(mockHandler), logger.Object, httpService.Object);
			httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
				.ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.OK, expectedResponse));

			var result = await sut.SearchEstablishments(searchString);

			Assert.Equal(expectedResponse, result);
		}

		[Theory, AutoMoqData]
		public async Task Should_searchby_urn(
			 [Frozen] Mock<IHttpClientService> httpService,
			 IEnumerable<EstablishmentSearchResponse> expectedResponse,
			 Mock<ILogger<EstablishmentService>> logger,
			 MockHttpMessageHandler mockHandler,
			 int urn)
		{
			// Arrange
			var sut = new EstablishmentService(new MockHttpClientFactory(mockHandler), logger.Object, httpService.Object);
			httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
				.ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.OK, expectedResponse));

			// Act
			await sut.SearchEstablishments(urn.ToString());

			// Assert
			httpService.Verify(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(
			   It.IsAny<HttpClient>(), $"establishments?urn={urn}"), Times.Once);
		}

		[Theory, AutoMoqData]
		public async Task Should_searchby_name(
			   [Frozen] Mock<IHttpClientService> httpService,
			   IEnumerable<EstablishmentSearchResponse> expectedResponse,
			   Mock<ILogger<EstablishmentService>> logger,
			   MockHttpMessageHandler mockHandler,
			   string name)
		{
			// Arrange
			var sut = new EstablishmentService(new MockHttpClientFactory(mockHandler), logger.Object, httpService.Object);
			httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
				.ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.OK, expectedResponse));

			// Act
			await sut.SearchEstablishments(name);

			// Assert
			httpService.Verify(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(
			   It.IsAny<HttpClient>(), $"establishments?name={name}"), Times.Once);
		}

		[Theory, AutoMoqData]
		public async Task Should_throw_exception(
				 [Frozen] Mock<IHttpClientService> httpService,
				 IEnumerable<EstablishmentSearchResponse> expectedResponse,
				 Mock<ILogger<EstablishmentService>> logger,
				 MockHttpMessageHandler mockHandler,
				 string name)
		{
			// Arrange
			var sut = new EstablishmentService(new MockHttpClientFactory(mockHandler), logger.Object, httpService.Object);
			httpService.Setup(m => m.Get<IEnumerable<EstablishmentSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
				.ReturnsAsync(new ApiResponse<IEnumerable<EstablishmentSearchResponse>>(HttpStatusCode.InternalServerError, expectedResponse));

			// Act
			var ex = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SearchEstablishments(name));

			// Assert
			Assert.Equal("Request to Api failed | StatusCode - InternalServerError", ex.Message);
		}
	}
}
