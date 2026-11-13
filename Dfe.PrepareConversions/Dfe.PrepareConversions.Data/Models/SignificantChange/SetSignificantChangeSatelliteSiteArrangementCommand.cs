namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeSatelliteSiteArrangementCommand(
   YesNoNotApplicableAnswer? Answer,
   string AdditionalInformation,
   string SupportingEvidence);
