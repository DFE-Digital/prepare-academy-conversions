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
    public class LikelyhoodImpactModelTests
    {
      private readonly Mock<IAcademyConversionProjectRepository> _academyConversionProjectRepository;
      private readonly Mock<ErrorService> _errorService;
      private readonly LikelyhoodImpactModel _subject;

      public LikelyhoodImpactModelTests()
      {
         _academyConversionProjectRepository = new Mock<IAcademyConversionProjectRepository>();
         _errorService = new Mock<ErrorService>();

         var httpContext = new DefaultHttpContext();
         httpContext.Request.Query = new QueryCollection();
         var pageContext = new PageContext
         {
            HttpContext = httpContext
         };

         _subject = new LikelyhoodImpactModel(_academyConversionProjectRepository.Object, _errorService.Object)
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

      [Theory]
      [InlineData("Unlikely", PublicSectorEqualityDutyImpact.Unlikely)]
      [InlineData("Some impact", PublicSectorEqualityDutyImpact.SomeImpact)]
      [InlineData("Likely", PublicSectorEqualityDutyImpact.Likely)]
      public async Task OnGet_Returns_Page(string impact, PublicSectorEqualityDutyImpact impactEnum)
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            PublicEqualityDutyImpact = impact,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         var result = await _subject.OnGetAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         _academyConversionProjectRepository.Verify(r => r.GetProjectById(conversionProject.Id), Times.Once);

         Assert.Equal(impactEnum, _subject.Impact);
      }

      [Fact]
      public async Task OnPost_Updates_Record_And_Redirects_to_Psed_screen_when_impact_is_unlikely()
      {
         var impact = "Unlikely";

         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            PublicEqualityDutyImpact = impact,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         _subject.Impact = PublicSectorEqualityDutyImpact.Unlikely;

         var response = await _subject.OnPostAsync(conversionProject.Id);

         _academyConversionProjectRepository.Verify(r => r.SetPublicEqualityDuty(
            conversionProject.Id,
            It.Is<SetConversionPublicEqualityDutyModel>(model =>
               model.Id == conversionProject.Id
               && model.PublicEqualityDutyImpact == impact
               && model.PublicEqualityDutyReduceImpactReason == string.Empty
               && !model.PublicEqualityDutySectionComplete)
         ), Times.Once);

         var redirectResponse = Assert.IsType<RedirectToPageResult>(response);

         Assert.Equal(Links.PublicSectorEqualityDutySection.ConversionTask.Page, redirectResponse.PageName);
         Assert.Equal(conversionProject.Id, redirectResponse.RouteValues["id"]);
      }

      [Theory]
      [InlineData("Some impact", PublicSectorEqualityDutyImpact.SomeImpact)]
      [InlineData("Likely", PublicSectorEqualityDutyImpact.Likely)]
      public async Task OnPost_Updates_Record_And_Redirects_to_Psed_reasons_screen_when_impact_is_likely_or_some_impact(string impact, PublicSectorEqualityDutyImpact impactEnum)
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            PublicEqualityDutyImpact = impact,
            PublicEqualityDutyReduceImpactReason = "Some reason",
            PublicEqualityDutySectionComplete = true,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         _subject.Impact = impactEnum;

         var response = await _subject.OnPostAsync(conversionProject.Id);

         _academyConversionProjectRepository.Verify(r => r.SetPublicEqualityDuty(
            conversionProject.Id,
            It.Is<SetConversionPublicEqualityDutyModel>(model =>
               model.Id == conversionProject.Id
               && model.PublicEqualityDutyImpact == impact
               && model.PublicEqualityDutyReduceImpactReason == conversionProject.PublicEqualityDutyReduceImpactReason
               && model.PublicEqualityDutySectionComplete)
         ), Times.Once);

         var pageResult = Assert.IsType<RedirectToPageResult>(response);

         Assert.Equal("/TaskList/PublicSectorEqualityDuty/ImpactReductionReason", WebUtility.UrlDecode(pageResult.PageName));
      }
   }
}
