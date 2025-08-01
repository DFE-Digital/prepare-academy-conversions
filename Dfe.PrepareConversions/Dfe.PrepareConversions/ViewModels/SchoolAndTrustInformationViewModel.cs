﻿namespace Dfe.PrepareConversions.ViewModels;

public class SchoolAndTrustInformationViewModel
{
   public string Id { get; set; }
   public string RecommendationForProject { get; set; }
   public string Author { get; set; }
   public string Version { get; set; }
   public string ClearedBy { get; set; }
   public string HeadTeacherBoardDate { get; set; }
   public string PreviousHeadTeacherBoardDate { get; set; }
   public string PreviousHeadTeacherBoardLink { get; set; }
   public string SchoolName { get; set; }
   public string SchoolType { get; set; }
   public string SchoolUrn { get; set; }
   public string LocalAuthority { get; set; }
   public string TrustReferenceNumber { get; set; }
   public string NameOfTrust { get; set; }
   public string SponsorReferenceNumber { get; set; }
   public string SponsorName { get; set; }
   public string AcademyTypeAndRoute { get; set; }
   public string ProposedAcademyOpeningDate { get; set; }
   public string ConversionSupportGrantAmount { get; set; }
   public string ConversionSupportNumberOfSites { get; set; }
   public string ConversionSupportGrantChangeReason { get; set; }
   public string ConversionSupportGrantType { get; set; }
   public string ConversionSupportGrantEnvironmentalImprovementGrant { get; set; }
   public bool IsApplicationReceivedBeforeSupportGrantDeadline { get; set; } = false;
   public bool IsVoluntaryConversionSupportGrantVisible { get; set; } = false;
   public string DaoPackSentDate { get; set; }
   public string Form7Received { get; set; }
   public string Form7ReceivedDate { get; set; }
   public bool IsDao { get; set; }
   public bool IsPRU => SchoolType?.ToLower().Equals("pupil referral unit") ?? false;
   public bool IsSEN => SchoolType?.ToLower().Contains("special") ?? false;
   public bool WasForm7Received { get; set; }

   public bool IsPreview { get; set; }
   public bool IsReadOnly { get; set; }
}
