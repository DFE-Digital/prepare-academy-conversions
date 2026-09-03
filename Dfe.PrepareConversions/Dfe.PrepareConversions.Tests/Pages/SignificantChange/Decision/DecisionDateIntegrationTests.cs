using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class DecisionDateIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartAtDecisionDate()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
      await Wizard.SetIsConditionalAndContinue(false);
      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
      await Wizard.SetDecisionMakerNameAndContinue("Jane Smith");
   }

   [Fact]
   public async Task Should_continue_to_the_summary()
   {
      await StartAtDecisionDate();

      await Wizard.SetDecisionDateAndContinue(DateTime.UtcNow.AddMonths(-1));

      Document.Url.Should().Be(BuildRequestAddress(PathTo("summary")));
   }

   [Fact]
   public async Task Should_reject_a_future_date()
   {
      await StartAtDecisionDate();

      await Wizard.SetDecisionDateAndContinue(DateTime.UtcNow.AddMonths(1));

      Document.Url.Should().Be(BuildRequestAddress(PathTo("decision-date")));
      Document.QuerySelector("[data-cy='error-summary']").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_reject_a_partial_date()
   {
      await StartAtDecisionDate();

      InputWithId("decision-date-day").Value = "12";
      await Wizard.ClickSubmitButton();

      Document.Url.Should().Be(BuildRequestAddress(PathTo("decision-date")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Date must include a");
   }

   [Fact]
   public async Task Should_link_back_to_decision_maker_name()
   {
      await StartAtDecisionDate();

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain(PathTo("decision-maker"));
   }

   [Fact]
   public async Task Should_link_back_to_who_decided_when_nobody_made_the_decision()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
      await Wizard.SetIsConditionalAndContinue(false);
      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.None);

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain(PathTo("who-decided"));
   }
}