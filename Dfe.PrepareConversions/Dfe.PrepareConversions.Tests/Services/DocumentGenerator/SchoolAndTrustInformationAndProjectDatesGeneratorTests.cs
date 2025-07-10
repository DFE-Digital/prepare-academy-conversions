using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Services.DocumentGenerator;
using Dfe.PrepareConversions.Tests.Pages;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services.DocumentGenerator
{
   public class SchoolAndTrustInformationAndProjectDatesGeneratorTests : BaseIntegrationTests
   {
      public SchoolAndTrustInformationAndProjectDatesGeneratorTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

      [Fact]
      public void AddLocalAuthorityAndSponsorDetails_ReturnsExpectedElements()
      {
         // Arrange
         var project = new AcademyConversionProject
         {
               LocalAuthority = "Test LA",
               SponsorName = "Test Sponsor",
               SponsorReferenceNumber = "12345"
         };

         // Act
         var result = SchoolAndTrustInformationAndProjectDatesGenerator.AddLocalAuthorityAndSponsorDetails(project);

         // Assert
         Assert.Equal(3, result.Count);
         Assert.Contains(result, row => row[0].Value.Equals("Local authority") && row[1].Value.Equals("Test LA"));
         Assert.Contains(result, row => row[0].Value.Equals("Sponsor name") && row[1].Value.Equals("Test Sponsor"));
         Assert.Contains(result, row => row[0].Value.Equals("Sponsor reference number") && row[1].Value.Equals("12345"));
      }

      [Fact]
      public void Should_render_conversion_details_into_word_document()
      {
         // Arrange
         AcademyConversionProject project = AddGetProject(x =>
         {
            x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
            x.ApplicationReceivedDate = new DateTime(2024, 12, 16, 15, 0, 0, DateTimeKind.Utc); // pre deadline
            x.ConversionSupportGrantAmount = 30000;
            x.ConversionSupportGrantChangeReason = "I need some cash please";
            x.RecommendationForProject = "Test recommendation for project";
            x.SchoolType = "pupil referral unit";
            x.ConversionSupportGrantNumberOfSites = "2";
         });

         // Act
         List<TextElement[]> elements = SchoolAndTrustInformationAndProjectDatesGenerator.VoluntaryRouteInfo(project);

         // Assert
         Assert.NotNull(elements);
         Assert.Equal(5, elements.Count);

         Assert.Equal("Academy type and route", elements[0][0].Value);
         Assert.Equal(AcademyTypeAndRoutes.Voluntary, elements[0][1].Value);

         Assert.Equal("Grant funding amount", elements[1][0].Value);
         Assert.Equal(project.ConversionSupportGrantAmount?.ToMoneyString(true), elements[1][1].Value);

         Assert.Equal("Grant funding reason", elements[2][0].Value);
         Assert.Equal(project.ConversionSupportGrantChangeReason, elements[2][1].Value);

         Assert.Equal("Recommendation", elements[3][0].Value);
         Assert.Equal(project.RecommendationForProject, elements[3][1].Value);

         Assert.Equal("Number of sites", elements[4][0].Value);
         Assert.Equal(project.ConversionSupportGrantNumberOfSites?.ToString(), elements[4][1].Value);
      }

      [Fact]
      public void Should_render_conversion_details_into_word_document_without_grant_amount_and_reason_when_post_dealine()
      {
         // Arrange
         AcademyConversionProject project = AddGetProject(x =>
         {
            x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
            x.ApplicationReceivedDate = new DateTime(2024, 12, 28, 15, 0, 0, DateTimeKind.Utc); // post deadline
            x.ConversionSupportGrantAmount = 0;
            x.ConversionSupportGrantChangeReason = null;
            x.RecommendationForProject = "Test recommendation for project";
            x.SchoolType = "pupil referral unit";
            x.ConversionSupportGrantNumberOfSites = "2";
         });

         // Act
         List<TextElement[]> elements = SchoolAndTrustInformationAndProjectDatesGenerator.VoluntaryRouteInfo(project);

         // Assert
         Assert.NotNull(elements);
         Assert.Equal(3, elements.Count);

         Assert.Equal("Academy type and route", elements[0][0].Value);
         Assert.Equal(AcademyTypeAndRoutes.Voluntary, elements[0][1].Value);

         Assert.Equal("Recommendation", elements[1][0].Value);
         Assert.Equal(project.RecommendationForProject, elements[1][1].Value);

         Assert.Equal("Number of sites", elements[2][0].Value);
         Assert.Equal(project.ConversionSupportGrantNumberOfSites?.ToString(), elements[2][1].Value);
      }

      [Fact]
      public void Should_render_SponsoredRouteInfo_into_word_document()
      {
         // Arrange
         AcademyConversionProject project = AddGetProject(x =>
         {
            x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
            x.ConversionSupportGrantType = "test suport grant type";
            x.ConversionSupportGrantAmount = 30000;
            x.ConversionSupportGrantChangeReason = "I need some cash please";
            x.ConversionSupportGrantEnvironmentalImprovementGrant = "Yes";
            x.Form7Received = "Yes";
            x.Form7ReceivedDate = new DateTime(2024, 12, 16, 15, 0, 0, DateTimeKind.Utc);
            x.DaoPackSentDate = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            x.SchoolType = "test pupil referral unit";
            x.ConversionSupportGrantNumberOfSites = "2";
         });

         // Act
         List<TextElement[]> elements = SchoolAndTrustInformationAndProjectDatesGenerator.SponsoredRouteInfo(project);

         // Assert
         Assert.NotNull(elements);
         Assert.Equal(9, elements.Count);

         Assert.Equal("Academy type and route", elements[0][0].Value);
         Assert.Equal(AcademyTypeAndRoutes.Sponsored, elements[0][1].Value);

         Assert.Equal("Grant funding type", elements[1][0].Value);
         Assert.Equal(project.ConversionSupportGrantType, elements[1][1].Value);

         Assert.Equal("Grant funding amount", elements[2][0].Value);
         Assert.Equal(project.ConversionSupportGrantAmount?.ToMoneyString(true), elements[2][1].Value);

         Assert.Equal("Grant funding reason", elements[3][0].Value);
         Assert.Equal(project.ConversionSupportGrantChangeReason, elements[3][1].Value);

         Assert.Equal("Is the school applying for an EIG (Environmental Improvement Grant)?", elements[4][0].Value);
         Assert.Equal(project.ConversionSupportGrantEnvironmentalImprovementGrant, elements[4][1].Value);

         Assert.Equal("Has the Schools Notification Mailbox (SNM) received a Form 7?", elements[5][0].Value);
         Assert.Equal(project.Form7Received, elements[5][1].Value);

         Assert.Equal("Date SNM received Form 7", elements[6][0].Value);
         Assert.Equal(project.Form7ReceivedDate.ToDateString(), elements[6][1].Value);

         Assert.Equal("Date directive academy order (DAO) pack sent", elements[7][0].Value);
         Assert.Equal(project.DaoPackSentDate.ToDateString(), elements[7][1].Value);

         Assert.Equal("Number of sites", elements[8][0].Value);
         Assert.Equal(project.ConversionSupportGrantNumberOfSites?.ToString(), elements[8][1].Value);
      }

      [Fact]
      public void Should_render_advisory_board_dates_into_word_document()
      {
         // Arrange
         var headTeacherBoardDate = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);
         var proposedConversionDate = new DateTime(2029, 1, 1, 0, 0, 0, DateTimeKind.Utc);
         var PreviousHeadTeacherBoardDate = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc);


         AcademyConversionProject project = AddGetProject(x =>
         {
            x.HeadTeacherBoardDate = headTeacherBoardDate;
            x.ProposedConversionDate = proposedConversionDate;
            x.PreviousHeadTeacherBoardDate = PreviousHeadTeacherBoardDate;
         });

         // Act
         List<TextElement[]> elements = SchoolAndTrustInformationAndProjectDatesGenerator.AddAdvisoryBoardDetails(project);

         // Assert
         Assert.NotNull(elements);
         Assert.Equal(3, elements.Count);

         Assert.Equal("Proposed decision date", elements[0][0].Value);
         Assert.Equal(headTeacherBoardDate.ToDateString(), elements[0][1].Value);

         Assert.Equal("Proposed academy opening date", elements[1][0].Value);
         Assert.Equal(proposedConversionDate.ToDateString(), elements[1][1].Value);

         Assert.Equal("Previously considered date", elements[2][0].Value);
         Assert.Equal(PreviousHeadTeacherBoardDate.ToDateString(), elements[2][1].Value);
      }
   }
}
