using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeLandTransactionResponse
{
	public SignificantChangeLandTransactionConsent? LAndTransactionConsentSecured { get; set; }
	public string LandTransactionConsentAdditionalInfo { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}