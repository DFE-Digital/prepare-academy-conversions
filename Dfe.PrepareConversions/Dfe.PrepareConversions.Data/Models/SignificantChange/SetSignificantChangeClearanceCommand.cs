namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeClearanceCommand(
   string RegionsGroupClearance,
   string TrustRegulationClearance);
