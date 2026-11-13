namespace Dfe.PrepareConversions.Data.Models.SignificantChange
{
   public record SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand(
      YesNoNotApplicableAnswer? Answer,
      string AdditionalInformation,
      string SupportingEvidence);
}
