using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision
{
   public class AdvisoryBoardDecision
   {
      private AdvisoryBoardDecisions? _decision;

      public AdvisoryBoardDecision()
      {
         DeferredReasons = new List<AdvisoryBoardDeferredReasonDetails>();
         DeclinedReasons = new List<AdvisoryBoardDeclinedReasonDetails>();
         WithdrawnReasons = new List<AdvisoryBoardWithdrawnReasonDetails>();
         DAORevokedReasons = new List<AdvisoryBoardDAORevokedReasonDetails>();
      }

      public int AdvisoryBoardDecisionId { get; set; }
      public int ConversionProjectId { get; set; }
      public bool? ApprovedConditionsSet { get; set; }
      public string ApprovedConditionsDetails { get; set; }
      public List<AdvisoryBoardDeclinedReasonDetails> DeclinedReasons { get; set; }
      public List<AdvisoryBoardDeferredReasonDetails> DeferredReasons { get; set; }
      public List<AdvisoryBoardWithdrawnReasonDetails> WithdrawnReasons { get; set; }
      public List<AdvisoryBoardDAORevokedReasonDetails> DAORevokedReasons { get; set; }
      public DateTime? AdvisoryBoardDecisionDate { get; set; }
      public DecisionMadeBy? DecisionMadeBy { get; set; }
      public DateTime? AcademyOrderDate { get; set; }
      public string DecisionMakerName { get; set; }

      public AdvisoryBoardDecisions? Decision
      {
         get => _decision;
         set
         {
            if (value != _decision)
            {
               ClearReasonsExceptForDecision(value);
               _decision = value;
            }
         }
      }

      private void ClearReasonsExceptForDecision(AdvisoryBoardDecisions? decision)
      {
         switch (decision)
         {
            case AdvisoryBoardDecisions.Approved:
               DeclinedReasons.Clear();
               DeferredReasons.Clear();
               WithdrawnReasons.Clear();
               DAORevokedReasons.Clear();
               break;
            case AdvisoryBoardDecisions.Declined:
               ApprovedConditionsSet = null;
               ApprovedConditionsDetails = null;
               DeferredReasons.Clear();
               WithdrawnReasons.Clear();
               DAORevokedReasons.Clear();
               break;
            case AdvisoryBoardDecisions.Deferred:
               ApprovedConditionsSet = null;
               ApprovedConditionsDetails = null;
               DeclinedReasons.Clear();
               WithdrawnReasons.Clear();
               DAORevokedReasons.Clear();
               break;
            case AdvisoryBoardDecisions.Withdrawn:
               ApprovedConditionsSet = null;
               ApprovedConditionsDetails = null;
               DeclinedReasons.Clear();
               DeferredReasons.Clear();
               DAORevokedReasons.Clear();
               break;
            case AdvisoryBoardDecisions.DAORevoked:
               ApprovedConditionsSet = null;
               ApprovedConditionsDetails = null;
               DeclinedReasons.Clear();
               DeferredReasons.Clear();
               WithdrawnReasons.Clear();
               break;
            default:
               break;
         }
      }

      public string GetDecisionAsFriendlyName()
      {
         return this switch
         {
            { Decision: AdvisoryBoardDecisions.Approved, ApprovedConditionsSet: true } => "Approved with Conditions",
            { Decision: AdvisoryBoardDecisions.DAORevoked } => "DAO Revoked",
            _ => Decision?.ToString()
         };
      }
   }
}
