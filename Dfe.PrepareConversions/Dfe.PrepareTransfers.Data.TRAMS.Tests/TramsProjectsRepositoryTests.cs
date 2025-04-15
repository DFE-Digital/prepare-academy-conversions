using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.TRAMS.Models;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace Dfe.PrepareTransfers.Data.TRAMS.Tests
{
    public class TramsProjectsRepositoryTests
    {
      private readonly Mock<IAcademies> _academies;
      private readonly Mock<IMapper<AcademisationProject, Project>> _externalToInternalProjectMapper;
      private readonly Mock<IMapper<Project, TramsProjectUpdate>> _internalToUpdateMapper;
      private readonly Mock<IMapper<TramsProjectSummary, ProjectSearchResult>> _summaryToInternalProjectMapper;
      private readonly Mock<ITrusts> _trusts;
      private readonly Mock<IDfeHttpClientFactory> _httpClientFactory;
      private readonly TramsProjectsRepository _subject;

      public TramsProjectsRepositoryTests()
      {
         _academies = new Mock<IAcademies>();
         _externalToInternalProjectMapper = new Mock<IMapper<AcademisationProject, Project>>();
         _internalToUpdateMapper = new Mock<IMapper<Project, TramsProjectUpdate>>();
         _summaryToInternalProjectMapper = new Mock<IMapper<TramsProjectSummary, ProjectSearchResult>>();
         _trusts = new Mock<ITrusts>();
         _httpClientFactory = new Mock<IDfeHttpClientFactory>();

         _subject = new TramsProjectsRepository(
            _httpClientFactory.Object,
            _externalToInternalProjectMapper.Object,
            _summaryToInternalProjectMapper.Object,
            _academies.Object,
            _trusts.Object,
            _internalToUpdateMapper.Object
         );
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

      private static PrepareConversions.Data.Models.SetTransferPublicEqualityDutyModel GetUpdateModel()
      {
         var urn = 12345;
         var publicEqualityDutyImpact = "Likely";
         var publicEqualityDutyReduceImpactReason = "Some reason";
         var publicEqualityDutySectionComplete = true;

         return new PrepareConversions.Data.Models.SetTransferPublicEqualityDutyModel(urn, publicEqualityDutyImpact, publicEqualityDutyReduceImpactReason, publicEqualityDutySectionComplete);
      }

      [Fact]
      public async Task When_SetTransferPublicEqualityDutyt_Returns_True()
      {
         // Arrange
         var httpClient = GetMockHttpClient("SendAsync", HttpStatusCode.OK);
         _httpClientFactory.Setup(s => s.CreateAcademisationClient()).Returns(httpClient);

         var model = GetUpdateModel();

         // Act
         var result = await _subject.SetTransferPublicEqualityDuty(model.Urn, model);

         // Assert
         Assert.True(result);
      }

      [Fact]
      public async Task When_SetTransferPublicEqualityDutyt_Throws_TramsApiException()
      {
         var httpClient = GetMockHttpClient("SendAsync", HttpStatusCode.NotFound);
         _httpClientFactory.Setup(s => s.CreateAcademisationClient()).Returns(httpClient);

         var model = GetUpdateModel();

         await Assert.ThrowsAsync<TramsApiException>(() => _subject.SetTransferPublicEqualityDuty(model.Urn, model));
      }
   }
}

