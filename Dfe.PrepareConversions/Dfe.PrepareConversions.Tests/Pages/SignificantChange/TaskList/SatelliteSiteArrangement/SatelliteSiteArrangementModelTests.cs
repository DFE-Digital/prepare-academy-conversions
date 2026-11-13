using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.SatelliteSiteArrangement;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.SatelliteSiteArrangement;

public class SatelliteSiteArrangementModelTests
{
   private const int ProjectId = 611;
   private readonly Mock<ISignificantChangeProjectRepository> _repository = new();

   [Fact]
   public async Task OnGet_PopulatesExistingAssessment()
   {
      IndexModel model = BuildModel(new SignificantChangeSatelliteSiteArrangementResponse
      {
         Answer = YesNoNotApplicableAnswer.No,
         AdditionalInformation = "Not a genuine expansion",
         SupportingEvidence = "https://example.test/evidence",
         Status = SignificantChangeTaskStatus.Completed
      });

      Assert.IsType<PageResult>(await model.OnGetAsync(ProjectId));
      Assert.Equal(YesNoNotApplicableAnswer.No, model.Answer);
      Assert.Equal("Not a genuine expansion", model.AdditionalInformation);
      Assert.Equal("https://example.test/evidence", model.SupportingEvidence);
      Assert.Equal("Satellite site arrangement", model.ViewData["TaskTitle"]);
   }

   [Theory]
   [InlineData(null, null, "Answer", "Select an option")]
   [InlineData(YesNoNotApplicableAnswer.No, null, "AdditionalInformation", "Add additional information")]
   [InlineData(YesNoNotApplicableAnswer.No, " ", "AdditionalInformation", "Add additional information")]
   public async Task OnPost_InvalidAssessment_DoesNotSave(
      YesNoNotApplicableAnswer? answer, string information, string field, string message)
   {
      IndexModel model = BuildModel();
      model.Answer = answer;
      model.AdditionalInformation = information;

      Assert.IsType<PageResult>(await model.OnPostAsync(ProjectId));
      Assert.Equal(message, Assert.Single(model.ModelState[field].Errors).ErrorMessage);
      _repository.Verify(x => x.SetSatelliteSiteArrangement(
         It.IsAny<int>(), It.IsAny<SetSignificantChangeSatelliteSiteArrangementCommand>()), Times.Never);
   }

   [Theory]
   [InlineData(YesNoNotApplicableAnswer.Yes, null)]
   [InlineData(YesNoNotApplicableAnswer.NotApplicable, null)]
   [InlineData(YesNoNotApplicableAnswer.No, "Mitigation")]
   public async Task OnPost_ValidAssessment_SavesAndReturnsToTaskList(
      YesNoNotApplicableAnswer answer, string information)
   {
      IndexModel model = BuildModel();
      model.Answer = answer;
      model.AdditionalInformation = information;
      model.SupportingEvidence = "Evidence";
      _repository.Setup(x => x.SetSatelliteSiteArrangement(ProjectId, It.IsAny<SetSignificantChangeSatelliteSiteArrangementCommand>()))
         .Returns(Task.CompletedTask);

      var redirect = Assert.IsType<RedirectToPageResult>(await model.OnPostAsync(ProjectId));

      Assert.Equal(Links.SignificantChange.SignificantChangeTaskList.Page, redirect.PageName);
      Assert.Equal(ProjectId, redirect.RouteValues["id"]);
      _repository.Verify(x => x.SetSatelliteSiteArrangement(ProjectId,
         It.Is<SetSignificantChangeSatelliteSiteArrangementCommand>(command =>
            command.Answer == answer && command.AdditionalInformation == information && command.SupportingEvidence == "Evidence")), Times.Once);
   }

   [Theory]
   [InlineData(false)]
   [InlineData(true)]
   public async Task MissingProject_ReturnsNotFoundWithoutSaving(bool post)
   {
      IndexModel model = BuildModel();
      _repository.Setup(x => x.GetProjectById(ProjectId))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.NotFound, null));

      IActionResult result = post ? await model.OnPostAsync(ProjectId) : await model.OnGetAsync(ProjectId);

      Assert.IsType<NotFoundResult>(result);
      _repository.Verify(x => x.SetSatelliteSiteArrangement(
         It.IsAny<int>(), It.IsAny<SetSignificantChangeSatelliteSiteArrangementCommand>()), Times.Never);
   }

   private IndexModel BuildModel(SignificantChangeSatelliteSiteArrangementResponse assessment = null)
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
         SatelliteSiteArrangement = assessment ?? new()
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
