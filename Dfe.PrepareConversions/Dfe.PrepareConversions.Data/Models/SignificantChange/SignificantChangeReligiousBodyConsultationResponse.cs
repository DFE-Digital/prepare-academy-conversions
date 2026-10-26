namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeReligiousBodyConsultationResponse
{
   public YesNoNotApplicableAnswer? TrustConsultedReligiousBody { get; set; }
   public string TrustConsultedReligiousBodyNotConsultedReason { get; set; } = string.Empty;
   public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; }
   public string ReligiousBodyRaisedObjectionReason { get; set; } = string.Empty;
   public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; }
   public string ReligiousBodyConsentNotGrantedReason { get; set; } = string.Empty;
   public string SupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
