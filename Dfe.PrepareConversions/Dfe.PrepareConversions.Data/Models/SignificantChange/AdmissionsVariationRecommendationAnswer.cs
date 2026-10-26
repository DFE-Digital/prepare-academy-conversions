using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum AdmissionsVariationRecommendationAnswer
{
   Approve = 1,
   Decline = 2,

   [Description("Not Applicable")]
   NotApplicable = 3
}
