using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.MainstreamSchoolSatelliteConsiderations;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.MainstreamSchoolSatelliteConsiderations;

public class MainstreamSchoolSatelliteConsiderationsModelTests
{
   private const int ProjectId = 611;
   private readonly Mock<ISignificantChangeProjectRepository> _repository = new();

   [Fact]
   public async Task OnGet_PopulatesSavedValues()
   {
      var model = BuildModel(new()
      {
         Answer = YesNoNotApplicableAnswer.No,
         AdditionalInformation = "Requirements gap",
         SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/evidence"
      });

      Assert.IsType<PageResult>(await model.OnGetAsync(ProjectId));

      Assert.Equal(YesNoNotApplicableAnswer.No, model.Answer);
      Assert.Equal("Requirements gap", model.AdditionalInformation);
      Assert.Equal("https://educationgovuk.sharepoint.com/sites/evidence", model.SupportingEvidence);
   }

   [Theory]
   [InlineData(YesNoNotApplicableAnswer.Yes, null)]
   [InlineData(YesNoNotApplicableAnswer.NotApplicable, null)]
   [InlineData(YesNoNotApplicableAnswer.No, "Requirements gap")]
   public async Task OnPost_ValidAnswer_SavesAndRedirects(YesNoNotApplicableAnswer answer, string details)
   {
      var model = BuildModel();
      model.Answer = answer;
      model.AdditionalInformation = details;
      model.SupportingEvidence = "Evidence";
      _repository.Setup(x => x.SetMainstreamSchoolSatelliteConsiderations(
         ProjectId, It.IsAny<SetSignificantChangeMainstreamSchoolSatelliteConsiderationsCommand>()))
         .Returns(Task.CompletedTask);

      var result = Assert.IsType<RedirectToPageResult>(await model.OnPostAsync(ProjectId));

      Assert.Equal(Links.SignificantChange.SignificantChangeTaskList.Page, result.PageName);
      Assert.Equal(ProjectId, result.RouteValues["id"]);
      _repository.Verify(x => x.SetMainstreamSchoolSatelliteConsiderations(ProjectId,
         new SetSignificantChangeMainstreamSchoolSatelliteConsiderationsCommand(answer, details, "Evidence")), Times.Once);
   }

   [Theory]
   [InlineData(null, null, "Answer", "Select whether the proposal meets the requirements set out for special school provision on a satellite site in the operational guidance")]
   [InlineData(YesNoNotApplicableAnswer.No, null, "AdditionalInformation", "Enter details")]
   [InlineData(YesNoNotApplicableAnswer.No, " ", "AdditionalInformation", "Enter details")]
   public async Task OnPost_InvalidAnswer_ShowsExactErrorWithoutSaving(YesNoNotApplicableAnswer? answer, string details, string field, string error)
   {
      var model = BuildModel();
      model.Answer = answer;
      model.AdditionalInformation = details;

      Assert.IsType<PageResult>(await model.OnPostAsync(ProjectId));

      Assert.Equal(error, Assert.Single(model.ModelState[field].Errors).ErrorMessage);
      _repository.Verify(x => x.SetMainstreamSchoolSatelliteConsiderations(
         It.IsAny<int>(), It.IsAny<SetSignificantChangeMainstreamSchoolSatelliteConsiderationsCommand>()), Times.Never);
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

   private IndexModel BuildModel(SignificantChangeMainstreamSchoolSatelliteConsiderationsResponse task = null)
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
         MainstreamSchoolSatelliteConsiderations = task ?? new()
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
