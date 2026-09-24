namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeLandTransactionConsentCommand(
   SignificantChangeLandTransactionConsent? landTransactionConsent,
   string landTransactionConsentAdditionalInfo)
{
   public SignificantChangeLandTransactionConsent? LandTransactionConsent { get; set; } = landTransactionConsent;
   public string LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;
}