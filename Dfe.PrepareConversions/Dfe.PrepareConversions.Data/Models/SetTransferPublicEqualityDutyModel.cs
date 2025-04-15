namespace Dfe.PrepareConversions.Data.Models
{
    public class SetTransferPublicEqualityDutyModel(int urn, string publicEqualityDutyImpact, string publicEqualityDutyReduceImpactReason, bool publicEqualityDutySectionComplete) : SetPublicEqualityDutyModel(publicEqualityDutyImpact, publicEqualityDutyReduceImpactReason, publicEqualityDutySectionComplete)
    {
      public int Urn { get; set; } = urn;
    }
}
