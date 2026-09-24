using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.PlanningPermission;
using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.PlanningPermission;

public class PlanningPermissionModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 701;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.PlanningPermission.PlanningPermissionAnswer = PlanningPermissionAnswer.No;
      project.PlanningPermission.AdditionalInformation = "Planning decision expected next month";
      project.PlanningPermission.SupportingEvidence = "Planning reference 12345";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.PlanningPermissionAnswer.Should().Be(PlanningPermissionAnswer.No);
      sut.PlanningPermissionAdditionalInformation.Should().Be("Planning decision expected next month");
      sut.PlanningPermissionSupportingEvidence.Should().Be("Planning reference 12345");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYes_ShouldSaveAndRedirect()
   {
      const int id = 702;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetPlanningPermission(id, It.IsAny<SetSignificantChangePlanningPermissionCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
   sut.PlanningPermissionAnswer = PlanningPermissionAnswer.Yes;
      sut.PlanningPermissionAdditionalInformation = "This should be cleared";
      sut.PlanningPermissionSupportingEvidence = "Planning approval reference 12345";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetPlanningPermission(
         id,
         It.Is<SetSignificantChangePlanningPermissionCommand>(command =>
            command.PlanningPermissionAnswer == PlanningPermissionAnswer.Yes
            && command.AdditionalInformation == null
            && command.SupportingEvidence == "Planning approval reference 12345")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithAdditionalInformation_ShouldSaveAndRedirect()
   {
      const int id = 703;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetPlanningPermission(id, It.IsAny<SetSignificantChangePlanningPermissionCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
   sut.PlanningPermissionAnswer = PlanningPermissionAnswer.No;
      sut.PlanningPermissionAdditionalInformation = "Planning permission is under review";
      sut.PlanningPermissionSupportingEvidence = "Planning application reference 67890";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetPlanningPermission(
         id,
         It.Is<SetSignificantChangePlanningPermissionCommand>(command =>
            command.PlanningPermissionAnswer == PlanningPermissionAnswer.No
            && command.AdditionalInformation == "Planning permission is under review"
            && command.SupportingEvidence == "Planning application reference 67890")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNotApplicable_ShouldSaveAndRedirect()
   {
      const int id = 704;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetPlanningPermission(id, It.IsAny<SetSignificantChangePlanningPermissionCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
   sut.PlanningPermissionAnswer = PlanningPermissionAnswer.NotApplicable;
      sut.PlanningPermissionAdditionalInformation = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<RedirectToPageResult>();
      repository.Verify(x => x.SetPlanningPermission(
         id,
         It.Is<SetSignificantChangePlanningPermissionCommand>(command =>
            command.PlanningPermissionAnswer == PlanningPermissionAnswer.NotApplicable
            && command.AdditionalInformation == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 705;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
   sut.PlanningPermissionAnswer = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
   sut.ModelState.ContainsKey(nameof(IndexModel.PlanningPermissionAnswer)).Should().BeTrue();
      repository.Verify(x => x.SetPlanningPermission(id, It.IsAny<SetSignificantChangePlanningPermissionCommand>()), Times.Never);
   }

   private static IndexModel BuildModel(ISignificantChangeProjectRepository repository)
   {
      DefaultHttpContext httpContext = new();
      ModelStateDictionary modelState = new();
      ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(), modelState);
      PageContext pageContext = new(actionContext)
      {
         ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
      };

      return new IndexModel(repository, new ErrorService())
      {
         PageContext = pageContext
      };
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         PlanningPermission = new SignificantChangePlanningPermissionResponse()
      };
   }
}