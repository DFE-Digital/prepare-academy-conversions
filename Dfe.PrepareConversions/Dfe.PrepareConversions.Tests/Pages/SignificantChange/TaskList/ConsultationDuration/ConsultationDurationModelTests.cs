using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ConsultationDuration;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ConsultationDuration;

public class ConsultationDurationModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 601;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.ConsultationDuration.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No;
      project.ConsultationDuration.ConsultationDurationNotMetReason = "Consultation ran for two weeks only";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ConsultationLastedMinimumThreeWeeks.Should().Be(ConsultationDurationAnswer.No);
      sut.ConsultationDurationNotMetReason.Should().Be("Consultation ran for two weeks only");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Theory]
   [InlineData(null)]
   [InlineData(false)]
   public async Task OnGetAsync_WhenStakeholderConsultationIsNotYes_ShouldRedirectToTaskList(bool? trustConsultedStakeholders)
   {
      const int id = 602;
      SignificantChangeProjectResponse project = BuildProject(id, trustConsultedStakeholders);

      IndexModel sut = BuildModel(BuildRepository(id, project).Object);

      IActionResult result = await sut.OnGetAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
   }

   [Theory]
   [InlineData(ConsultationDurationAnswer.Yes)]
   [InlineData(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut)]
   public async Task OnPostAsync_WhenAnswerIsNotNo_ShouldSaveWithoutReasonAndRedirect(ConsultationDurationAnswer answer)
   {
      const int id = 603;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetConsultationDuration(id, It.IsAny<SetSignificantChangeConsultationDurationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationLastedMinimumThreeWeeks = answer;
      sut.ConsultationDurationNotMetReason = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetConsultationDuration(
         id,
         It.Is<SetSignificantChangeConsultationDurationCommand>(command =>
            command.ConsultationLastedMinimumThreeWeeks == answer
            && command.ConsultationDurationNotMetReason == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithReason_ShouldSaveAndRedirect()
   {
      const int id = 604;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetConsultationDuration(id, It.IsAny<SetSignificantChangeConsultationDurationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No;
      sut.ConsultationDurationNotMetReason = "Consultation ran for two weeks only";

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);

      repository.Verify(x => x.SetConsultationDuration(
         id,
         It.Is<SetSignificantChangeConsultationDurationCommand>(command =>
            command.ConsultationLastedMinimumThreeWeeks == ConsultationDurationAnswer.No
            && command.ConsultationDurationNotMetReason == "Consultation ran for two weeks only")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithoutReason_ShouldReturnPageWithValidationError()
   {
      const int id = 605;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No;
      sut.ConsultationDurationNotMetReason = " ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.ConsultationDurationNotMetReason)).Should().BeTrue();
      repository.Verify(x => x.SetConsultationDuration(id, It.IsAny<SetSignificantChangeConsultationDurationCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 606;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationLastedMinimumThreeWeeks = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.ConsultationLastedMinimumThreeWeeks)).Should().BeTrue();
      repository.Verify(x => x.SetConsultationDuration(id, It.IsAny<SetSignificantChangeConsultationDurationCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenStakeholderConsultationIsNotYes_ShouldRedirectWithoutSaving()
   {
      const int id = 607;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id, trustConsultedStakeholders: false));

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.Yes;

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);
      repository.Verify(x => x.SetConsultationDuration(id, It.IsAny<SetSignificantChangeConsultationDurationCommand>()), Times.Never);
   }

   private static Mock<ISignificantChangeProjectRepository> BuildRepository(int id, SignificantChangeProjectResponse project)
   {
      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      return repository;
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

   private static SignificantChangeProjectResponse BuildProject(int id, bool? trustConsultedStakeholders = true)
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
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse
         {
            TrustConsultedStakeholders = trustConsultedStakeholders
         },
         ConsultationDuration = new SignificantChangeConsultationDurationResponse()
      };
   }
}