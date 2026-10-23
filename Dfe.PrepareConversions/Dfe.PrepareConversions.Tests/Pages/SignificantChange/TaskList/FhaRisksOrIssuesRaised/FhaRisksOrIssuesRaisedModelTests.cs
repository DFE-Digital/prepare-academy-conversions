using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.FhaRisksOrIssuesRaised;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.FhaRisksOrIssuesRaised;

public class FhaRisksOrIssuesRaisedModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 921;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.FHARisksOrIssuesRaised.FhaRisksOrIssuesRaised = true;
      project.FHARisksOrIssuesRaised.AdditionalInformation = "Cashflow risk identified";
      project.FHARisksOrIssuesRaised.SupportingEvidence = "FHA report";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.FhaRisksOrIssuesRaised.Should().BeTrue();
      sut.AdditionalInformation.Should().Be("Cashflow risk identified");
      sut.SupportingEvidence.Should().Be("FHA report");
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYesWithAdditionalInformation_ShouldSaveAndRedirect()
   {
      const int id = 922;
      Mock<ISignificantChangeProjectRepository> repository = CreateRepository(id);
      repository
         .Setup(x => x.SetFhaRisksOrIssuesRaised(id, It.IsAny<SetSignificantChangeFhaRisksOrIssuesRaisedCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.FhaRisksOrIssuesRaised = true;
      sut.AdditionalInformation = "A deficit is forecast";
      sut.SupportingEvidence = "Financial forecast";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      repository.Verify(x => x.SetFhaRisksOrIssuesRaised(
         id,
         It.Is<SetSignificantChangeFhaRisksOrIssuesRaisedCommand>(command =>
            command.FhaRisksOrIssuesRaised == true
            && command.AdditionalInformation == "A deficit is forecast"
            && command.SupportingEvidence == "Financial forecast")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNo_ShouldClearAdditionalInformation()
   {
      const int id = 923;
      Mock<ISignificantChangeProjectRepository> repository = CreateRepository(id);
      repository
         .Setup(x => x.SetFhaRisksOrIssuesRaised(id, It.IsAny<SetSignificantChangeFhaRisksOrIssuesRaisedCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.FhaRisksOrIssuesRaised = false;
      sut.AdditionalInformation = "Clear this";

      await sut.OnPostAsync(id);

      repository.Verify(x => x.SetFhaRisksOrIssuesRaised(
         id,
         It.Is<SetSignificantChangeFhaRisksOrIssuesRaisedCommand>(command =>
            command.FhaRisksOrIssuesRaised == false
            && command.AdditionalInformation == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenYesHasNoAdditionalInformation_ShouldReturnPageWithValidationError()
   {
      const int id = 924;
      Mock<ISignificantChangeProjectRepository> repository = CreateRepository(id);

      IndexModel sut = BuildModel(repository.Object);
      sut.FhaRisksOrIssuesRaised = true;
      sut.AdditionalInformation = " ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.AdditionalInformation)).Should().BeTrue();
      repository.Verify(x => x.SetFhaRisksOrIssuesRaised(id, It.IsAny<SetSignificantChangeFhaRisksOrIssuesRaisedCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoOptionIsSelected_ShouldReturnPageWithValidationError()
   {
      const int id = 925;
      Mock<ISignificantChangeProjectRepository> repository = CreateRepository(id);

      IndexModel sut = BuildModel(repository.Object);
      sut.FhaRisksOrIssuesRaised = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.FhaRisksOrIssuesRaised)).Should().BeTrue();
      repository.Verify(x => x.SetFhaRisksOrIssuesRaised(id, It.IsAny<SetSignificantChangeFhaRisksOrIssuesRaisedCommand>()), Times.Never);
   }

   private static Mock<ISignificantChangeProjectRepository> CreateRepository(int id)
   {
      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
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
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         FHARisksOrIssuesRaised = new SignificantChangeFhaRisksOrIssuesRaisedResponse()
      };
   }
}
