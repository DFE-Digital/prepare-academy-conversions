namespace Dfe.PrepareConversions.Data.Models.NewProject;

public record CreateNewProject(NewProjectSchool School, NewProjectTrust Trust);

public record NewProjectTrust(string Name, string ReferenceNumber);

public record NewProjectSchool(string Name, string Urn, bool PartOfPfiScheme, string LocalAuthorityName, string Region);
