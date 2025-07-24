using Xunit;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using System.Net;
using Dfe.PrepareConversions.Data;
using System;
using Dfe.Academisation.ExtensionMethods;

namespace Dfe.PrepareConversions.Tests.Services.DocumentGenerator
{
    public class DocumentGeneratorTests
    {
      [Fact]
      public void GenerateDocument_ReturnsHtbTemplate_AndDocumentByteArray()
      {
         // Arrange
         var project = new AcademyConversionProject {
            SchoolName = "Test School",
            AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored,
            SchoolType = "pupil referral unit",
            ConversionSupportGrantNumberOfSites = "2",
            ApplicationReceivedDate = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ConversionSupportGrantAmount = 30000,
            ConversionSupportGrantChangeReason = "I need some cash please",
            RecommendationForProject = "Test recommendation for project",
            RationaleForProject = "Test rationale for project",
            ProjectStatus = "In Progress",
            FormAMatProjectId = 12345,
            Form7Received = "Yes",
            Form7ReceivedDate = new DateTime(2030, 1, 2, 0, 0, 0, DateTimeKind.Utc),
            ConversionSupportGrantType = "Sponsored",
            ConversionSupportGrantEnvironmentalImprovementGrant = "No",
            CreatedOn = DateTime.UtcNow,
            LocalAuthority = "Test Authority",
            LocalAuthorityInformationTemplateSentDate = DateTime.UtcNow,
            LocalAuthorityInformationTemplateReturnedDate = DateTime.UtcNow,
            LocalAuthorityInformationTemplateComments = "Test comments",
            LocalAuthorityInformationTemplateLink = "http://example.com/template",
            LocalAuthorityInformationTemplateSectionComplete = true,
            DaoPackSentDate = DateTime.UtcNow,
            Region = "North West",
            AssignedDate = DateTime.UtcNow,
            HeadTeacherBoardDate = DateTime.UtcNow,
            BaselineDate = DateTime.UtcNow,
            PreviousHeadTeacherBoardDateQuestion = "Was the previous head teacher board date provided?",
            PreviousHeadTeacherBoardDate = DateTime.UtcNow,
            PreviousHeadTeacherBoardLink = "http://example.com/previous-htb",
            TrustReferenceNumber = "TRN12345",
            NameOfTrust = "Test Trust",
            SponsorReferenceNumber = "SRN12345",
            SponsorName = "Test Sponsor",
            Urn = 123456,
            ApplicationReferenceNumber = "APP123456",
            SchoolPhase = "Secondary",
            SchoolSharePointId = Guid.NewGuid(),
            ApplicationSharePointId = Guid.NewGuid()

         };
         var response = new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, project);
         var schoolOverview = new SchoolOverview { SchoolType = "Primary" };
         var keyStagePerformance = new KeyStagePerformance();
         byte[] documentBytes;

         // Act
         var result = PrepareConversions.Services.DocumentGenerator.DocumentGenerator.GenerateDocument(
             response,
             schoolOverview,
             keyStagePerformance,
             project,
             out documentBytes);

         // Assert
         Assert.NotNull(result);

         Assert.NotNull(documentBytes);
         Assert.True(documentBytes.Length > 0);

         Assert.Equal("Test School", result.SchoolName);
         Assert.Equal("Primary", result.SchoolType);
         Assert.Equal("2", result.ConversionSupportGrantNumberOfSites);
         Assert.Equal(project.ApplicationReceivedDate.ToDateString(), result.ApplicationReceivedDate);
         Assert.Equal("Sponsored - £30,000.00", result.AcademyTypeRouteAndConversionGrant);
         Assert.Equal("I need some cash please", result.ConversionSupportGrantChangeReason);
         Assert.Equal("Test recommendation for project", result.RecommendationForProject);
         Assert.Equal("Test rationale for project", result.RationaleForProject);
      }
   }
}
