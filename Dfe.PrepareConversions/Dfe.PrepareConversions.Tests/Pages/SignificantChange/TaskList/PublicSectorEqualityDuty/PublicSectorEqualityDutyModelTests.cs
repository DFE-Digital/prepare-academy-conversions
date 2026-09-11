using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.PublicSectorEqualityDuty;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.PublicSectorEqualityDuty;

public class PublicSectorEqualityDutyModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 601;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.EqualitiesImpactAssessment.EqualitiesImpactAssessmentCompleted = true;
      project.EqualitiesImpactAssessment.EqualitiesImpactIdentified = EqualitiesImpact.ImpactsIdentified;
      project.EqualitiesImpactAssessment.EqualitiesImpactIdentifiedMitigation = "Additional info required";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.EqualitiesImpactAssessmentCompleted.Should().BeTrue();
      sut.EqualitiesImpactIdentified.Should().Be(EqualitiesImpact.ImpactsIdentified);
      sut.EqualitiesImpactIdentifiedMitigation.Should().Be("Additional info required");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoImpactsIdentified_ShouldSaveAndRedirect()
   {
      const int id = 602;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetEqualitiesImpactAssessment(id, It.IsAny<SetSignificantChangeEqualitiesImpactAssessmentCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.EqualitiesImpactAssessmentCompleted = true;
      sut.EqualitiesImpactIdentified = EqualitiesImpact.None;
      sut.EqualitiesImpactIdentifiedMitigation = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetEqualitiesImpactAssessment(
         id,
         It.Is<SetSignificantChangeEqualitiesImpactAssessmentCommand>(command =>
            command.EqualitiesImpactAssessmentCompleted == true
            && command.EqualitiesImpactIdentified == EqualitiesImpact.None
            && command.EqualitiesImpactIdentifiedMitigation == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenImpactsIdentifiedWithGroups_ShouldSaveAndRedirect()
   {
      const int id = 603;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetEqualitiesImpactAssessment(id, It.IsAny<SetSignificantChangeEqualitiesImpactAssessmentCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.EqualitiesImpactAssessmentCompleted = true;
      sut.EqualitiesImpactIdentified = EqualitiesImpact.ImpactsIdentified;
      sut.EqualitiesImpactIdentifiedMitigation = "Pupils with SEND - additional transitional support planned";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetEqualitiesImpactAssessment(
         id,
         It.Is<SetSignificantChangeEqualitiesImpactAssessmentCommand>(command =>
            command.EqualitiesImpactAssessmentCompleted == true
            && command.EqualitiesImpactIdentified == EqualitiesImpact.ImpactsIdentified
            && command.EqualitiesImpactIdentifiedMitigation == "Pupils with SEND - additional transitional support planned")), Times.Once);
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
         EqualitiesImpactAssessment = new SignificantChangeEqualitiesImpactAssessmentResponse()
      };
   }
}
