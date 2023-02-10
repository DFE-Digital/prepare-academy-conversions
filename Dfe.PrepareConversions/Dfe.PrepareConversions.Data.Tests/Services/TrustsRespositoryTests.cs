using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models;
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

namespace Dfe.PrepareConversions.Data.Tests.Services
{
   public class TrustsRespositoryTests
	{
		[Theory, AutoMoqData]
		public async Task SearchTrusts_ReturnsTrusts(
			   [Frozen] Mock<IHttpClientService> httpService,
			   TrustsResponse expectedResponse,
			   MockHttpMessageHandler mockHandler,
			   string name)
		{
			// Arrange
			var sut = new TrustsRespository(new MockHttpClientFactory(mockHandler), httpService.Object);
			httpService.Setup(m => m.Get<TrustsResponse>(It.IsAny<HttpClient>(), It.IsAny<string>()))
				.ReturnsAsync(new ApiResponse<TrustsResponse>(HttpStatusCode.OK, expectedResponse));

			// Act
			var results = await sut.SearchTrusts(name);

			// Assert
			Assert.Equivalent(expectedResponse, results);
		}

		[Theory, AutoMoqData]
		public async Task Should_throw_exception(
				 [Frozen] Mock<IHttpClientService> httpService,
				 TrustsResponse expectedResponse,
				 MockHttpMessageHandler mockHandler,
				 string name)
		{
			// Arrange
			var sut = new TrustsRespository(new MockHttpClientFactory(mockHandler), httpService.Object);
			httpService.Setup(m => m.Get<TrustsResponse>(It.IsAny<HttpClient>(), It.IsAny<string>()))
				.ReturnsAsync(new ApiResponse<TrustsResponse>(HttpStatusCode.InternalServerError, expectedResponse));

			// Act
			var ex = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SearchTrusts(name));

			// Assert
			Assert.Equal("Request to Api failed | StatusCode - InternalServerError", ex.Message);
		}
	}
}
