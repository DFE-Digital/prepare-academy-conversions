namespace Dfe.PrepareConversions.Data.Models.SignificantChange
{
   public class SignificantChangeSpecialSchoolSatelliteConsiderationsResponse
   {
      public YesNoNotApplicableAnswer? Answer { get; set; }
      public string AdditionalInformation { get; set; } = string.Empty;
      public string SupportingEvidence { get; set; } = string.Empty;
      public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
   }
}
