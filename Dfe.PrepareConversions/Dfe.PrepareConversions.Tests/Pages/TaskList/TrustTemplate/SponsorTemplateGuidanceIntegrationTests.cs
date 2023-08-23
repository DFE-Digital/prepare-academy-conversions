using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.TrustTemplate
{
   public class SponsorTemplateGuidanceIntegrationTests : BaseIntegrationTests
   {
      public SponsorTemplateGuidanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

      [Fact]
      public async Task Should_navigate_between_sponsor_template_guidance_and_task_list()
      {
         AcademyConversionProject project = AddGetProject(x =>
         {
            x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         });

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
         await NavigateAsync("Prepare your template");

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/sponsor-guidance");

         await NavigateAsync("Back");

         Document.Url.Should().BeUrl($"/task-list/{project.Id}");
      }

      [Fact]
      public async Task Should_navigate_and_display_correct_heading_for_sponsored()
      {
         AcademyConversionProject project = AddGetProject(x =>
         {
            x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         });

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}/sponsor-guidance");

         Document.QuerySelector("h1.govuk-heading-l")!.InnerHtml.Should().Be("Prepare your sponsor template");
      }

   }
}