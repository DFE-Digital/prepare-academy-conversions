using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans
{
   public enum SchoolImprovementPlanConfidenceLevel
   {
      [Description("High")]
      High = 1,
      [Description("Medium")]
      Medium = 2,
      [Description("Low")]
      Low = 3,
      [Description("No Confidence")]
      NoConfidence = 4
   }
}
