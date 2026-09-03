using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models;

public class SignificantChangeDecisionTests
{
   [Fact]
   public void Switching_from_approved_to_declined_clears_the_condition_answers()
   {
      SignificantChangeDecision decision = new()
      {
         Decision = SignificantChangeDecisions.Approved,
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "Some conditions"
      };

      decision.Decision = SignificantChangeDecisions.Declined;

      decision.ApprovedConditionsSet.Should().BeNull("the API requires null unless Approved");
      decision.ApprovedConditionsDetails.Should().BeNull();
   }

   [Fact]
   public void Switching_from_declined_to_deferred_clears_the_declined_reasons()
   {
      SignificantChangeDecision decision = new() { Decision = SignificantChangeDecisions.Declined };
      decision.DeclinedReasons.Add(
         new SignificantChangeDeclinedReasonDetails(SignificantChangeDeclinedReason.Finance, "Cost"));

      decision.Decision = SignificantChangeDecisions.Deferred;

      decision.DeclinedReasons.Should().BeEmpty("the API rejects reasons that do not match the decision");
   }

   [Fact]
   public void Switching_to_withdrawn_clears_deferred_reasons()
   {
      SignificantChangeDecision decision = new() { Decision = SignificantChangeDecisions.Deferred };
      decision.DeferredReasons.Add(
         new AdvisoryBoardDeferredReasonDetails(AdvisoryBoardDeferredReason.Other, "Waiting"));

      decision.Decision = SignificantChangeDecisions.Withdrawn;

      decision.DeferredReasons.Should().BeEmpty();
   }

   [Fact]
   public void Reassigning_the_same_decision_keeps_the_answers()
   {
      SignificantChangeDecision decision = new() { Decision = SignificantChangeDecisions.Declined };
      decision.DeclinedReasons.Add(
         new SignificantChangeDeclinedReasonDetails(SignificantChangeDeclinedReason.Finance, "Cost"));

      decision.Decision = SignificantChangeDecisions.Declined;

      decision.DeclinedReasons.Should().HaveCount(1);
   }

   [Theory]
   [InlineData(SignificantChangeDecisions.Approved, null, "Approved")]
   [InlineData(SignificantChangeDecisions.Approved, true, "Approved with conditions")]
   [InlineData(SignificantChangeDecisions.Declined, null, "Declined")]
   [InlineData(SignificantChangeDecisions.Deferred, null, "Deferred")]
   [InlineData(SignificantChangeDecisions.Withdrawn, null, "Withdrawn")]
   public void GetDecisionAsFriendlyName_returns_the_display_name(
      SignificantChangeDecisions decision, bool? conditionsSet, string expected)
   {
      SignificantChangeDecision model = new() { Decision = decision, ApprovedConditionsSet = conditionsSet };

      model.GetDecisionAsFriendlyName().Should().Be(expected);
   }

   [Fact]
   public void Declined_reason_values_match_the_api()
   {
      ((int)SignificantChangeDeclinedReason.Finance).Should().Be(0);
      ((int)SignificantChangeDeclinedReason.Performance).Should().Be(1);
      ((int)SignificantChangeDeclinedReason.Other).Should().Be(2);
      ((int)SignificantChangeDeclinedReason.Governance).Should().Be(3);
      ((int)SignificantChangeDeclinedReason.ChoiceOfTrust).Should().Be(4);
   }
}