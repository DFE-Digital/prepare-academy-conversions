using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ImpactOnSenProvision;
using Dfe.PrepareConversions.Services;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ImpactOnSenProvision;

public class ImpactOnSenProvisionModelTests
{
   private const int ProjectId = 612;
   private const string AnswerRequiredError = "Select whether the proposal is likely to improve the standard, quality and range of SEN provision";
   private readonly Mock<ISignificantChangeProjectRepository> _repository = new();

   [Fact]
   public async Task OnGet_PopulatesSavedValues()
   {
      var model = BuildModel(new()
      {
         Answer = YesNoNotApplicableAnswer.No,
         AdditionalInformation = "Provision reduced",
         SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/evidence"
      });

      Assert.IsType<PageResult>(await model.OnGetAsync(ProjectId));

      Assert.Equal(YesNoNotApplicableAnswer.No, model.Answer);
      Assert.Equal("Provision reduced", model.AdditionalInformation);
      Assert.Equal("https://educationgovuk.sharepoint.com/sites/evidence", model.SupportingEvidence);
   }

   [Theory]
   [InlineData(YesNoNotApplicableAnswer.Yes, null)]
   [InlineData(YesNoNotApplicableAnswer.NotApplicable, null)]
   [InlineData(YesNoNotApplicableAnswer.No, "Provision reduced")]
   public async Task OnPost_ValidAnswer_SavesAndRedirects(YesNoNotApplicableAnswer answer, string details)
   {
      var model = BuildModel();
      model.Answer = answer;
      model.AdditionalInformation = details;
      model.SupportingEvidence = "Evidence";
      _repository.Setup(x => x.SetImpactOnSenProvision(
         ProjectId, It.IsAny<SetSignificantChangeImpactOnSenProvisionCommand>()))
         .Returns(Task.CompletedTask);

      var result = Assert.IsType<RedirectToPageResult>(await model.OnPostAsync(ProjectId));

      Assert.Equal(Links.SignificantChange.SignificantChangeTaskList.Page, result.PageName);
      Assert.Equal(ProjectId, result.RouteValues["id"]);
      _repository.Verify(x => x.SetImpactOnSenProvision(ProjectId,
         new SetSignificantChangeImpactOnSenProvisionCommand(answer, details, "Evidence")), Times.Once);
   }

   [Theory]
   [InlineData(null, null, "Answer", AnswerRequiredError)]
   [InlineData(YesNoNotApplicableAnswer.No, null, "AdditionalInformation", "Enter details")]
   [InlineData(YesNoNotApplicableAnswer.No, " ", "AdditionalInformation", "Enter details")]
   public async Task OnPost_InvalidAnswer_ShowsExactErrorWithoutSaving(YesNoNotApplicableAnswer? answer, string details, string field, string error)
   {
      var model = BuildModel();
      model.Answer = answer;
      model.AdditionalInformation = details;

      Assert.IsType<PageResult>(await model.OnPostAsync(ProjectId));

      Assert.Equal(error, Assert.Single(model.ModelState[field].Errors).ErrorMessage);
      _repository.Verify(x => x.SetImpactOnSenProvision(
         It.IsAny<int>(), It.IsAny<SetSignificantChangeImpactOnSenProvisionCommand>()), Times.Never);
   }

   [Theory]
   [InlineData(false)]
   [InlineData(true)]
   public async Task MissingProject_ReturnsNotFound(bool post)
   {
      var model = BuildModel();
      _repository.Setup(x => x.GetProjectById(ProjectId))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.NotFound, null));

      Assert.IsType<NotFoundResult>(post ? await model.OnPostAsync(ProjectId) : await model.OnGetAsync(ProjectId));
   }

   private IndexModel BuildModel(SignificantChangeImpactOnSenProvisionResponse task = null)
   {
      SignificantChangeProjectResponse project = new()
      {
         Id = ProjectId,
         Urn = 123456,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Test trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority",
         ImpactOnSenProvision = task ?? new()
      };
      _repository.Setup(x => x.GetProjectById(ProjectId))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      ModelStateDictionary modelState = new();
      ActionContext actionContext = new(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), modelState);
      return new IndexModel(_repository.Object, new ErrorService())
      {
         PageContext = new PageContext(actionContext)
         {
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
         }
      };
   }
}
