namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public sealed record SetSignificantChangeMainstreamSchoolSatelliteConsiderationsCommand(
   YesNoNotApplicableAnswer? Answer,
   string AdditionalInformation,
   string SupportingEvidenceSharepointLink);
