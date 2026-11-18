namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeLandTransactionCommand(
   YesNoNotApplicableAnswer? landTransactionApplication,
   string landTransactionApplicationAdditionalInfo,
   YesNoNotApplicableAnswer? landTransactionConsent,
   string landTransactionConsentAdditionalInfo, 
   string landTransactionSupportingEvidenceSharepointLink)
{
   public YesNoNotApplicableAnswer? LandTransactionApplication { get; set; } = landTransactionApplication;
   public string LandTransactionApplicationAdditionalInfo { get; set; } = landTransactionApplicationAdditionalInfo;

   public YesNoNotApplicableAnswer? LandTransactionConsent { get; set; } = landTransactionConsent;
   public string LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;

   public string LandTransactionSupportingEvidenceSharepointLink { get; set; } = landTransactionSupportingEvidenceSharepointLink;
}