namespace Dfe.PrepareConversions.Data.Models.InvoluntaryProject
{
   public record CreateInvoluntaryProject(InvoluntaryProjectSchool School, InvoluntaryProjectTrust Trust);

	public record InvoluntaryProjectTrust(string Name, string ReferenceNumber);

	public record InvoluntaryProjectSchool(string Name, string Urn, bool PartOfPfiScheme);
}
