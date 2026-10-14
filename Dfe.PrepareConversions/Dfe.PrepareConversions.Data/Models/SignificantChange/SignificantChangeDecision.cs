
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeDecision
{
   private SignificantChangeDecisions? _decision;
   private string _declinedReason = string.Empty;

   public int AdvisoryBoardDecisionId { get; set; }

   public int? SignificantChangeProjectId { get; set; }
   public bool? ApprovedConditionsSet { get; set; }
   public string ApprovedConditionsDetails { get; set; }
   public string DeclinedReason
   {
      get => _declinedReason;
      set
      {
         _declinedReason = value ?? string.Empty;
         if (string.IsNullOrWhiteSpace(_declinedReason))
            DeclinedReasons.Clear();
         else
         {
            DeclinedReasons.Clear();
            DeclinedReasons.Add(_declinedReason);
         }
      }
   }

   // For API compatibility the decision should carry an array of declined reasons.
   public List<string> DeclinedReasons { get; set; } = new();

   public List<AdvisoryBoardWithdrawnReasonDetails> WithdrawnReasons { get; set; } = new();
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
            DeclinedReason = string.Empty;
            DeclinedReasons.Clear();
            WithdrawnReasons.Clear();
            break;
         case SignificantChangeDecisions.Declined:
            ApprovedConditionsSet = null;
            ApprovedConditionsDetails = null;
            WithdrawnReasons.Clear();
            break;
         case SignificantChangeDecisions.Withdrawn:
            ApprovedConditionsSet = null;
            ApprovedConditionsDetails = null;
            DeclinedReason = string.Empty;
            DeclinedReasons.Clear();
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