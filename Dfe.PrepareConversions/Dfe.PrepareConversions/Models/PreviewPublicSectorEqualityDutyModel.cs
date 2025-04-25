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
               result = "The decision is unlikely to disproportionately affect any particular person or group who share protected characteristics";
               break;
            case "Some impact":
               result = "There are some impacts but on balance the analysis indicates these changes will not disproportionately affect any particular person or group who share protected";
               break;
            case "Likely":
               result = "The decision is likely to disproportionately affect any particular person or group who share protected characteristics";
               break;
            default:
               break;
         }

         return result;
      }

      public static bool IsValid(string impact, string reduceImpactReason, bool sectionComplete)
      {
         var result = true;

         if (!sectionComplete || ((impact == "Some impact" || impact == "Likely") && string.IsNullOrWhiteSpace(reduceImpactReason)))
         {
            result = false;
         }

         return result;
      }
   }
}
