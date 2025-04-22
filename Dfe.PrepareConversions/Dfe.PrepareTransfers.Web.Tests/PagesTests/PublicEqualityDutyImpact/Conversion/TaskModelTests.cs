using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.PublicSectorEqualityDuty.Conversion;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Moq;
using Xunit;
using FluentAssertions;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.PublicEqualityDutyImpact.Conversion
{
    public class TaskModelTests
    {
      private readonly Mock<IAcademyConversionProjectRepository> _academyConversionProjectRepository;
      private readonly Mock<ErrorService> _errorService;
      private readonly TaskModel _subject;

      public TaskModelTests()
      {
         _academyConversionProjectRepository = new Mock<IAcademyConversionProjectRepository>();
         _errorService = new Mock<ErrorService>();
         _subject = new TaskModel(_academyConversionProjectRepository.Object, _errorService.Object);
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
      [InlineData("Unlikely", "The decision is unlikely to disproportionately affect any particular person or group who share protected characteristics")]
      [InlineData("Some impact", "There are some impacts but on balance the analysis indicates these changes will not disproportionately affect any particular person or group who share protected")]
      [InlineData("Likely", "The decision is likely to disproportionately affect any particular person or group who share protected characteristics")]
      public async Task OnGet_Returns_Custom_Impact_Reason_Label(string impact, string reasonLabel)
      {
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

         var result = await _subject.OnGetAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         Assert.Equal(reasonLabel, _subject.ReduceImpactReasonLabel);
      }

      [Fact]
      public async Task OnGet_Returns_New_Task_Page()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            PublicEqualityDutyImpact = null,
            PublicEqualityDutyReduceImpactReason = null,
            PublicEqualityDutySectionComplete = false,
            ProjectStatus = "Deferred"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         var result = await _subject.OnGetAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         _academyConversionProjectRepository.Verify(r => r.GetProjectById(conversionProject.Id), Times.Once);

         Assert.Equal(conversionProject.PublicEqualityDutyReduceImpactReason, _subject.ReduceImpactReason);
         Assert.True(string.IsNullOrEmpty(_subject.ReduceImpactReasonLabel));
         Assert.Equal(conversionProject.PublicEqualityDutySectionComplete, _subject.SectionComplete);
         Assert.True(_subject.IsNew);
         Assert.False(_subject.RequiresReason);
      }

      [Fact]
      public async Task OnGet_Returns_Page()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            PublicEqualityDutyImpact = "Likely",
            PublicEqualityDutyReduceImpactReason = "Some likely reason",
            PublicEqualityDutySectionComplete = true,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         var result = await _subject.OnGetAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         _academyConversionProjectRepository.Verify(r => r.GetProjectById(conversionProject.Id), Times.Once);

         Assert.Equal(conversionProject.PublicEqualityDutyReduceImpactReason, _subject.ReduceImpactReason);
         Assert.Equal("The decision is likely to disproportionately affect any particular person or group who share protected characteristics", _subject.ReduceImpactReasonLabel);
         Assert.Equal(conversionProject.PublicEqualityDutySectionComplete, _subject.SectionComplete);
         Assert.False(_subject.IsNew);
         Assert.True(_subject.RequiresReason);
      }

      [Fact]
      public async Task OnPost_Navigates_Back_When_Complete_Not_Checked()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         var result = await _subject.OnPostAsync(conversionProject.Id);

         RedirectToPageResult redirectResponse = Assert.IsType<RedirectToPageResult>(result);
         Assert.Equal(Links.TaskList.Index.Page, redirectResponse.PageName);
      }

      [Fact]
      public async Task OnPost_Returns_Error_Consider_Public_Sector_Equality_Duty()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         _subject.SectionComplete = true;

         var result = await _subject.OnPostAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         List<Error> errors = _errorService.Object.GetErrors().ToList();
         errors.Count.Should().Be(1);
         errors[0].Key.Should().Be("Impact");
         errors[0].Message.Should().Be("Consider the public Sector Equality Duty");
      }

      [Fact]
      public async Task OnPost_Returns_Error_Describe_What_Will_Be_Done_To_Reduce_Impact()
      {
         AcademyConversionProject conversionProject = new()
         {
            Id = 12345,
            Urn = 113609,
            SchoolName = "Plymouth",
            PublicEqualityDutyImpact = "Likely",
            PublicEqualityDutyReduceImpactReason = "", // is required
            PublicEqualityDutySectionComplete = false,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         _subject.SectionComplete = true;

         var result = await _subject.OnPostAsync(conversionProject.Id);

         Assert.IsType<PageResult>(result);

         List<Error> errors = _errorService.Object.GetErrors().ToList();
         errors.Count.Should().Be(1);
         errors[0].Key.Should().Be("ReduceImpactReason");
         errors[0].Message.Should().Be("Describe what will be done to reduce the impact");
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
            PublicEqualityDutyReduceImpactReason = "Some likely reason",
            PublicEqualityDutySectionComplete = true,
            ProjectStatus = "Approved"
         };

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(conversionProject.Id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, conversionProject));

         _subject.SectionComplete = true;

         var response = await _subject.OnPostAsync(conversionProject.Id);

         _academyConversionProjectRepository.Verify(r => r.SetPublicEqualityDuty(
            conversionProject.Id,
            It.Is<SetConversionPublicEqualityDutyModel>(model => 
               model.Id == conversionProject.Id
               && model.PublicEqualityDutyImpact == conversionProject.PublicEqualityDutyImpact
               && model.PublicEqualityDutyReduceImpactReason == conversionProject.PublicEqualityDutyReduceImpactReason
               && model.PublicEqualityDutySectionComplete == _subject.SectionComplete
            )
         ), Times.Once);

         var redirectResponse = Assert.IsType<RedirectToPageResult>(response);

         Assert.Equal(Links.TaskList.Index.Page, redirectResponse.PageName);
         Assert.Equal(conversionProject.Id, redirectResponse.RouteValues["id"]);
      }
   }
}