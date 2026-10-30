using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeLandTransactionResponse
{
    public SignificantChangeGenericYesNoNa LandTransactionApplication { get; set; }
    public string LandTransactionApplicationAdditionalInfo { get; set; } = string.Empty;
	public SignificantChangeGenericYesNoNa? LandTransactionConsent { get; set; }
	public string LandTransactionConsentAdditionalInfo { get; set; } = string.Empty;
    public string LandTransactionSupportingEvidence { get; set; } = string.Empty;
    public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}