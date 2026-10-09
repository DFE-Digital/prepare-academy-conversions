using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum AdmissionsVariationRecommendationAnswer
{
   Approve = 1,
   Defer = 2,
   Decline = 3,

   [Description("Not Applicable")]
   NotApplicable = 4
}
