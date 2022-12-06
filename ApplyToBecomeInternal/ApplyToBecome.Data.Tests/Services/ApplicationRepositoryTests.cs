using ApplyToBecome.Data.Features;
using ApplyToBecome.Data.Models.Application;
using ApplyToBecome.Data.Services;
using AutoFixture.Xunit2;
using FluentAssertions;
using MELT;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecome.Data.Tests.Services;

public class ApplicationRepositoryTests
{
   private readonly ApplicationRepository _applicationRepository;
   private readonly Mock<IApiClient> _mockApiClient;
   private readonly MockHttpMessageHandler _mockHandler;

   public ApplicationRepositoryTests()
   {
      _mockHandler = new MockHttpMessageHandler();
      _mockApiClient = new Mock<IApiClient>();

      ITestLoggerFactory testLogger = TestLoggerFactory.Create();
      _applicationRepository = new ApplicationRepository(_mockApiClient.Object, testLogger.CreateLogger<ApplicationRepository>()
      );
   }

   [Theory]
   [AutoData]
   public async Task Should_get_application_data_by_application_reference(Application applicationMockData)
   {
      ApiV2Wrapper<Application> responseObject = new() { Data = applicationMockData };

      JsonContent jsonContent = JsonContent.Create(responseObject, responseObject.GetType(), MediaTypeHeaderValue.Parse("application/json"), JsonSerializerOptions.Default);

      _mockApiClient.Setup(x => x.GetApplicationByReferenceAsync(It.IsAny<string>()))
         .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = jsonContent }));

      ApiResponse<Application> applicationResponse = await _applicationRepository.GetApplicationByReference(applicationMockData.ApplicationId);

      applicationResponse.Success.Should().BeTrue();
      applicationResponse.Body.Should().BeEquivalentTo(applicationMockData);
   }

   [Fact]
   public async Task Should_return_not_success_when_application_not_found()
   {
      // API responses might need more than just Success and !Success 
      string applicationReference = "123a";

      _mockApiClient.Setup(x => x.GetApplicationByReferenceAsync(applicationReference))
         .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));

      ApiResponse<Application> applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReference);

      applicationResponse.Success.Should().BeFalse();
   }
}
