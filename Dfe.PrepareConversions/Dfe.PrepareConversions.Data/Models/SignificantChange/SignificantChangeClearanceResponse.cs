namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeClearanceResponse
{
   public string RegionsGroupClearance { get; set; } = string.Empty;
   public string TrustRegulationClearance { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
