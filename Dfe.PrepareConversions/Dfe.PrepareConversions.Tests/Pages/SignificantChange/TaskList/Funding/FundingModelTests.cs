using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Funding;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Funding;

public class FundingModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 611;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.Funding.FundingAnswer = FundingAnswer.No;
      project.Funding.AdditionalInformation = "Funding gap identified";
      project.Funding.SupportingEvidence = "Board minutes link";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.FundingAnswer.Should().Be(FundingAnswer.No);
      sut.AdditionalInformation.Should().Be("Funding gap identified");
      sut.SupportingEvidence.Should().Be("Board minutes link");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Theory]
   [InlineData(FundingAnswer.Yes)]
   [InlineData(FundingAnswer.NotApplicable)]
   public async Task OnPostAsync_WhenAnswerIsNotNo_ShouldSaveWithoutAdditionalInformationAndRedirect(FundingAnswer answer)
   {
      const int id = 613;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetFunding(id, It.IsAny<SetSignificantChangeFundingCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.FundingAnswer = answer;
      sut.AdditionalInformation = "This should be cleared";
      sut.SupportingEvidence = "Supporting evidence";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetFunding(
         id,
         It.Is<SetSignificantChangeFundingCommand>(command =>
            command.FundingAnswer == answer
            && command.AdditionalInformation == null
            && command.SupportingEvidence == "Supporting evidence")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithAdditionalInformation_ShouldSaveAndRedirect()
   {
      const int id = 614;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetFunding(id, It.IsAny<SetSignificantChangeFundingCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.FundingAnswer = FundingAnswer.No;
      sut.AdditionalInformation = "Funding gap identified";
      sut.SupportingEvidence = "Board minutes link";

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);

      repository.Verify(x => x.SetFunding(
         id,
         It.Is<SetSignificantChangeFundingCommand>(command =>
            command.FundingAnswer == FundingAnswer.No
            && command.AdditionalInformation == "Funding gap identified"
            && command.SupportingEvidence == "Board minutes link")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithoutAdditionalInformation_ShouldReturnPageWithValidationError()
   {
      const int id = 615;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.FundingAnswer = FundingAnswer.No;
      sut.AdditionalInformation = " ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.AdditionalInformation)).Should().BeTrue();
      repository.Verify(x => x.SetFunding(id, It.IsAny<SetSignificantChangeFundingCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 616;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.FundingAnswer = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.FundingAnswer)).Should().BeTrue();
      repository.Verify(x => x.SetFunding(id, It.IsAny<SetSignificantChangeFundingCommand>()), Times.Never);
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
         Funding = new SignificantChangeFundingResponse()
      };
   }
}
