using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class DeclineReasonIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartDeclinedJourney()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Declined);
   }

   [Fact]
   public async Task Should_continue_to_who_decided_when_a_reason_is_given()
   {
      await StartDeclinedJourney();

      await Wizard.SetDeclinedReasonAndContinue("Not affordable");

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_require_details()
   {
      await StartDeclinedJourney();

      await Wizard.SetDeclinedReasonAndContinue(string.Empty);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("declined-reason")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Enter details");
   }

   [Fact]
   public async Task Should_repopulate_reasons_held_in_session()
   {
      await StartDeclinedJourney();
      await Wizard.SetDeclinedReasonAndContinue("Not affordable");

      await OpenAndConfirmPathAsync(PathTo("declined-reason"));

      Document.QuerySelector<IHtmlTextAreaElement>("#declinedReason-txtarea").Value.Should().Be("Not affordable");
   }
}