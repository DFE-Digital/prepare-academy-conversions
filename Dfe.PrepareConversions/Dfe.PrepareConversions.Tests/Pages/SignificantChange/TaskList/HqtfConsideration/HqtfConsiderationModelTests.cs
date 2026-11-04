using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.HqtfConsideration;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.HqtfConsideration;

public class HqtfConsiderationModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 901;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.HQTFConsideration.HqtfConsiderationAnswer = HqtfConsiderationAnswer.Yes;
      project.HQTFConsideration.AdditionalInformation = "Governance gap identified";
      project.HQTFConsideration.SupportingEvidence = "Trust improvement plan";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.HQTFConsiderationAnswer.Should().Be(HqtfConsiderationAnswer.Yes);
      sut.AdditionalInformation.Should().Be("Governance gap identified");
      sut.SupportingEvidence.Should().Be("Trust improvement plan");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYesWithAdditionalInformation_ShouldSaveAndRedirect()
   {
      const int id = 902;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetHQTFConsideration(id, It.IsAny<SetSignificantChangeHqtfConsiderationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.HQTFConsiderationAnswer = HqtfConsiderationAnswer.Yes;
      sut.AdditionalInformation = "Workforce capacity risk";
      sut.SupportingEvidence = "Board minutes";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetHQTFConsideration(
         id,
         It.Is<SetSignificantChangeHqtfConsiderationCommand>(command =>
            command.HqtfConsiderationAnswer == HqtfConsiderationAnswer.Yes
            && command.AdditionalInformation == "Workforce capacity risk"
            && command.SupportingEvidence == "Board minutes")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNo_ShouldSaveAndClearAdditionalInformation()
   {
      const int id = 903;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetHQTFConsideration(id, It.IsAny<SetSignificantChangeHqtfConsiderationCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.HQTFConsiderationAnswer = HqtfConsiderationAnswer.No;
      sut.AdditionalInformation = "This should be cleared";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<RedirectToPageResult>();
      repository.Verify(x => x.SetHQTFConsideration(
         id,
         It.Is<SetSignificantChangeHqtfConsiderationCommand>(command =>
            command.HqtfConsiderationAnswer == HqtfConsiderationAnswer.No
            && command.AdditionalInformation == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYesWithoutAdditionalInformation_ShouldReturnPageWithValidationError()
   {
      const int id = 904;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.HQTFConsiderationAnswer = HqtfConsiderationAnswer.Yes;
      sut.AdditionalInformation = "   ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.AdditionalInformation)).Should().BeTrue();
      repository.Verify(x => x.SetHQTFConsideration(id, It.IsAny<SetSignificantChangeHqtfConsiderationCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 905;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.HQTFConsiderationAnswer = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.HQTFConsiderationAnswer)).Should().BeTrue();
      repository.Verify(x => x.SetHQTFConsideration(id, It.IsAny<SetSignificantChangeHqtfConsiderationCommand>()), Times.Never);
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
         HQTFConsideration = new SignificantChangeHqtfConsiderationResponse()
      };
   }
}
