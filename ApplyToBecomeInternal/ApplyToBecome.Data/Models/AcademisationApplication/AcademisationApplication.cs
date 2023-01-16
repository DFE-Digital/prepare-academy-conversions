using ApplyToBecome.Data.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecome.Data.Models.AcademisationApplication
{
   public class AcademisationApplication
   {
      public int ApplicationId { get; set; }
      public string ApplicationType { get; set; }
      public string ApplicationStatus { get; set; }
      public string ApplicationReference { get; set; }
      public List<Contributor> Contributors { get; set; }
      public List<School> Schools { get; set; }

      public static Application.Application MapToApplication(AcademisationApplication academisationApplication)
      {
         // Following the fields used by the front end
         var academiesApplication = new Application.Application();
         //app.TrustName = academisationApplication.TrustName (exists in current, not present on academisation unless it is foundationTrustOrBodyName?)
         academiesApplication.ApplicationId = academisationApplication.ApplicationReference; //Do we want this to be the case going forwards?
         //academiesApplication.ApplicationLeadAuthorName = academisationApplication.Contributors (we have contributors on academisation but how do we know their the lead)
         //academiesApplication.ChangesToTrust = academisationApplication.Schools.FirstOrDefault().
         //academiesApplication.ChangesToTrust

         return academiesApplication;
      }
   }

   public class Contributor
   {
      public int ContributorId { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string EmailAddress { get; set; }
      public string Role { get; set; }
      public string OtherRoleName { get; set; }
   }

   public class School
   {
      public int Id { get; set; }
      public int Urn { get; set; }
      public string SchoolName { get; set; }
      public LandAndBuildings LandAndBuildings { get; set; }
      public string TrustBenefitDetails { get; set; }
      public string OfstedInspectionDetails { get; set; }
      public string SafeguardingDetails { get; set; }
      public string LocalAuthorityReorganisationDetails { get; set; }
      public string LocalAuthorityClosurePlanDetails { get; set; }
      public string DioceseName { get; set; }
      public string DioceseFolderIdentifier { get; set; }
      public bool PartOfFederation { get; set; }
      public string FoundationTrustOrBodyName { get; set; }
      public string FoundationConsentFolderIdentifier { get; set; }
      public string MainFeederSchools { get; set; }
      public string ResolutionConsentFolderIdentifier { get; set; }
      public string FurtherInformation { get; set; }
      public FinancialYear PreviousFinancialYear { get; set; }
      public FinancialYear CurrentFinancialYear { get; set; }
      public FinancialYear NextFinancialYear { get; set; }
      public List<Loan> Loans { get; set; } // Based off current
      public List<Lease> Leases { get; set; } // Based off current
      public string SchoolConversionContactHeadName { get; set; }
      public string SchoolConversionContactHeadEmail { get; set; }
      public string SchoolConversionContactHeadTel { get; set; }
      public string SchoolConversionContactChairName { get; set; }
      public string SchoolConversionContactChairEmail { get; set; }
      public string SchoolConversionContactChairTel { get; set; }
      public string SchoolConversionContactRole { get; set; }
      public string SchoolConversionMainContactOtherName { get; set; }
      public string SchoolConversionMainContactOtherEmail { get; set; }
      public string SchoolConversionMainContactOtherTelephone { get; set; }
      public string SchoolConversionMainContactOtherRole { get; set; }
      public string SchoolConversionApproverContactName { get; set; }
      public string SchoolConversionApproverContactEmail { get; set; }
      public string SchoolConversionTargetDateSpecified { get; set; }
      public DateTime SchoolConversionTargetDate { get; set; }
      public string SchoolConversionTargetDateExplained { get; set; }
      public string ConversionChangeNamePlanned { get; set; }
      public string ProposedNewSchoolName { get; set; }
      public string ApplicationJoinTrustReason { get; set; }
      public string ProjectedPupilNumbersYear1 { get; set; }
      public string ProjectedPupilNumbersYear2 { get; set; }
      public string ProjectedPupilNumbersYear3 { get; set; }
      public string SchoolCapacityAssumptions { get; set; }
      public string SchoolCapacityPublishedAdmissionNumber { get; set; }
      public string SchoolSupportGrantFundsPaidTo { get; set; }
      public bool ConfirmPaySupportGrantToSchool { get; set; }
      public bool SchoolHasConsultedStakeholders { get; set; }
      public string SchoolPlanToConsultStakeholders { get; set; }
      public bool FinanceOngoingInvestigations { get; set; }
      public string FinancialInvestigationsExplain { get; set; }
      public bool FinancialInvestigationsTrustAware { get; set; }
      public bool DeclarationBodyAgree { get; set; }
      public bool DeclarationIAmTheChairOrHeadteacher { get; set; }
      public string DeclarationSignedByName { get; set; }
   }

   public class LandAndBuildings
   {
      public string OwnerExplained { get; set; }
      public bool WorksPlanned { get; set; }
      public DateTime WorksPlannedDate { get; set; }
      public string WorksPlannedExplained { get; set; }
      public bool FacilitiesShared { get; set; }
      public string FacilitiesSharedExplained { get; set; }
      public bool Grants { get; set; }
      public string GrantsAwardingBodies { get; set; }
      public bool PartOfPfiScheme { get; set; }
      public bool PartOfPrioritySchoolsBuildingProgramme { get; set; }
      public bool PartOfBuildingSchoolsForFutureProgramme { get; set; }
   }

   public class FinancialYear
   {
      public DateTime FinancialYearEndDate { get; set; }
      public double Revenue { get; set; }
      public string RevenueStatus { get; set; }
      public string RevenueStatusExplained { get; set; }
      public string RevenueStatusFileLink { get; set; }
      public double CapitalCarryForward { get; set; }
      public string CapitalCarryForwardStatus { get; set; }
      public string CapitalCarryForwardExplained { get; set; }
   }
}
