namespace Dfe.PrepareConversions.Data.Models
{
    public class SetConversionPublicEqualityDutyModel(int id, string publicEqualityDutyImpact, string publicEqualityDutyReduceImpactReason, bool publicEqualityDutySectionComplete) : SetPublicEqualityDutyModel(publicEqualityDutyImpact, publicEqualityDutyReduceImpactReason, publicEqualityDutySectionComplete)
    {
      public int Id { get; set; } = id;
    }
}
