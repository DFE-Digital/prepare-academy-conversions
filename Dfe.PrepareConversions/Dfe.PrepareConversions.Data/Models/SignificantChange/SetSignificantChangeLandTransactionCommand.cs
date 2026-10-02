namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeLandTransactionCommand(
   SignificantChangeGenericYesNoNa? landTransactionApplication,
   string landTransactionApplicationAdditionalInfo,
   SignificantChangeGenericYesNoNa? landTransactionConsent,
   string landTransactionConsentAdditionalInfo, 
   string landTransactionSupportingEvidence)
{
   public SignificantChangeGenericYesNoNa? LandTransactionApplication { get; set; } = landTransactionApplication;
   public string LandTransactionApplicationAdditionalInfo { get; set; } = landTransactionApplicationAdditionalInfo;

   public SignificantChangeGenericYesNoNa? LandTransactionConsent { get; set; } = landTransactionConsent;
   public string LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;

   public string LandTransactionSupportingEvidence { get; set; } = landTransactionSupportingEvidence;
}