using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.LandTransaction;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.LandTransaction;

public class LandTransactionModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 801;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.LandTransaction.LandTransactionApplication = SignificantChange_Generic_YesNoNa.No;
      project.LandTransaction.LandTransactionApplicationAdditionalInfo = "Application details";
      project.LandTransaction.LandTransactionConsent = SignificantChange_Generic_YesNoNa.Yes;
      project.LandTransaction.LandTransactionConsentAdditionalInfo = "Consent details";
      project.LandTransaction.LandTransactionSupportingEvidence = "Evidence link";

      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.LandTransactionApplication.Should().Be(SignificantChange_Generic_YesNoNa.No);
      sut.LandTransactionApplicationAdditionalInfo.Should().Be("Application details");
      sut.LandTransactionConsent.Should().Be(SignificantChange_Generic_YesNoNa.Yes);
      sut.LandTransactionConsentAdditionalInfo.Should().Be("Consent details");
      sut.LandTransactionSupportingEvidence.Should().Be("Evidence link");
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswersAreNoWithDetails_ShouldSaveAndRedirect()
   {
      const int id = 802;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetLandTransaction(id, It.IsAny<SetSignificantChangeLandTransactionCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.LandTransactionApplication = SignificantChange_Generic_YesNoNa.No;
      sut.LandTransactionApplicationAdditionalInfo = "Application details";
      sut.LandTransactionConsent = SignificantChange_Generic_YesNoNa.No;
      sut.LandTransactionConsentAdditionalInfo = "Consent details";
      sut.LandTransactionSupportingEvidence = "Evidence link";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);
      repository.Verify(x => x.SetLandTransaction(
         id,
         It.Is<SetSignificantChangeLandTransactionCommand>(command =>
            command.LandTransactionApplication == SignificantChange_Generic_YesNoNa.No
            && command.LandTransactionApplicationAdditionalInfo == "Application details"
            && command.LandTransactionConsent == SignificantChange_Generic_YesNoNa.No
            && command.LandTransactionConsentAdditionalInfo == "Consent details"
            && command.LandTransactionSupportingEvidence == "Evidence link")), Times.Once);
   }

   [Theory]
   [InlineData(SignificantChange_Generic_YesNoNa.Yes)]
   [InlineData(SignificantChange_Generic_YesNoNa.NotApplicable)]
   public async Task OnPostAsync_WhenAnswerIsNotNo_ShouldClearThatAnswerDetails(
      SignificantChange_Generic_YesNoNa answer)
   {
      const int id = 803;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetLandTransaction(id, It.IsAny<SetSignificantChangeLandTransactionCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.LandTransactionApplication = answer;
      sut.LandTransactionApplicationAdditionalInfo = "Application details to clear";
      sut.LandTransactionConsent = answer;
      sut.LandTransactionConsentAdditionalInfo = "Consent details to clear";

      await sut.OnPostAsync(id);

      repository.Verify(x => x.SetLandTransaction(
         id,
         It.Is<SetSignificantChangeLandTransactionCommand>(command =>
            command.LandTransactionApplication == answer
            && command.LandTransactionApplicationAdditionalInfo == null
            && command.LandTransactionConsent == answer
            && command.LandTransactionConsentAdditionalInfo == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenAnswersAreMissing_ShouldReturnPageWithValidationErrors()
   {
      const int id = 804;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.LandTransactionApplication)).Should().BeTrue();
      sut.ModelState.ContainsKey(nameof(IndexModel.LandTransactionConsent)).Should().BeTrue();
      repository.Verify(x => x.SetLandTransaction(id, It.IsAny<SetSignificantChangeLandTransactionCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_WhenNoAnswersHaveNoDetails_ShouldReturnPageWithValidationErrors()
   {
      const int id = 805;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.LandTransactionApplication = SignificantChange_Generic_YesNoNa.No;
      sut.LandTransactionApplicationAdditionalInfo = " ";
      sut.LandTransactionConsent = SignificantChange_Generic_YesNoNa.No;
      sut.LandTransactionConsentAdditionalInfo = "";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.LandTransactionApplicationAdditionalInfo)).Should().BeTrue();
      sut.ModelState.ContainsKey(nameof(IndexModel.LandTransactionConsentAdditionalInfo)).Should().BeTrue();
      repository.Verify(x => x.SetLandTransaction(id, It.IsAny<SetSignificantChangeLandTransactionCommand>()), Times.Never);
   }

   private static Mock<ISignificantChangeProjectRepository> BuildRepository(
      int id,
      SignificantChangeProjectResponse project)
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
         LandTransaction = new SignificantChangeLandTransactionResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}