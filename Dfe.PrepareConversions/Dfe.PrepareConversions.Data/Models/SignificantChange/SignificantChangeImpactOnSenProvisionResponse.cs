namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public sealed record SignificantChangeImpactOnSenProvisionResponse
{
   public YesNoNotApplicableAnswer? Answer { get; init; }
   public string AdditionalInformation { get; init; } = string.Empty;
   public string SupportingEvidence { get; init; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; init; } = SignificantChangeTaskStatus.NotStarted;
}
