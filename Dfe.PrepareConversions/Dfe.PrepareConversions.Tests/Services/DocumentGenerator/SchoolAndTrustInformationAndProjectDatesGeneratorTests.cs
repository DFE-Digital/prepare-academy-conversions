using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Services.DocumentGenerator;
using Dfe.PrepareConversions.Tests.Extensions;
using Dfe.PrepareConversions.Tests.Pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services.DocumentGenerator
{
   public class SchoolAndTrustInformationAndProjectDatesGeneratorTests : BaseIntegrationTests
   {
      public SchoolAndTrustInformationAndProjectDatesGeneratorTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

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
            x.RecommendationNotesForProject = "Test recommendation notes";
            x.SchoolType = "pupil referral unit";
            x.ConversionSupportGrantNumberOfSites = "2";
         });

         // Act
         List<TextElement[]> elements = SchoolAndTrustInformationAndProjectDatesGenerator.VoluntaryRouteInfo(project);

         // Assert
         Assert.NotNull(elements);
         Assert.Equal(6, elements.Count);

         Assert.Equal("Academy type and route", elements[0][0].Value);
         Assert.Equal(AcademyTypeAndRoutes.Voluntary, elements[0][1].Value);

         Assert.Equal("Grant funding amount", elements[1][0].Value);
         Assert.Equal(project.ConversionSupportGrantAmount?.ToMoneyString(true), elements[1][1].Value);

         Assert.Equal("Grant funding reason", elements[2][0].Value);
         Assert.Equal(project.ConversionSupportGrantChangeReason, elements[2][1].Value);

         Assert.Equal("Recommendation", elements[3][0].Value);
         Assert.Equal(project.RecommendationForProject, elements[3][1].Value);

         Assert.Equal("Recommendation notes", elements[4][0].Value);
         Assert.Equal(project.RecommendationNotesForProject, elements[4][1].Value);

         Assert.Equal("Number of sites", elements[5][0].Value);
         Assert.Equal(project.ConversionSupportGrantNumberOfSites?.ToString(), elements[5][1].Value);
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
            x.RecommendationNotesForProject = null;
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
   }
}
