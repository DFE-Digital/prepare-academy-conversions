using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.TrustTemplate;

public class TrustTemplateGuidanceIntegrationTests : BaseIntegrationTests
{
   public TrustTemplateGuidanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_between_trust_template_guidance_and_task_list()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Prepare your template");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-guidance");

      await NavigateAsync("Back to task list");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_navigate_to_getting_your_template_section()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/trust-guidance");

      await NavigateAsync("Getting your template from SharePoint", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-guidance#sharepoint");
   }

   [Fact]
   public async Task Should_navigate_to_updating_your_template_in_the_trust_area_section()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/trust-guidance");

      await NavigateAsync("Updating your template in the trust area in KIM", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-guidance#trust");
   }

   [Fact]
   public async Task Should_navigate_to_updating_your_template_in_the_sponsor_area_section()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/trust-guidance");

      await NavigateAsync("Updating your template in the sponsor area in KIM (if the trust has sponsor status)", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-guidance#sponsor");
   }

   [Fact]
   public async Task Should_navigate_to_download_your_trust_template_section()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/trust-guidance");

      await NavigateAsync("Download your trust template from KIM", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-guidance#download");
   }

   [Fact]
   public async Task Should_navigate_to_send_template_for_review_section()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/trust-guidance");

      await NavigateAsync("Send your project template and trust template for review", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-guidance#send");
   }
}
