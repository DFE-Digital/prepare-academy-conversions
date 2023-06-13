using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using HandlebarsDotNet;
using System.IO;
using System.Linq;
using System.Text;
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
   [InlineData(AcademyTypeAndRoutes.FormAMat)]
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
   [InlineData(AcademyTypeAndRoutes.FormAMat)]
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
   public async Task Given_SponsoredConversion_And_No_AdvisoryBoardDate_When_Previewed_Then_ValidationError_Shown()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.HeadTeacherBoardDate = null;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#error-summary-title").Should().NotBeNull();
      Document.QuerySelector("#error-summary-title").Text().Trim().Should().Be("There is a problem");

      string selector = "*[data-element-type='error-link']";
      var errors = Document.QuerySelectorAll(selector);
      errors.Length.Should().Be(1);
      errors.First().TextContent.Should().Be("Set an Advisory Board date before you generate your project template");
   }

   [Fact]
   public async Task Given_SponsoredConversion_And_No_AdvisoryBoardDate_When_GenerateTemplate_Clicked_Then_Page_Is_Redisplayed()
   {
      static void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.HeadTeacherBoardDate = null;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);

      var expectedUrl = $"/task-list/{project.Id}/preview-project-template";

      await OpenAndConfirmPathAsync(expectedUrl);
      Document.QuerySelector("#error-summary-title").Should().NotBeNull();
      Document.QuerySelector("#error-summary-title").Text().Trim().Should().Be("There is a problem");

      string selector = "*[data-element-type='error-link']";
      var errors = Document.QuerySelectorAll(selector);
      errors.Length.Should().Be(1);
      errors.First().TextContent.Should().Be("Set an Advisory Board date before you generate your project template");

      // Act
      await Document.QuerySelector<IHtmlButtonElement>("#generate-template-button")!.SubmitAsync();

      Document.Url.Should().Be(BuildRequestAddress(expectedUrl));
   }
}