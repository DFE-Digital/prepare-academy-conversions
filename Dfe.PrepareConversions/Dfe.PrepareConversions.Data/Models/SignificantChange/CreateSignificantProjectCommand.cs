namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record CreateSignificantProjectCommand(int Urn, byte Tier, string Route, string TrustUkprn);
