using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList;

public class PreviewProjectTemplateTests : BaseIntegrationTests
{
   public PreviewProjectTemplateTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Given_InvoluntaryConversion_When_Previewed_Then_RationaleForProject_Is_Hidden()
   {
      void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#rationale-for-project").Should().BeNull();
   }

   [Fact]
   public async Task Given_InvoluntaryConversion_When_Previewed_Then_LegalRequirements_Are_Hidden()
   {
      void PostProjectSetup(AcademyConversionProject project)
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
   public async Task Given_Not_InvoluntaryConversion_When_Previewed_Then_LegalRequirements_Are_Shown(string routeToConversion)
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
   public async Task Given_Not_InvoluntaryConversion_When_Previewed_Then_RationaleForProject_Is_Shown(string routeToConversion)
   {
      void PostProjectSetup(AcademyConversionProject project)
      {
         project.AcademyTypeAndRoute = routeToConversion;
      }

      AcademyConversionProject project = AddGetProject(PostProjectSetup);
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/preview-project-template");
      Document.QuerySelector("#rationale-for-project").Should().NotBeNull();
   }
}