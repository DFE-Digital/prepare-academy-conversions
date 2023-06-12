namespace Dfe.PrepareConversions.Data.Models.SponsoredProject;

public record CreateSponsoredProject(SponsoredProjectSchool School, SponsoredProjectTrust Trust);

public record SponsoredProjectTrust(string Name, string ReferenceNumber);

public record SponsoredProjectSchool(string Name, string Urn, bool PartOfPfiScheme, string LocalAuthorityName, string Region);
