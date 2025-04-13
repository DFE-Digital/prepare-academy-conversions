using System;
using System.Net;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Pages.TaskList.PublicSectorEqualityDuty.Conversion;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.PublicEqualityDutyImpact.Conversion
{
    public class TaskTests
    {
      private readonly Mock<IAcademyConversionProjectRepository> _academyConversionProjectRepository;
      private readonly ErrorService _errorService = new();
      private readonly TaskModel _subject;

      public TaskTests()
      {
         _academyConversionProjectRepository = new Mock<IAcademyConversionProjectRepository>();
         _subject = new TaskModel(_academyConversionProjectRepository.Object, _errorService);
      }

      [Fact]
      public async Task Given_Id_Returns_NotFound()
      {
         var id = It.IsAny<int>();

         _academyConversionProjectRepository.Setup(s => s.GetProjectById(id))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.NotFound, null));

         var result = await _subject.OnGetAsync(id);

         Assert.IsType<NotFoundResult>(result);
      }

      [Theory]
      [InlineData("Unlikely", "The equalities duty has been considered and the Secretary of State’s decision is unlikely to affect disproportionately any particular person or group who share protected characteristics.")]
      [InlineData("Some impact", "The equalities duty has been considered and there are some impacts but on balance the analysis indicates these changes will not affect disproportionately any particular person or group who share protected characteristics.")]
      [InlineData("Likely", "The equalities duty has been considered and the decision is likely to affect disproportionately a particular person or group who share protected characteristics")]
      public async Task Given_Id_Returns_Custom_Impact_Reason_Label(string impact, string reasonLabel)
      {
         AcademyConversionProject conversionProject = new AcademyConversionProject
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
      public async Task Given_Id_Returns_New_Task_Page()
      {
         AcademyConversionProject conversionProject = new AcademyConversionProject
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
      public async Task Given_Id_Returns_Page()
      {
         AcademyConversionProject conversionProject = new AcademyConversionProject
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
         Assert.Equal("The equalities duty has been considered and the decision is likely to affect disproportionately a particular person or group who share protected characteristics", _subject.ReduceImpactReasonLabel);
         Assert.Equal(conversionProject.PublicEqualityDutySectionComplete, _subject.SectionComplete);
         Assert.False(_subject.IsNew);
         Assert.True(_subject.RequiresReason);
      }
   }
}

//AdvisoryBoardDecisions.Approved => new ProjectStatus(result.ToString().ToUpper(), green),
//               AdvisoryBoardDecisions.Deferred => new ProjectStatus(result.ToString().ToUpper(), orange),
//               AdvisoryBoardDecisions.Declined => new ProjectStatus(result.ToString().ToUpper(), red),
//               AdvisoryBoardDecisions.DAORevoked => new ProjectStatus("DAO Revoked", red),
//               AdvisoryBoardDecisions.Withdrawn => new ProjectStatus(result.ToString().ToUpper(), purple),