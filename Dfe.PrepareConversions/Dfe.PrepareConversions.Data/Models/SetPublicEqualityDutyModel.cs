
namespace Dfe.PrepareConversions.Data.Models
{
    public abstract class SetPublicEqualityDutyModel(string publicEqualityDutyImpact, string publicEqualityDutyReduceImpactReason, bool publicEqualityDutySectionComplete)
    {
      public string PublicEqualityDutyImpact { get; set; } = publicEqualityDutyImpact;

      public string PublicEqualityDutyReduceImpactReason { get; set; } = publicEqualityDutyReduceImpactReason;

      public bool PublicEqualityDutySectionComplete { get; init; } = publicEqualityDutySectionComplete;
    }
}
