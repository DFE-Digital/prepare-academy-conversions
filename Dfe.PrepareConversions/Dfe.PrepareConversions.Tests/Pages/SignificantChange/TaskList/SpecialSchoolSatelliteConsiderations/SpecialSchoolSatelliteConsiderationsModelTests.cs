using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.SpecialSchoolSatelliteConsiderations;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.SpecialSchoolSatelliteConsiderations;

public class SpecialSchoolSatelliteConsiderationsModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 611;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.SpecialSchoolSatelliteConsiderations = new SignificantChangeSpecialSchoolSatelliteConsiderationsResponse
      {
         Answer = YesNoNotApplicableAnswer.No,
         AdditionalInformation = "Satellite site requirements still need to be met",
         SupportingEvidence = "https://example.test/evidence"
      };

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.Answer.Should().Be(YesNoNotApplicableAnswer.No);
      sut.AdditionalInformation.Should().Be("Satellite site requirements still need to be met");
      sut.SupportingEvidence.Should().Be("https://example.test/evidence");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Theory]
   [InlineData(YesNoNotApplicableAnswer.Yes)]
   [InlineData(YesNoNotApplicableAnswer.NotApplicable)]
   public async Task OnPostAsync_WhenAnswerDoesNotRequireAdditionalInformation_ShouldSaveAndRedirect(
      YesNoNotApplicableAnswer answer)
   {
      const int id = 612;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetSpecialSchoolSatelliteConsiderations(id, It.IsAny<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.Answer = answer;
      sut.AdditionalInformation = string.Empty;
      sut.SupportingEvidence = "https://example.test/evidence";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);
      repository.Verify(x => x.SetSpecialSchoolSatelliteConsiderations(
         id,
         It.Is<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand>(command =>
            command.Answer == answer
            && command.AdditionalInformation == string.Empty
            && command.SupportingEvidence == "https://example.test/evidence")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithAdditionalInformation_ShouldSaveAndRedirect()
   {
      const int id = 613;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetSpecialSchoolSatelliteConsiderations(id, It.IsAny<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.Answer = YesNoNotApplicableAnswer.No;
      sut.AdditionalInformation = "Requirements gap identified";

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);
      repository.Verify(x => x.SetSpecialSchoolSatelliteConsiderations(
         id,
         It.Is<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand>(command =>
            command.Answer == YesNoNotApplicableAnswer.No
            && command.AdditionalInformation == "Requirements gap identified")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsNoWithoutAdditionalInformation_ShouldReturnPageWithValidationError()
   {
      const int id = 614;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.Answer = YesNoNotApplicableAnswer.No;
      sut.AdditionalInformation = " ";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.AdditionalInformation)).Should().BeTrue();
      repository.Verify(x => x.SetSpecialSchoolSatelliteConsiderations(
         id, It.IsAny<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoAnswerIsSelected_ShouldReturnPageWithAnswerValidationError()
   {
      const int id = 615;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.Answer)).Should().BeTrue();
      repository.Verify(x => x.SetSpecialSchoolSatelliteConsiderations(
         id, It.IsAny<SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand>()), Times.Never);
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
         LocalAuthorityName = "Test local authority"
      };
   }
}