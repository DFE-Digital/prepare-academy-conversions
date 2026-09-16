using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.AdmissionVariationConsultation;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.AdmissionVariationConsultation;

public class AdmissionVariationConsultationModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 411;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.AdmissionVariationConsultation.ConsultationIncludeAdmissionVariation = false;
      project.AdmissionVariationConsultation.ConsultationNoAdmissionVariationReason = "Consultation focused on catchment options";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ConsultationIncludeAdmissionVariation.Should().BeFalse();
      sut.ConsultationNoAdmissionVariationReason.Should().Be("Consultation focused on catchment options");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYes_ShouldSaveAndRedirect()
   {
      const int id = 412;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetAdmissionVariationConsultation(id, It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationIncludeAdmissionVariation = true;
      sut.ConsultationNoAdmissionVariationReason = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetAdmissionVariationConsultation(
         id,
         It.Is<SetSignificantChangeAdmissionVariationConsultationCommand>(command =>
            command.ConsultationIncludeAdmissionVariation == true
            && command.NoAdmissionVariationReason == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithReason_ShouldSaveAndRedirect()
   {
      const int id = 413;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetAdmissionVariationConsultation(id, It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationIncludeAdmissionVariation = false;
      sut.ConsultationNoAdmissionVariationReason = "Consultation did not include admission changes";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetAdmissionVariationConsultation(
         id,
         It.Is<SetSignificantChangeAdmissionVariationConsultationCommand>(command =>
            command.ConsultationIncludeAdmissionVariation == false
            && command.NoAdmissionVariationReason == "Consultation did not include admission changes")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithoutReason_ShouldReturnPageWithValidationError()
   {
      const int id = 414;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationIncludeAdmissionVariation = false;
      sut.ConsultationNoAdmissionVariationReason = "  ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.ConsultationNoAdmissionVariationReason)).Should().BeTrue();
      repository.Verify(x => x.SetAdmissionVariationConsultation(id, It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 415;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.ConsultationIncludeAdmissionVariation = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.ConsultationIncludeAdmissionVariation)).Should().BeTrue();
      repository.Verify(x => x.SetAdmissionVariationConsultation(id, It.IsAny<SetSignificantChangeAdmissionVariationConsultationCommand>()), Times.Never);
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
         AdmissionVariationConsultation = new SignificantChangeAdmissionVariationConsultationResponse()
      };
   }
}
