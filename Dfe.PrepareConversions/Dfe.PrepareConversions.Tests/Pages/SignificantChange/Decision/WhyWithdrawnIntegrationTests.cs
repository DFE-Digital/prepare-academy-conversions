using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class WhyWithdrawnIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartWithdrawnJourney()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Withdrawn);
   }

   [Fact]
   public async Task Should_continue_to_who_decided_when_a_reason_is_given()
   {
      await StartWithdrawnJourney();

      await Wizard.SetWithdrawnReasonAndContinue("Need finance pack");

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_require_at_least_one_reason()
   {
      await StartWithdrawnJourney();

      await Wizard.ClickSubmitButton();

      Document.QuerySelector("[data-cy='error-summary']")?.TextContent.Should().Contain("Enter details");
   }
}