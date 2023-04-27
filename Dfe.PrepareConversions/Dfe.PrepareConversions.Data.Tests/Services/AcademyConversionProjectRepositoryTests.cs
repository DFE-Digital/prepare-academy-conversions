using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.InvoluntaryProject;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class AcademyConversionProjectRepositoryTests
{
   private readonly Mock<IApiClient> _mockApiClient;
   private AcademyConversionProjectRepository _subject;

   public AcademyConversionProjectRepositoryTests()
   {
      _mockApiClient = new Mock<IApiClient>();
   }

   [Fact]
   public void GivenDeliveryOfficers_GetsRelevantProjects()
   {
      List<AcademyConversionProject> project = new() { new AcademyConversionProject { AssignedUser = new User("1", "example@email.com", "John Smith") } };
      ApiV2Wrapper<IEnumerable<AcademyConversionProject>> projects = new() { Data = project };

      JsonContent jsonContent = JsonContent.Create(projects, projects.GetType(), MediaTypeHeaderValue.Parse("application/json"), JsonSerializerOptions.Default);

      _mockApiClient.Setup(x => x.GetAllProjectsAsync(It.IsAny<AcademyConversionSearchModel>()))
         .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = jsonContent }));

      _subject = new AcademyConversionProjectRepository(_mockApiClient.Object);
      List<string> deliveryOfficers = new() { "John Smith", "Jane Doe" };

      string firstAssignedFullName = _subject.GetAllProjects(1, 50, string.Empty, default, deliveryOfficers).Result.Body.Data.First().AssignedUser.FullName;

      firstAssignedFullName.Should().Be(project.First().AssignedUser.FullName);
   }

   [Fact]
   public void GivenDeliveryOfficers_GetsRelevantProjects_WhenMultiple()
   {
      List<AcademyConversionProject> project = new()
      {
         new AcademyConversionProject { AssignedUser = new User("1", "example@email.com", "John Smith") },
         new AcademyConversionProject { AssignedUser = new User("2", "example@email.com", "John Smith") }
      };
      ApiV2Wrapper<IEnumerable<AcademyConversionProject>> projects = new() { Data = project };

      JsonContent jsonContent = JsonContent.Create(projects, projects.GetType(), MediaTypeHeaderValue.Parse("application/json"), JsonSerializerOptions.Default);

      _mockApiClient.Setup(x => x.GetAllProjectsAsync(It.IsAny<AcademyConversionSearchModel>()))
         .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = jsonContent }));

      _subject = new AcademyConversionProjectRepository(_mockApiClient.Object);
      List<string> deliveryOfficers = new() { "John Smith" };

      int projectCountReceived = _subject.GetAllProjects(1, 50, string.Empty, default, deliveryOfficers).Result.Body.Data.Count();

      projectCountReceived.Should().Be(project.Count);
   }

   [Fact]
   public void GivenDeliveryOfficers_GetsNoProjects_WhenDeliveryOfficerHasNoneAssigned()
   {
      List<AcademyConversionProject> project = new();
      ApiV2Wrapper<IEnumerable<AcademyConversionProject>> projects = new() { Data = project };

      JsonContent jsonContent = JsonContent.Create(projects, projects.GetType(), MediaTypeHeaderValue.Parse("application/json"), JsonSerializerOptions.Default);

      _mockApiClient.Setup(x => x.GetAllProjectsAsync(It.IsAny<AcademyConversionSearchModel>()))
         .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = jsonContent }));

      _subject = new AcademyConversionProjectRepository(_mockApiClient.Object);
      List<string> deliveryOfficers = new() { "John Smith" };

      IEnumerable<AcademyConversionProject> data = _subject.GetAllProjects(1, 50, string.Empty, default, deliveryOfficers).Result.Body.Data;

      data.Should().BeEmpty();
   }

   [Theory]
   [AutoMoqData]
   public async Task GivenAValidProject_PostToApi([Frozen] Mock<IHttpClientService> httpService, AcademyConversionProjectRepository subject)
   {
      httpService.Setup(m => m.Post<CreateInvoluntaryProject, string>(
            It.IsAny<HttpClient>(), @"legacy/project/involuntary-conversion-project", It.IsAny<CreateInvoluntaryProject>()))
         .ReturnsAsync(new ApiResponse<string>(HttpStatusCode.OK, string.Empty));

      CreateInvoluntaryProject project = new(null, null);
      await subject.CreateInvoluntaryProject(project);

      httpService.Verify(m => m.Post<CreateInvoluntaryProject, string>(
         It.IsAny<HttpClient>(), @"legacy/project/involuntary-conversion-project", project), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task GivenAFailedResponse_ThrowAnException([Frozen] Mock<IHttpClientService> httpService, AcademyConversionProjectRepository subject)
   {
      httpService.Setup(m => m.Post<CreateInvoluntaryProject, string>(
            It.IsAny<HttpClient>(), @"legacy/project/involuntary-conversion-project", It.IsAny<CreateInvoluntaryProject>()))
         .ReturnsAsync(new ApiResponse<string>(HttpStatusCode.InternalServerError, string.Empty));

      CreateInvoluntaryProject project = new(null, null);
      ApiResponseException exception = await Assert.ThrowsAsync<ApiResponseException>(() => subject.CreateInvoluntaryProject(project));

      Assert.Equal("Request to Api failed | StatusCode - InternalServerError", exception.Message);
   }
}
