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

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }
}
