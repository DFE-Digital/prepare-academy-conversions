
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeDecision
{
   private SignificantChangeDecisions? _decision;

   public SignificantChangeDecision()
   {
      DeclinedReasons = new List<SignificantChangeDeclinedReasonDetails>();
      DeferredReasons = new List<AdvisoryBoardDeferredReasonDetails>();
      WithdrawnReasons = new List<AdvisoryBoardWithdrawnReasonDetails>();
   }

   public int? SignificantChangeProjectId { get; set; }
   public bool? ApprovedConditionsSet { get; set; }
   public string ApprovedConditionsDetails { get; set; }
   public List<SignificantChangeDeclinedReasonDetails> DeclinedReasons { get; set; }
   public List<AdvisoryBoardDeferredReasonDetails> DeferredReasons { get; set; }
   public List<AdvisoryBoardWithdrawnReasonDetails> WithdrawnReasons { get; set; }
   public DateTime? DecisionDate { get; set; }
   public DecisionMadeBy? DecisionMadeBy { get; set; }
   public string DecisionMakerName { get; set; }

   // Declared last on purpose - see note above.
   public SignificantChangeDecisions? Decision
   {
      get => _decision;
      set
      {
         if (value != _decision)
         {
            ClearAnswersNotRelevantTo(value);
            _decision = value;
         }
      }
   }

   /// <summary>
   ///    The API rejects a decision that carries answers belonging to a different decision type,
   ///    so switching branch mid-wizard must discard the stale answers.
   /// </summary>
   private void ClearAnswersNotRelevantTo(SignificantChangeDecisions? decision)
   {
      switch (decision)
      {
         case SignificantChangeDecisions.Approved:
            DeclinedReasons.Clear();
            DeferredReasons.Clear();
            WithdrawnReasons.Clear();
            break;
         case SignificantChangeDecisions.Declined:
            ApprovedConditionsSet = null;
            ApprovedConditionsDetails = null;
            DeferredReasons.Clear();
            WithdrawnReasons.Clear();
            break;
         case SignificantChangeDecisions.Deferred:
            ApprovedConditionsSet = null;
            ApprovedConditionsDetails = null;
            DeclinedReasons.Clear();
            WithdrawnReasons.Clear();
            break;
         case SignificantChangeDecisions.Withdrawn:
            ApprovedConditionsSet = null;
            ApprovedConditionsDetails = null;
            DeclinedReasons.Clear();
            DeferredReasons.Clear();
            break;
         default:
            break;
      }
   }

   public string GetDecisionAsFriendlyName()
   {
      return this switch
      {
         { Decision: SignificantChangeDecisions.Approved, ApprovedConditionsSet: true } => "Approved with conditions",
         _ => Decision?.ToString()
      };
   }
}