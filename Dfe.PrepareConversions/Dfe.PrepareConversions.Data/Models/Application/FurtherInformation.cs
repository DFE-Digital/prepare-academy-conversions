namespace Dfe.PrepareConversions.Data.Models.Application;

public class FurtherInformation
{
   public string WhatWillSchoolBringToTrust { get; set; }
   public bool HasUnpublishedOfstedInspection { get; set; }
   public bool HasSafeguardingInvestigations { get; set; }
   public bool IsPartOfLocalAuthorityReorganisation { get; set; }
   public bool IsPartOfLocalAuthorityClosurePlans { get; set; }
   public bool IsLinkedToDiocese { get; set; }
   public string NameOfDiocese { get; set; }
   public Link DioceseLetterOfConsent { get; set; }
   public bool IsPartOfFederation { get; set; }
   public bool IsSupportedByFoundationTrustOrOtherBody { get; set; }
   public bool HasSACREChristianWorshipExcemption { get; set; }
   public string MainFeederSchools { get; set; }
   public Link SchoolConsent { get; set; }
   public string EqualitiesImpactAssessmentResult { get; set; }
   public string AdditionalInformation { get; set; }
}
