using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.StakeholderConsultation;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.StakeholderConsultation;

public class StakeholderConsultationModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 401;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.StakeholderConsultation.TrustConsultedStakeholders = false;
      project.StakeholderConsultation.TrustConsultedStakeholdersNotConsultedReason = "Consultation planned for next month";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.TrustConsultedStakeholders.Should().BeFalse();
      sut.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Consultation planned for next month");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYes_ShouldSaveAndRedirect()
   {
      const int id = 402;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetStakeholderConsultation(id, It.IsAny<SetSignificantChangeStakeholderConsultationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = true;
      sut.TrustConsultedStakeholdersNotConsultedReason = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetStakeholderConsultation(
         id,
         It.Is<SetSignificantChangeStakeholderConsultationCommand>(command =>
            command.TrustConsultedStakeholders == true
            && command.TrustConsultedStakeholdersNotConsultedReason == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithReason_ShouldSaveAndRedirect()
   {
      const int id = 403;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetStakeholderConsultation(id, It.IsAny<SetSignificantChangeStakeholderConsultationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = false;
      sut.TrustConsultedStakeholdersNotConsultedReason = "Consultation starts after governance meeting";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetStakeholderConsultation(
         id,
         It.Is<SetSignificantChangeStakeholderConsultationCommand>(command =>
            command.TrustConsultedStakeholders == false
            && command.TrustConsultedStakeholdersNotConsultedReason == "Consultation starts after governance meeting")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithoutReason_ShouldReturnPageWithValidationError()
   {
      const int id = 404;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = false;
      sut.TrustConsultedStakeholdersNotConsultedReason = " ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.TrustConsultedStakeholdersNotConsultedReason)).Should().BeTrue();
      repository.Verify(x => x.SetStakeholderConsultation(id, It.IsAny<SetSignificantChangeStakeholderConsultationCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 405;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.TrustConsultedStakeholders)).Should().BeTrue();
      repository.Verify(x => x.SetStakeholderConsultation(id, It.IsAny<SetSignificantChangeStakeholderConsultationCommand>()), Times.Never);
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
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse()
      };
   }
}
