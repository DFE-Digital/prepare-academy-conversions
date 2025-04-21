namespace Dfe.PrepareConversions.Models
{
   public class PreviewPublicSectorEqualityDutyModel(string id, string impact, string reduceImpactReason)
   {
      public string Id { get; set; } = id;
      public string Urn { get; set; } = id;

      public string Impact { get; set; } = impact;

      public string ReduceImpactReason { get; set; } = reduceImpactReason;

      public string ReduceImpactDescription
      {
         get
         {
            return GenerateReduceImpactReasonLabel(Impact);
         }
      }

      public bool RequiresReason
      {
         get
         {
            return Impact == "Some impact" || Impact == "Likely";
         }
      }

      public static string GenerateReduceImpactReasonLabel(string publicSectorEqualityDutyImpact)
      {
         var result = string.Empty;

         switch (publicSectorEqualityDutyImpact)
         {
            case "Unlikely":
               result = "The equalities duty has been considered and the Secretary of State's decision is unlikely to affect disproportionately any particular person or group who share protected characteristics.";
               break;
            case "Some impact":
               result = "The equalities duty has been considered and there are some impacts but on balance the analysis indicates these changes will not affect disproportionately any particular person or group who share protected characteristics.";
               break;
            case "Likely":
               result = "The equalities duty has been considered and the decision is likely to affect disproportionately a particular person or group who share protected characteristics.";
               break;
            default:
               break;
         }

         return result;
      }
   }
}
