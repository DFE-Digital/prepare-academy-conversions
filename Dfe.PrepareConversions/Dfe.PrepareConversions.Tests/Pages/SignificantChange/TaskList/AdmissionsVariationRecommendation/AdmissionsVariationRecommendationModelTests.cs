using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.AdmissionsVariationRecommendation;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.AdmissionsVariationRecommendation;

public class AdmissionsVariationRecommendationModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 811;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.AdmissionsVariationRecommendation.AdmissionsVariationRecommendationAnswer = AdmissionsVariationRecommendationAnswer.Defer;
      project.AdmissionsVariationRecommendation.FurtherInformation = "Awaiting trust response";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.AdmissionsVariationRecommendationAnswer.Should().Be(AdmissionsVariationRecommendationAnswer.Defer);
      sut.FurtherInformation.Should().Be("Awaiting trust response");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenValid_ShouldSaveAndRedirect()
   {
      const int id = 812;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetAdmissionsVariationRecommendation(id, It.IsAny<SetAdmissionsVariationRecommendationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.AdmissionsVariationRecommendationAnswer = AdmissionsVariationRecommendationAnswer.Approve;
      sut.FurtherInformation = "Subject to funding confirmation";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetAdmissionsVariationRecommendation(
         id,
         It.Is<SetAdmissionsVariationRecommendationCommand>(command =>
            command.AdmissionsVariationRecommendationAnswer == AdmissionsVariationRecommendationAnswer.Approve
            && command.FurtherInformation == "Subject to funding confirmation")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 813;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.AdmissionsVariationRecommendationAnswer = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.AdmissionsVariationRecommendationAnswer)).Should().BeTrue();
      repository.Verify(x => x.SetAdmissionsVariationRecommendation(id, It.IsAny<SetAdmissionsVariationRecommendationCommand>()), Times.Never);
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
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         AdmissionsVariationRecommendation = new SignificantChangeAdmissionsVariationRecommendationResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}
