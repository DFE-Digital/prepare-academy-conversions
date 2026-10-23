namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeFhaRisksOrIssuesRaisedCommand(
   bool? FhaRisksOrIssuesRaised,
   string? AdditionalInformation,
   string? SupportingEvidence);
