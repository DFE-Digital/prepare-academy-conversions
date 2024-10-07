using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.KeyStagePerformance;

public class KeyStage2PerformanceAdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_not_have_update_additional_information()
   {
      var project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/key-stage-2-performance-tables");
      VerifyNullElement("Change");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-2-performance-tables");
   }
}
