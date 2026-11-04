namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeHqtfConsiderationCommand(
   HqtfConsiderationAnswer? HqtfConsiderationAnswer,
   string AdditionalInformation,
   string SupportingEvidence);
