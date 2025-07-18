using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList;

public class PreviewProjectTemplateTests : BaseIntegrationTests
{
   public PreviewProjectTemplateTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Given_SponsoredConversion_When_Previewed_Then_RationaleForProject_Is_Hidden()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#rationale-for-project").Should().BeNull();
   }

   [Fact]
   public async Task Given_SponsoredConversion_When_Previewed_Then_LegalRequirements_Are_Hidden()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#legal-requirements-heading").Should().BeNull();
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   public async Task Given_Not_SponsoredConversion_When_Previewed_Then_LegalRequirements_Are_Shown(string routeToConversion)
   {
      void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = routeToConversion;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#legal-requirements-heading").Should().NotBeNull();
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   public async Task Given_Not_SponsoredConversion_When_Previewed_Then_RationaleForProject_Is_Shown(string routeToConversion)
   {
      void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = routeToConversion;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#rationale-for-project").Should().NotBeNull();
   }

   [Fact]
   public async Task Given_SponsoredConversion_And_No_AdvisoryBoardDate_And_ValidPsed_When_CreateProjectDocument_Button_Clicked_Then_ValidationError_Shown()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.HeadTeacherBoardDate = null;
         project.PublicEqualityDutyImpact = null;
         project.PublicEqualityDutyReduceImpactReason = null;
         project.PublicEqualityDutySectionComplete = false;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");

      // Act
      await Document.QuerySelector<IHtmlButtonElement>("#generate-template-button")!.SubmitAsync();

      // Assert
      var errorSummaryTitle = Document.QuerySelector("#error-summary-title");

      errorSummaryTitle.Should().NotBeNull();
      errorSummaryTitle.Text().Trim().Should().Be("There is a problem");

      string selector = "*[data-element-type='error-link']";
      var errors = Document.QuerySelectorAll(selector);
      errors.Length.Should().Be(2);

      errors[0].TextContent.Should().Be("Set a Proposed decision date before you generate your project template");
      errors[1].TextContent.Should().Be("Consider the Public Sector Equality Duty");
   }

   [Fact]
   public async Task Given_SponsoredConversion_And_No_AdvisoryBoardDate_When_CreateProjectDocument_Button_Clicked_Then_ValidationError_Shown()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.HeadTeacherBoardDate = null;
         project.PublicEqualityDutyImpact = "Likely";
         project.PublicEqualityDutyReduceImpactReason = "Some reason";
         project.PublicEqualityDutySectionComplete = true;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");

      // Act
      await Document.QuerySelector<IHtmlButtonElement>("#generate-template-button")!.SubmitAsync();

      // Assert
      var errorSummaryTitle = Document.QuerySelector("#error-summary-title");

      errorSummaryTitle.Should().NotBeNull();
      errorSummaryTitle.Text().Trim().Should().Be("There is a problem");

      string selector = "*[data-element-type='error-link']";
      var errors = Document.QuerySelectorAll(selector);
      errors.Length.Should().Be(1);

      errors.First().TextContent.Should().Be("Set a Proposed decision date before you generate your project template");
   }

   [Fact]
   public async Task Given_SponsoredConversion_And_No_Valid_Psed_When_CreateProjectDocument_Button_Clicked_Then_ValidationError_Shown()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.HeadTeacherBoardDate = DateTime.Now;
         project.PublicEqualityDutyImpact = null;
         project.PublicEqualityDutyReduceImpactReason = null;
         project.PublicEqualityDutySectionComplete = false;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");

      // Act
      await Document.QuerySelector<IHtmlButtonElement>("#generate-template-button")!.SubmitAsync();

      // Assert
      var errorSummaryTitle = Document.QuerySelector("#error-summary-title");

      errorSummaryTitle.Should().NotBeNull();
      errorSummaryTitle.Text().Trim().Should().Be("There is a problem");

      string selector = "*[data-element-type='error-link']";
      var errors = Document.QuerySelectorAll(selector);
      errors.Length.Should().Be(1);

      errors.First().TextContent.Should().Be("Consider the Public Sector Equality Duty");
   }

   [Fact]
   public async Task Given_SponsoredConversion_With_AdvisoryBoardDate_And_ValidPsed_Then_Navigates_To_Download_Page()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.HeadTeacherBoardDate = DateTime.Now;
         project.PublicEqualityDutyImpact = "Likely";
         project.PublicEqualityDutyReduceImpactReason = "Some reason";
         project.PublicEqualityDutySectionComplete = true;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");

      // Act
      await Document.QuerySelector<IHtmlButtonElement>("#generate-template-button")!.SubmitAsync();

      // Assert
      Document.Url.Should().Be(BuildRequestAddress($"/task-list/{project.Id}/download-project-template"));
   }
}