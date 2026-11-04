namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeReligiousBodyConsultationCommand(
   YesNoNotApplicableAnswer? trustConsultedReligiousBody,
   string trustConsultedReligiousBodyNotConsultedReason,
   YesNoNotApplicableAnswer? religiousBodyRaisedObjection,
   string religiousBodyRaisedObjectionReason,
   YesNoNotApplicableAnswer? religiousBodyConsentGranted,
   string religiousBodyConsentNotGrantedReason,
   string supportingEvidence)
{
   public YesNoNotApplicableAnswer? TrustConsultedReligiousBody { get; set; } = trustConsultedReligiousBody;
   public string TrustConsultedReligiousBodyNotConsultedReason { get; set; } = trustConsultedReligiousBodyNotConsultedReason;
   public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; } = religiousBodyRaisedObjection;
   public string ReligiousBodyRaisedObjectionReason { get; set; } = religiousBodyRaisedObjectionReason;
   public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; } = religiousBodyConsentGranted;
   public string ReligiousBodyConsentNotGrantedReason { get; set; } = religiousBodyConsentNotGrantedReason;
   public string SupportingEvidence { get; set; } = supportingEvidence;
}
