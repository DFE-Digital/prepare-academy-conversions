namespace Dfe.PrepareConversions.Data.Models
{
   public class SetSchoolOverviewModel
   {
      public int Id { get; set; }
      public string PublishedAdmissionNumber { get; set; }
      public string ViabilityIssues { get; set; }
      public string FinancialDeficit { get; set; }
      public decimal? NumberOfPlacesFundedFor { get; set; }
      public decimal? NumberOfResidentialPlaces { get; set; }
      public decimal? NumberOfFundedResidentialPlaces { get; set; }
      public string PartOfPfiScheme { get; set; }
      public string PfiSchemeDetails { get; set; }
      public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
      public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
      public string MemberOfParliamentNameAndParty { get; set; }
      public bool? PupilsAttendingGroupPermanentlyExcluded { get; set; }
      public bool? PupilsAttendingGroupMedicalAndHealthNeeds { get; set; }
      public bool? PupilsAttendingGroupTeenageMums { get; set; }

      public SetSchoolOverviewModel() { }

      public SetSchoolOverviewModel(
         int id,
          string publishedAdmissionNumber,
          string viabilityIssues,
          string financialDeficit,
          decimal? numberOfPlacesFundedFor,
          decimal? numberOfResidentialPlaces,
          decimal? numberOfFundedResidentialPlaces,
          string partOfPfiScheme,
          string pfiSchemeDetails,
          decimal? distanceFromSchoolToTrustHeadquarters,
          string distanceFromSchoolToTrustHeadquartersAdditionalInformation,
          string memberOfParliamentNameAndParty,
          bool? pupilsAttendingGroupPermanentlyExcluded,
          bool? pupilsAttendingGroupMedicalAndHealthNeeds,
          bool? pupilsAttendingGroupTeenageMums)
      {
         Id = id;
         PublishedAdmissionNumber = publishedAdmissionNumber;
         ViabilityIssues = viabilityIssues;
         FinancialDeficit = financialDeficit;
         NumberOfPlacesFundedFor = numberOfPlacesFundedFor;
         NumberOfResidentialPlaces = numberOfResidentialPlaces;
         NumberOfFundedResidentialPlaces = numberOfFundedResidentialPlaces;
         PartOfPfiScheme = partOfPfiScheme;
         PfiSchemeDetails = pfiSchemeDetails;
         DistanceFromSchoolToTrustHeadquarters = distanceFromSchoolToTrustHeadquarters;
         DistanceFromSchoolToTrustHeadquartersAdditionalInformation = distanceFromSchoolToTrustHeadquartersAdditionalInformation;
         MemberOfParliamentNameAndParty = memberOfParliamentNameAndParty;
         PupilsAttendingGroupPermanentlyExcluded = pupilsAttendingGroupPermanentlyExcluded;
         PupilsAttendingGroupMedicalAndHealthNeeds = pupilsAttendingGroupMedicalAndHealthNeeds;
         PupilsAttendingGroupTeenageMums = pupilsAttendingGroupTeenageMums;
      }
   }
}
