using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class SignificantChangeProjectRepositoryTests
{
   [Theory]
   [AutoMoqData]
   public async Task CreateProject_ShouldPostToApiAndReturnResponse(
      [Frozen] Mock<IHttpClientService> httpClientService,
      [Frozen] Mock<IDfeHttpClientFactory> httpClientFactory,
      SignificantChangeProjectRepository sut)
   {
      CreateSignificantProjectCommand command = new(123456, 2, "Route", "10000001");
      HttpClient httpClient = new();
      SignificantChangeProjectResponse expectedBody = new()
      {
         Id = 1,
         Urn = command.Urn,
         Tier = command.Tier,
         SchoolName = "Example School",
         TrustName = "Example Trust",
         TrustUkprn = command.TrustUkprn,
         TypeOfSignificantChange = "Fast track",
         Status = "Pre decision"
      };

      httpClientFactory
         .Setup(x => x.CreateAcademisationClient())
         .Returns(httpClient);

      httpClientService
         .Setup(x => x.Post<CreateSignificantProjectCommand, SignificantChangeProjectResponse>(
            httpClient,
            PathFor.CreateSignificantChangeProject,
            command))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.Created, expectedBody));

      ApiResponse<SignificantChangeProjectResponse> response = await sut.CreateProject(command);

      response.StatusCode.Should().Be(HttpStatusCode.Created);
      response.Body.Should().BeSameAs(expectedBody);

      httpClientFactory.Verify(x => x.CreateAcademisationClient(), Times.Once);
      httpClientService.Verify(x => x.Post<CreateSignificantProjectCommand, SignificantChangeProjectResponse>(
         httpClient,
         PathFor.CreateSignificantChangeProject,
         command), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task CreateProject_WhenApiCallFails_ShouldThrowApiResponseException(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      CreateSignificantProjectCommand command = new(123456, 2, "Route", "10000001");

      httpClientService
         .Setup(x => x.Post<CreateSignificantProjectCommand, SignificantChangeProjectResponse>(
            It.IsAny<HttpClient>(),
            PathFor.CreateSignificantChangeProject,
            command))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.InternalServerError, null));

      ApiResponseException exception = await Assert.ThrowsAsync<ApiResponseException>(() => sut.CreateProject(command));

      exception.Message.Should().Be("Request to Api failed | StatusCode - InternalServerError");
   }

   [Theory]
   [AutoMoqData]
   public async Task GetAllProjects_WhenApiCallSucceeds_ShouldReturnApiResponseBody(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      const int page = 3;
      const int count = 15;
      GetSignificantProjectsQuery expectedQuery = new(page, count);

      ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>> expectedBody = new()
      {
         Data = new[]
         {
            new SignificantChangeProjectResponse
            {
               Id = 99,
               Urn = 123456,
               Tier = 2,
               SchoolName = "Example School",
               TrustName = "Example Trust",
               TrustUkprn = "10000001",
               TypeOfSignificantChange = "Fast track",
               Status = "Pre decision"
            }
         },
         Paging = new ApiV2PagingInfo
         {
            Page = page,
            RecordCount = 1,
            NextPageUrl = "https://example.org/next"
         }
      };

      httpClientService
         .Setup(x => x.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            It.IsAny<HttpClient>(),
            PathFor.GetAllSignificantChangeProjects,
            expectedQuery))
         .ReturnsAsync(new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(HttpStatusCode.OK, expectedBody));

      ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>> response = await sut.GetAllProjects(page, count);

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      response.Body.Should().BeSameAs(expectedBody);

      httpClientService.Verify(x => x.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
         It.IsAny<HttpClient>(),
         PathFor.GetAllSignificantChangeProjects,
         expectedQuery), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task GetAllProjects_WhenApiCallFails_ShouldReturnEmptyDataAndDefaultPaging(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      const int page = 4;
      const int count = 20;
      GetSignificantProjectsQuery expectedQuery = new(page, count);

      httpClientService
         .Setup(x => x.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            It.IsAny<HttpClient>(),
            PathFor.GetAllSignificantChangeProjects,
            expectedQuery))
         .ReturnsAsync(new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(HttpStatusCode.BadRequest, null));

      ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>> response = await sut.GetAllProjects(page, count);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      response.Body.Should().NotBeNull();
      response.Body.Data.Should().NotBeNull();
      response.Body.Data.Should().BeEmpty();
      response.Body.Paging.Should().NotBeNull();
      response.Body.Paging.Page.Should().Be(page);
      response.Body.Paging.RecordCount.Should().Be(0);
      response.Body.Paging.NextPageUrl.Should().BeNull();
   }

   [Theory]
   [AutoMoqData]
   public async Task GetProjectById_ShouldBuildPathWithIdAndReturnApiResponse(
      [Frozen] Mock<IHttpClientService> httpClientService,
      [Frozen] Mock<IDfeHttpClientFactory> httpClientFactory,
      SignificantChangeProjectRepository sut)
   {
      const int id = 42;
      HttpClient httpClient = new();
      string expectedPath = string.Format(PathFor.GetSignificantChangeProjectById, id);

      SignificantChangeProjectResponse expectedBody = new()
      {
         Id = id,
         Urn = 123456,
         Tier = 2,
         SchoolName = "Example School",
         TrustName = "Example Trust",
         TrustUkprn = "10000001",
         TypeOfSignificantChange = "Fast track",
         Status = "Pre decision"
      };

      httpClientFactory
         .Setup(x => x.CreateAcademisationClient())
         .Returns(httpClient);

      httpClientService
         .Setup(x => x.Get<SignificantChangeProjectResponse>(httpClient, expectedPath))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, expectedBody));

      ApiResponse<SignificantChangeProjectResponse> response = await sut.GetProjectById(id);

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      response.Body.Should().BeSameAs(expectedBody);

      httpClientService.Verify(x => x.Get<SignificantChangeProjectResponse>(httpClient, expectedPath), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task GetProjectById_WhenApiCallFails_ShouldReturnStatusCodeAndNullBody(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      const int id = 42;
      string expectedPath = string.Format(PathFor.GetSignificantChangeProjectById, id);

      httpClientService
         .Setup(x => x.Get<SignificantChangeProjectResponse>(It.IsAny<HttpClient>(), expectedPath))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.NotFound, null));

      ApiResponse<SignificantChangeProjectResponse> response = await sut.GetProjectById(id);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      response.Body.Should().BeNull();
   }

   [Theory]
   [AutoMoqData]
   public async Task SetAssignedUser_WhenApiCallSucceeds_ShouldPutToExpectedPath(
      [Frozen] Mock<IHttpClientService> httpClientService,
      [Frozen] Mock<IDfeHttpClientFactory> httpClientFactory,
      SignificantChangeProjectRepository sut)
   {
      const int id = 42;
      string expectedPath = string.Format(PathFor.SetSignificantChangeAssignedUser, id);
      HttpClient httpClient = new();
      SetAssignedUserSignificantChangeCommand command = new(Guid.NewGuid(), "Delivery Officer", "delivery.officer@test.local");

      httpClientFactory
         .Setup(x => x.CreateAcademisationClient())
         .Returns(httpClient);

      httpClientService
         .Setup(x => x.Put<SetAssignedUserSignificantChangeCommand, object>(httpClient, expectedPath, command))
         .ReturnsAsync(new ApiResponse<object>(HttpStatusCode.OK, new object()));

      await sut.SetAssignedUser(id, command);

      httpClientService.Verify(
         x => x.Put<SetAssignedUserSignificantChangeCommand, object>(httpClient, expectedPath, command),
         Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task SetAssignedUser_WhenApiCallFails_ShouldThrowApiResponseException(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      const int id = 42;
      string expectedPath = string.Format(PathFor.SetSignificantChangeAssignedUser, id);
      SetAssignedUserSignificantChangeCommand command = new(Guid.NewGuid(), "Delivery Officer", "delivery.officer@test.local");

      httpClientService
         .Setup(x => x.Put<SetAssignedUserSignificantChangeCommand, object>(It.IsAny<HttpClient>(), expectedPath, command))
         .ReturnsAsync(new ApiResponse<object>(HttpStatusCode.InternalServerError, null));

      ApiResponseException exception = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SetAssignedUser(id, command));

      exception.Message.Should().Be("Request to Api failed | StatusCode - InternalServerError");
   }

   [Theory]
   [AutoMoqData]
   public async Task SetStakeholderConsultation_WhenApiCallSucceeds_ShouldPutToExpectedPath(
      [Frozen] Mock<IHttpClientService> httpClientService,
      [Frozen] Mock<IDfeHttpClientFactory> httpClientFactory,
      SignificantChangeProjectRepository sut)
   {
      const int id = 77;
      string expectedPath = string.Format(PathFor.SetSignificantChangeStakeholderConsultation, id);
      HttpClient httpClient = new();
      SetSignificantChangeStakeholderConsultationCommand command = new(false, "Consultation is scheduled next week");

      httpClientFactory
         .Setup(x => x.CreateAcademisationClient())
         .Returns(httpClient);

      httpClientService
         .Setup(x => x.Put<SetSignificantChangeStakeholderConsultationCommand, object>(httpClient, expectedPath, command))
         .ReturnsAsync(new ApiResponse<object>(HttpStatusCode.OK, new object()));

      await sut.SetStakeholderConsultation(id, command);

      httpClientService.Verify(
         x => x.Put<SetSignificantChangeStakeholderConsultationCommand, object>(httpClient, expectedPath, command),
         Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task SetStakeholderConsultation_WhenApiCallFails_ShouldThrowApiResponseException(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      const int id = 77;
      string expectedPath = string.Format(PathFor.SetSignificantChangeStakeholderConsultation, id);
      SetSignificantChangeStakeholderConsultationCommand command = new(false, "Reason");

      httpClientService
         .Setup(x => x.Put<SetSignificantChangeStakeholderConsultationCommand, object>(It.IsAny<HttpClient>(), expectedPath, command))
         .ReturnsAsync(new ApiResponse<object>(HttpStatusCode.InternalServerError, null));

      ApiResponseException exception = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SetStakeholderConsultation(id, command));

      exception.Message.Should().Be("Request to Api failed | StatusCode - InternalServerError");
   }

   [Theory]
   [AutoMoqData]
   public async Task SetConsultationDuration_WhenApiCallSucceeds_ShouldPutToExpectedPath(
      [Frozen] Mock<IHttpClientService> httpClientService,
      [Frozen] Mock<IDfeHttpClientFactory> httpClientFactory,
      SignificantChangeProjectRepository sut)
   {
      const int id = 78;
      string expectedPath = string.Format(PathFor.SetSignificantChangeConsultationDuration, id);
      HttpClient httpClient = new();
      SetSignificantChangeConsultationDurationCommand command = new(ConsultationDurationAnswer.No, "Consultation ran for two weeks only");

      httpClientFactory
         .Setup(x => x.CreateAcademisationClient())
         .Returns(httpClient);

      httpClientService
         .Setup(x => x.Put<SetSignificantChangeConsultationDurationCommand, object>(httpClient, expectedPath, command))
         .ReturnsAsync(new ApiResponse<object>(HttpStatusCode.OK, new object()));

      await sut.SetConsultationDuration(id, command);

      httpClientService.Verify(
         x => x.Put<SetSignificantChangeConsultationDurationCommand, object>(httpClient, expectedPath, command),
         Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task SetConsultationDuration_WhenApiCallFails_ShouldThrowApiResponseException(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      const int id = 78;
      string expectedPath = string.Format(PathFor.SetSignificantChangeConsultationDuration, id);
      SetSignificantChangeConsultationDurationCommand command = new(ConsultationDurationAnswer.No, "Reason");

      httpClientService
         .Setup(x => x.Put<SetSignificantChangeConsultationDurationCommand, object>(It.IsAny<HttpClient>(), expectedPath, command))
         .ReturnsAsync(new ApiResponse<object>(HttpStatusCode.InternalServerError, null));

      ApiResponseException exception = await Assert.ThrowsAsync<ApiResponseException>(() => sut.SetConsultationDuration(id, command));

      exception.Message.Should().Be("Request to Api failed | StatusCode - InternalServerError");
   }
}