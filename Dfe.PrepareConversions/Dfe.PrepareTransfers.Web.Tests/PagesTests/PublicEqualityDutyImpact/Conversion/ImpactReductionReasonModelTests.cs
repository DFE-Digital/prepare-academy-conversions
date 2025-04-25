using System.Net;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.PublicSectorEqualityDuty.Conversion;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.PublicEqualityDutyImpact.Conversion
{
    public class ImpactReductionReasonModelTests
    {
      private readonly Mock<IAcademyConversionProjectRepository> _academyConversionProjectRepository;
      private readonly Mock<ErrorService> _errorService;
      private readonly ImpactReductionReasonModel _subject;

      public ImpactReductionReasonModelTests()
      {
         _academyConversionProjectRepository = new Mock<IAcademyConversionProjectRepository>();
         _errorService = new Mock<ErrorService>();

         var httpContext = new DefaultHttpContext();
         httpContext.Request.Query = new QueryCollection();
         var pageContext = new PageContext
         {
            HttpContext = httpContext
         };

         _subject = new ImpactReductionReasonModel(_academyConversionProjectRepository.Object, _errorService.Object)
         {
            PageContext = pageContext
         };
      }

      [Fact]
      public async Task OnGet_Returns_NotFound()
      {
         var id = It.IsAny<int>();

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.NotFound, null));

         var result = await _subject.OnGetAsync(id);

         Assert.IsType<NotFoundResult>(result);
      }

      [Fact]
      public async Task OnGet_Returns_Page()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            PublicEqualityDutyReduceImpactReason = "Some likely reason",
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         var result = await _subject.OnGetAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         _academyConversionProjectRepository.Verify(r => r.GetProjectById(conversionProject.Id), Times.Once);

         Assert.Equal(conversionProject.PublicEqualityDutyReduceImpactReason, _subject.Reason);
      }

      [Fact]
      public async Task OnPost_Updates_Record()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            PublicEqualityDutyImpact = "Likely",
            PublicEqualityDutySectionComplete = true,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         var response = await _subject.OnPostAsync(conversionProject.Id);

         _academyConversionProjectRepository.Verify(r => r.SetPublicEqualityDuty(
            conversionProject.Id,
            It.Is<SetConversionPublicEqualityDutyModel>(model =>
               model.Id == conversionProject.Id
               && model.PublicEqualityDutyImpact == conversionProject.PublicEqualityDutyImpact
               && model.PublicEqualityDutyReduceImpactReason == _subject.Reason
               && model.PublicEqualityDutySectionComplete == conversionProject.PublicEqualityDutySectionComplete)
         ), Times.Once);

         var redirectResponse = Assert.IsType<RedirectToPageResult>(response);

         Assert.Equal(Links.PublicSectorEqualityDutySection.ConversionTask.Page, redirectResponse.PageName);
         Assert.Equal(conversionProject.Id, redirectResponse.RouteValues["id"]);
      }
   }
}
