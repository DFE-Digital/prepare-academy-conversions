namespace Dfe.PrepareConversions.Data.Models
{
   public record UpdateSchoolOverview(
      string PublishedAdmissionNumber,
      string ViabilityIssues,
      string FinancialDeficit,
      string PartOfPfiScheme,
      string PfiSchemeDetails,
      string DistanceFromSchoolToTrustHeadquarters,
      string DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
      string MemberOfParliamentNameAndParty);
}
