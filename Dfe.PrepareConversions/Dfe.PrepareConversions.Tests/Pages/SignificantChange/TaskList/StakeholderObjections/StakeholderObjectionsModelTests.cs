using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.StakeholderObjections;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.StakeholderObjections;

public class StakeholderObjectionsModelTests
{
    [Fact]
    public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
    {
        const int id = 401;
        const string comment = "Objections were raised but resolved";

        SignificantChangeProjectResponse project = BuildProject(id);

        project.StakeholderObjections = new SignificantChangeStakeholderObjectionsResponse
        {
            StakeholderObjections = SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided,
            StakeholderObjectionsComment = comment
        };

        Mock<ISignificantChangeProjectRepository> repository = new();
        repository
            .Setup(x => x.GetProjectById(id))
            .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

        IndexModel sut = BuildModel(repository.Object);

        IActionResult result = await sut.OnGetAsync(id);

        repository.Verify(x => x.GetProjectById(id), Times.Once);
        result.Should().BeOfType<PageResult>();
        sut.StakeholderObjections.Should().Be(SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided);
        sut.StakeholderObjectionsComment.Should().Be(comment);
    }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 405;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.StakeholderObjections = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.StakeholderObjections)).Should().BeTrue();
      repository.Verify(x => x.SetStakeholderObjections(id, It.IsAny<SetSignificantChangeStakeholderObjectionsCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYesNoFurtherInformationProvidedWithoutComment_ShouldReturnPageWithValidationError()
   {
      const int id = 406;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.StakeholderObjections = SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided;
      sut.StakeholderObjectionsComment = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.StakeholderObjectionsComment)).Should().BeTrue();
      repository.Verify(x => x.SetStakeholderObjections(id, It.IsAny<SetSignificantChangeStakeholderObjectionsCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswerIsYesAndCommentProvided_ShouldSaveAndRedirect()
   {
      const int id = 407;
      const string comment = "Additional information was supplied";

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetStakeholderObjections(id, It.IsAny<SetSignificantChangeStakeholderObjectionsCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.StakeholderObjections = SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided;
      sut.StakeholderObjectionsComment = comment;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetStakeholderObjections(
            id,
            It.Is<SetSignificantChangeStakeholderObjectionsCommand>(command =>
               command.StakeholderObjections == SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided
               && command.StakeholderObjectionsComment == comment)), Times.Once);
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
         StakeholderObjections = new SignificantChangeStakeholderObjectionsResponse()
      };
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
}