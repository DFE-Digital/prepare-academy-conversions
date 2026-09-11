namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeReligiousBodyConsultationCommand(
   bool? trustConsultedReligiousBody,
   string trustConsultedReligiousBodyNotConsultedReason)
{
   public bool? TrustConsultedReligiousBody { get; set; } = trustConsultedReligiousBody;
   public string TrustConsultedReligiousBodyNotConsultedReason { get; set; } = trustConsultedReligiousBodyNotConsultedReason;
}
