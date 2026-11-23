using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
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
using IndexModel = Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Clearance.IndexModel;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Clearance;

public class ClearanceModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 621;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.Clearance.RegionsGroupClearance = "Regions Group clearer";
      project.Clearance.TrustRegulationClearance = "Trust Regulation clearer";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.RegionsGroupClearance.Should().Be("Regions Group clearer");
      sut.TrustRegulationClearance.Should().Be("Trust Regulation clearer");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenDataIsProvided_ShouldSaveAndRedirect()
   {
      const int id = 623;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetClearance(id, It.IsAny<SetSignificantChangeClearanceCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.RegionsGroupClearance = "Regions Group clearer";
      sut.TrustRegulationClearance = "Trust Regulation clearer";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetClearance(
         id,
         It.Is<SetSignificantChangeClearanceCommand>(command =>
            command.RegionsGroupClearance == "Regions Group clearer"
            && command.TrustRegulationClearance == "Trust Regulation clearer")), Times.Once);
   }

   [Theory]
   [InlineData("", "Trust Regulation clearer", "RegionsGroupClearance", "Enter who cleared the significant change in Regions Group")]
   [InlineData("   ", "Trust Regulation clearer", "RegionsGroupClearance", "Enter who cleared the significant change in Regions Group")]
   [InlineData("Regions Group clearer", "", "TrustRegulationClearance", "Enter who cleared the significant change from Trust Regulation (formerly known as SFSO)")]
   [InlineData("Regions Group clearer", " ", "TrustRegulationClearance", "Enter who cleared the significant change from Trust Regulation (formerly known as SFSO)")]
   public async Task OnPostAsync_WhenMandatoryFieldIsMissing_ShouldReturnPageWithErrorAndNotSave(
      string regionsGroupClearance,
      string trustRegulationClearance,
      string expectedErrorKey,
      string expectedErrorMessage)
   {
      const int id = 624;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.RegionsGroupClearance = regionsGroupClearance;
      sut.TrustRegulationClearance = trustRegulationClearance;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.IsValid.Should().BeFalse();
      sut.ModelState[expectedErrorKey].Errors.Should().ContainSingle(e => e.ErrorMessage == expectedErrorMessage);
      repository.Verify(x => x.SetClearance(It.IsAny<int>(), It.IsAny<SetSignificantChangeClearanceCommand>()), Times.Never);
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
         Recommendation = new SignificantChangeRecommendationResponse(),
         Clearance = new SignificantChangeClearanceResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}
