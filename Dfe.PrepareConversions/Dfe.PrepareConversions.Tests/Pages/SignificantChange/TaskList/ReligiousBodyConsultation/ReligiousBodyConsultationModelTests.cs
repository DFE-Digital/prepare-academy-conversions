using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;

public class ReligiousBodyConsultationModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 601;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.ReligiousBodyConsultation.TrustConsultedReligiousBody = false;
      project.ReligiousBodyConsultation.TrustConsultedReligiousBodyNotConsultedReason = "Awaiting written response";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.TrustConsultedReligiousBody.Should().BeFalse();
      sut.TrustConsultedReligiousBodyNotConsultedReason.Should().Be("Awaiting written response");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYes_ShouldSaveAndRedirect()
   {
      const int id = 602;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = true;
      sut.TrustConsultedReligiousBodyNotConsultedReason = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetReligiousBodyConsultation(
         id,
         It.Is<SetSignificantChangeReligiousBodyConsultationCommand>(command =>
            command.TrustConsultedReligiousBody == true
            && command.TrustConsultedReligiousBodyNotConsultedReason == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithReason_ShouldSaveAndRedirect()
   {
      const int id = 603;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = false;
      sut.TrustConsultedReligiousBodyNotConsultedReason = "Trust will provide mitigation details after next meeting";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetReligiousBodyConsultation(
         id,
         It.Is<SetSignificantChangeReligiousBodyConsultationCommand>(command =>
            command.TrustConsultedReligiousBody == false
            && command.TrustConsultedReligiousBodyNotConsultedReason == "Trust will provide mitigation details after next meeting")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithoutReason_ShouldReturnPageWithValidationError()
   {
      const int id = 605;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = false;
      sut.TrustConsultedReligiousBodyNotConsultedReason = " ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.TrustConsultedReligiousBodyNotConsultedReason)).Should().BeTrue();
      repository.Verify(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 606;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.TrustConsultedReligiousBody)).Should().BeTrue();
      repository.Verify(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()), Times.Never);
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
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse(),
         ReligiousBodyConsultation = new SignificantChangeReligiousBodyConsultationResponse()
      };
   }
}
