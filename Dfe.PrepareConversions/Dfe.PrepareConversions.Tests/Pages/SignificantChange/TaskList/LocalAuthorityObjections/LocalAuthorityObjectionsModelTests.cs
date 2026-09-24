using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.LocalAuthorityObjections;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.LocalAuthorityObjections;

public class LocalAuthorityObjectionsModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 601;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.LocalAuthorityObjections.LocalAuthorityRaisedObjections = true;
      project.LocalAuthorityObjections.LocalAuthorityObjectionsFurtherInformation = "Objections have been raised about capacity impact";
      project.LocalAuthorityObjections.SupportingEvidenceLink = "https://example.org/evidence";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.LocalAuthorityRaisedObjections.Should().BeTrue();
      sut.LocalAuthorityObjectionsFurtherInformation.Should().Be("Objections have been raised about capacity impact");
      sut.LocalAuthorityObjectionsSupportingEvidenceLink.Should().Be("https://example.org/evidence");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Theory]
   [InlineData(602, true, "Objection details", "Objection details")]
   [InlineData(603, false, "This should be cleared", null)]
   public async Task OnPostAsync_WhenAnswerIsProvided_ShouldSaveAndRedirect(
      int id,
      bool localAuthorityRaisedObjections,
      string enteredFurtherInformation,
      string expectedFurtherInformation)
   {
      const string expectedEvidenceLink = "https://example.org/evidence";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetLocalAuthorityObjections(id, It.IsAny<SetSignificantChangeLocalAuthorityObjectionsCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.LocalAuthorityRaisedObjections = localAuthorityRaisedObjections;
      sut.LocalAuthorityObjectionsFurtherInformation = enteredFurtherInformation;
      sut.LocalAuthorityObjectionsSupportingEvidenceLink = expectedEvidenceLink;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetLocalAuthorityObjections(
         id,
         It.Is<SetSignificantChangeLocalAuthorityObjectionsCommand>(command =>
            command.LocalAuthorityRaisedObjections == localAuthorityRaisedObjections
            && command.LocalAuthorityObjectionsFurtherInformation == expectedFurtherInformation
            && command.LocalAuthoritySupportingEvidenceLink == expectedEvidenceLink)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoSelectionIsMade_ShouldReturnPageWithValidationError()
   {
      const int id = 604;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.LocalAuthorityRaisedObjections = null;
      sut.LocalAuthorityObjectionsFurtherInformation = "Objection details";
      sut.LocalAuthorityObjectionsSupportingEvidenceLink = "https://example.org/evidence";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.LocalAuthorityRaisedObjections)).Should().BeTrue();
      repository.Verify(x => x.SetLocalAuthorityObjections(id, It.IsAny<SetSignificantChangeLocalAuthorityObjectionsCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenYesIsSelectedWithoutFurtherInformation_ShouldReturnPageWithValidationError()
   {
      const int id = 606;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));

      IndexModel sut = BuildModel(repository.Object);
      sut.LocalAuthorityRaisedObjections = true;
      sut.LocalAuthorityObjectionsFurtherInformation = " ";
      sut.LocalAuthorityObjectionsSupportingEvidenceLink = "https://example.org/evidence";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.LocalAuthorityObjectionsFurtherInformation)).Should().BeTrue();
      repository.Verify(x => x.SetLocalAuthorityObjections(id, It.IsAny<SetSignificantChangeLocalAuthorityObjectionsCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenEvidenceLinkIsBlank_ShouldSaveNullEvidenceLink()
   {
      const int id = 605;

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetLocalAuthorityObjections(id, It.IsAny<SetSignificantChangeLocalAuthorityObjectionsCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.LocalAuthorityRaisedObjections = true;
      sut.LocalAuthorityObjectionsFurtherInformation = "Objection details";
      sut.LocalAuthorityObjectionsSupportingEvidenceLink = " ";

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);

      repository.Verify(x => x.SetLocalAuthorityObjections(
         id,
         It.Is<SetSignificantChangeLocalAuthorityObjectionsCommand>(command =>
            command.LocalAuthorityRaisedObjections.HasValue
            && command.LocalAuthorityRaisedObjections.Value
            && command.LocalAuthorityObjectionsFurtherInformation == "Objection details"
            && command.LocalAuthoritySupportingEvidenceLink == null)), Times.Once);
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
         LocalAuthorityObjections = new SignificantChangeLocalAuthorityObjectionsResponse()
      };
   }
}
