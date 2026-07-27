using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.ProjectAssignment;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.ProjectAssignment;

public class SignificantChangeProjectAssignmentModelTests
{
   [Fact]
   public async Task OnGet_ShouldPopulatePageModelAndReturnPage()
   {
      const int id = 100;
      User assignedUser = new(Guid.NewGuid().ToString(), "assigned.user@test.local", "Assigned User");
      SignificantChangeProjectResponse project = new()
      {
         Id = id,
         SchoolName = "Test school",
         AssignedUser = assignedUser
      };

      Mock<ISignificantChangeProjectRepository> projectRepository = new();
      projectRepository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      List<User> users =
      [
         assignedUser,
         new User(Guid.NewGuid().ToString(), "other.user@test.local", "Other User")
      ];

      Mock<IUserRepository> userRepository = new();
      userRepository
         .Setup(x => x.GetAllUsers())
         .ReturnsAsync(users);

      IndexModel sut = BuildModel(userRepository.Object, projectRepository.Object);

      IActionResult result = await sut.OnGet(id);

      result.Should().BeOfType<PageResult>();
      sut.Id.Should().Be(id);
      sut.SchoolName.Should().Be(project.SchoolName);
      sut.SelectedDeliveryOfficer.Should().Be(assignedUser.FullName);
      sut.DeliveryOfficers.Should().BeEquivalentTo(users);

      projectRepository.Verify(x => x.GetProjectById(id), Times.Once);
      userRepository.Verify(x => x.GetAllUsers(), Times.Once);
   }

   [Fact]
   public async Task OnPost_WhenAssigningUser_ShouldSetAssignedUserAndRedirectToTaskList()
   {
      const int id = 101;
      User userToAssign = new(Guid.NewGuid().ToString(), "delivery.officer@test.local", "Delivery Officer");

      Mock<ISignificantChangeProjectRepository> projectRepository = new();
      projectRepository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, new SignificantChangeProjectResponse { Id = id }));
      projectRepository
         .Setup(x => x.SetAssignedUser(id, It.IsAny<SetAssignedUserSignificantChangeCommand>()))
         .Returns(Task.CompletedTask);

      Mock<IUserRepository> userRepository = new();
      userRepository
         .Setup(x => x.GetAllUsers())
         .ReturnsAsync(new[] { userToAssign });

      IndexModel sut = BuildModel(userRepository.Object, projectRepository.Object);

      IActionResult result = await sut.OnPost(id, userToAssign.FullName, false, userToAssign.FullName);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      sut.TempData["NotificationType"].Should().Be("success");
      sut.TempData["NotificationTitle"].Should().Be("Done");
      sut.TempData["NotificationMessage"].Should().Be("Project is assigned");

      projectRepository.Verify(
         x => x.SetAssignedUser(id, It.Is<SetAssignedUserSignificantChangeCommand>(command =>
            command.UserId == Guid.Parse(userToAssign.Id)
            && command.FullName == userToAssign.FullName
            && command.EmailAddress == userToAssign.EmailAddress)),
         Times.Once);
   }

   [Fact]
   public async Task OnPost_WhenUnassigningUser_ShouldSetEmptyAssignedUserAndRedirectToTaskList()
   {
      const int id = 102;

      Mock<ISignificantChangeProjectRepository> projectRepository = new();
      projectRepository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, new SignificantChangeProjectResponse { Id = id }));
      projectRepository
         .Setup(x => x.SetAssignedUser(id, It.IsAny<SetAssignedUserSignificantChangeCommand>()))
         .Returns(Task.CompletedTask);

      Mock<IUserRepository> userRepository = new();
      IndexModel sut = BuildModel(userRepository.Object, projectRepository.Object);

      IActionResult result = await sut.OnPost(id, "Ignored", true, "Ignored");

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      sut.TempData["NotificationType"].Should().Be("success");
      sut.TempData["NotificationTitle"].Should().Be("Done");
      sut.TempData["NotificationMessage"].Should().Be("Project is unassigned");

      projectRepository.Verify(
         x => x.SetAssignedUser(id, It.Is<SetAssignedUserSignificantChangeCommand>(command =>
            command.UserId == Guid.Empty
            && command.FullName == string.Empty
            && command.EmailAddress == string.Empty)),
         Times.Once);

      userRepository.Verify(x => x.GetAllUsers(), Times.Never);
   }

   [Fact]
   public async Task OnPost_WhenInputIsBlank_ShouldNotAssignAndShouldRedirectToTaskList()
   {
      const int id = 103;

      Mock<ISignificantChangeProjectRepository> projectRepository = new();
      projectRepository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, new SignificantChangeProjectResponse { Id = id }));

      Mock<IUserRepository> userRepository = new();
      IndexModel sut = BuildModel(userRepository.Object, projectRepository.Object);

      IActionResult result = await sut.OnPost(id, "Delivery Officer", false, " ");

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      projectRepository.Verify(x => x.SetAssignedUser(id, It.IsAny<SetAssignedUserSignificantChangeCommand>()), Times.Never);
      userRepository.Verify(x => x.GetAllUsers(), Times.Never);
   }

   [Fact]
   public async Task OnPost_WhenReturnQueryIsPresent_ShouldRedirectToReturnPageWithFragment()
   {
      const int id = 104;

      Mock<ISignificantChangeProjectRepository> projectRepository = new();
      projectRepository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, new SignificantChangeProjectResponse { Id = id }));

      Mock<IUserRepository> userRepository = new();
      IndexModel sut = BuildModel(userRepository.Object, projectRepository.Object, "?return=/SignificantChange/TaskList/Index&fragment=project-details");

      IActionResult result = await sut.OnPost(id, string.Empty, false, string.Empty);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be("/SignificantChange/TaskList/Index");
      redirect.Fragment.Should().Be("project-details");
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);
   }

   private static IndexModel BuildModel(
      IUserRepository userRepository,
      ISignificantChangeProjectRepository projectRepository,
      string queryString = "")
   {
      DefaultHttpContext httpContext = new();
      httpContext.Request.QueryString = new QueryString(queryString);

      ModelStateDictionary modelState = new();
      ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(), modelState);

      TempDataDictionary tempData = new(httpContext, Mock.Of<ITempDataProvider>());
      PageContext pageContext = new(actionContext)
      {
         ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
      };

      return new IndexModel(userRepository, projectRepository)
      {
         PageContext = pageContext,
         TempData = tempData
      };
   }
}
