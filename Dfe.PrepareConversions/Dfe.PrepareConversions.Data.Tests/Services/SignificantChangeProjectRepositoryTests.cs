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
   public async Task GetAllProjects_WithNoFilters_ShouldSendNullForEveryFilterMember(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      GetSignificantProjectsQuery capturedQuery = null;

      httpClientService
         .Setup(x => x.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            It.IsAny<HttpClient>(),
            PathFor.GetAllSignificantChangeProjects,
            It.IsAny<GetSignificantProjectsQuery>()))
         .Callback<HttpClient, string, GetSignificantProjectsQuery>((_, _, query) => capturedQuery = query)
         .ReturnsAsync(new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(HttpStatusCode.OK, null));

      await sut.GetAllProjects(1, 10);

      capturedQuery.Should().NotBeNull();
      capturedQuery.Keyword.Should().BeNull();
      capturedQuery.Status.Should().BeNull();
      capturedQuery.Assignee.Should().BeNull();
      capturedQuery.Tier.Should().BeNull();
      capturedQuery.Route.Should().BeNull();
   }

   [Theory]
   [AutoMoqData]
   public async Task GetAllProjects_WithEmptyFilters_ShouldNormaliseThemToNull(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      GetSignificantProjectsQuery capturedQuery = null;

      httpClientService
         .Setup(x => x.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            It.IsAny<HttpClient>(),
            PathFor.GetAllSignificantChangeProjects,
            It.IsAny<GetSignificantProjectsQuery>()))
         .Callback<HttpClient, string, GetSignificantProjectsQuery>((_, _, query) => capturedQuery = query)
         .ReturnsAsync(new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(HttpStatusCode.OK, null));

      // Empty arrays and a whitespace-only keyword must not become empty lists — record equality on
      // List<T> is reference equality, so an empty list breaks request-body matching downstream.
      await sut.GetAllProjects(1, 10, "   ", [], [], [], []);

      capturedQuery.Keyword.Should().BeNull();
      capturedQuery.Status.Should().BeNull();
      capturedQuery.Assignee.Should().BeNull();
      capturedQuery.Tier.Should().BeNull();
      capturedQuery.Route.Should().BeNull();
   }

   [Theory]
   [AutoMoqData]
   public async Task GetAllProjects_WithFilters_ShouldSendTrimmedKeywordAndPopulatedLists(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      GetSignificantProjectsQuery capturedQuery = null;

      httpClientService
         .Setup(x => x.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            It.IsAny<HttpClient>(),
            PathFor.GetAllSignificantChangeProjects,
            It.IsAny<GetSignificantProjectsQuery>()))
         .Callback<HttpClient, string, GetSignificantProjectsQuery>((_, _, query) => capturedQuery = query)
         .ReturnsAsync(new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(HttpStatusCode.OK, null));

      await sut.GetAllProjects(
         2, 20,
         "  Example School  ",
         ["PreDecision"],
         ["Assigned User", "Not assigned"],
         [1, 3],
         ["Change of age range"]);

      capturedQuery.Page.Should().Be(2);
      capturedQuery.Count.Should().Be(20);
      capturedQuery.Keyword.Should().Be("Example School");
      capturedQuery.Status.Should().BeEquivalentTo(["PreDecision"]);
      capturedQuery.Assignee.Should().BeEquivalentTo(["Assigned User", "Not assigned"]);
      capturedQuery.Tier.Should().BeEquivalentTo([(byte)1, (byte)3]);
      capturedQuery.Route.Should().BeEquivalentTo(["Change of age range"]);
   }

   [Theory]
   [AutoMoqData]
   public async Task GetFilterParameters_WhenApiCallSucceeds_ShouldReturnApiResponseBody(
      [Frozen] Mock<IHttpClientService> httpClientService,
      [Frozen] Mock<IDfeHttpClientFactory> httpClientFactory,
      SignificantChangeProjectRepository sut)
   {
      HttpClient httpClient = new();
      SignificantChangeFilterParameters expectedBody = new()
      {
         Statuses = [new FilterValueDisplay { Value = "PreDecision", Display = "Pre decision" }],
         Tiers = [new FilterValueDisplay { Value = "1", Display = "Tier 1" }],
         AssignedUsers = [new FilterValueDisplay { Value = "Bob", Display = "Bob" }],
         Routes = [new FilterValueDisplay { Value = "Other", Display = "Other" }]
      };

      httpClientFactory
         .Setup(x => x.CreateAcademisationClient())
         .Returns(httpClient);

      httpClientService
         .Setup(x => x.Get<SignificantChangeFilterParameters>(httpClient, PathFor.GetSignificantChangeFilterParameters))
         .ReturnsAsync(new ApiResponse<SignificantChangeFilterParameters>(HttpStatusCode.OK, expectedBody));

      ApiResponse<SignificantChangeFilterParameters> response = await sut.GetFilterParameters();

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      response.Body.Should().BeSameAs(expectedBody);
   }

   [Theory]
   [AutoMoqData]
   public async Task GetFilterParameters_WhenApiCallFails_ShouldReturnEmptyBodyNotNull(
      [Frozen] Mock<IHttpClientService> httpClientService,
      SignificantChangeProjectRepository sut)
   {
      httpClientService
         .Setup(x => x.Get<SignificantChangeFilterParameters>(
            It.IsAny<HttpClient>(),
            PathFor.GetSignificantChangeFilterParameters))
         .ReturnsAsync(new ApiResponse<SignificantChangeFilterParameters>(HttpStatusCode.NotFound, null));

      ApiResponse<SignificantChangeFilterParameters> response = await sut.GetFilterParameters();

      // Fail soft: a missing endpoint must give an empty panel, never a null-reference on the page.
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      response.Body.Should().NotBeNull();
      response.Body.Statuses.Should().BeEmpty();
      response.Body.Tiers.Should().BeEmpty();
      response.Body.AssignedUsers.Should().BeEmpty();
      response.Body.Routes.Should().BeEmpty();
   }
}