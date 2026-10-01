namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeLandTransactionCommand(
   SignificantChange_Generic_YesNoNa? landTransactionApplication,
   string landTransactionApplicationAdditionalInfo,
   SignificantChange_Generic_YesNoNa? landTransactionConsent,
   string landTransactionConsentAdditionalInfo, 
   string landTransactionSupportingEvidence)
{
   public SignificantChange_Generic_YesNoNa? LandTransactionApplication { get; set; } = landTransactionApplication;
   public string LandTransactionApplicationAdditionalInfo { get; set; } = landTransactionApplicationAdditionalInfo;

   public SignificantChange_Generic_YesNoNa? LandTransactionConsent { get; set; } = landTransactionConsent;
   public string LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;

   public string LandTransactionSupportingEvidence { get; set; } = landTransactionSupportingEvidence;
}