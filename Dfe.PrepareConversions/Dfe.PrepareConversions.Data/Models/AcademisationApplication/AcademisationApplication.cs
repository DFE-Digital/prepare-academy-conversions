
using Dfe.PrepareConversions.Data.Models.Application;
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

      public static Application MapToApplication(AcademisationApplication academisationApplication)
      {
         // Following the fields used by the front end
         var academiesApplication = new Application();
         //app.TrustName = academisationApplication.TrustName (exists in current, not present on academisation unless it is foundationTrustOrBodyName?)
         academiesApplication.ApplicationId = academisationApplication.ApplicationReference; //Do we want this to be the case going forwards?
         //academiesApplication.ApplicationLeadAuthorName = academisationApplication.Contributors (we have contributors on academisation but how do we know their the lead)
         //academiesApplication.ChangesToTrust = academisationApplication.Schools.FirstOrDefault() // What property is this on the academisation api?
         //academiesApplication.ChangesToTrustExplained // same again
         //academiesApplication.ChangesToLaGovernance // same again

         // SCHOOL

         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolName = academisationApplication.Schools.FirstOrDefault().SchoolName; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionContactHeadName = academisationApplication.Schools.FirstOrDefault().SchoolConversionContactHeadName; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionContactHeadEmail = academisationApplication.Schools.FirstOrDefault().SchoolConversionContactHeadEmail; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionContactHeadTel = academisationApplication.Schools.FirstOrDefault().SchoolConversionContactHeadTel; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionContactChairName = academisationApplication.Schools.FirstOrDefault().SchoolConversionContactChairName; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionContactChairEmail = academisationApplication.Schools.FirstOrDefault().SchoolConversionContactChairEmail; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionContactChairTel = academisationApplication.Schools.FirstOrDefault().SchoolConversionContactChairTel; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionMainContactOtherName = academisationApplication.Schools.FirstOrDefault().SchoolConversionMainContactOtherName; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionMainContactOtherEmail = academisationApplication.Schools.FirstOrDefault().SchoolConversionMainContactOtherEmail; // Why are these lists, our application form has just one?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionTargetDateSpecified = academisationApplication.Schools.FirstOrDefault().SchoolConversionTargetDateSpecified;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionTargetDate = academisationApplication.Schools.FirstOrDefault().SchoolConversionTargetDate;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionTargetDateExplained = academisationApplication.Schools.FirstOrDefault().SchoolConversionTargetDateExplained;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionReasonsForJoining = academisationApplication.Schools.FirstOrDefault().ApplicationJoinTrustReason;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionChangeNamePlanned = academisationApplication.Schools.FirstOrDefault().ConversionChangeNamePlanned;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolConversionProposedNewSchoolName = academisationApplication.Schools.FirstOrDefault().ProposedNewSchoolName;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdSchoolContributionToTrust = academisationApplication.Schools.FirstOrDefault().TrustBenefitDetails; // Is this the correct property for "What will the school bring to the trust they are joining?"
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdInspectedButReportNotPublished = academisationApplication.Schools.FirstOrDefault().OfstedInspectionDetails.Any(); // There isn't an equivalent bool but I guess we can work with the details?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdInspectedButReportNotPublishedExplain = academisationApplication.Schools.FirstOrDefault().OfstedInspectionDetails;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolOngoingSafeguardingInvestigations = academisationApplication.Schools.FirstOrDefault().SafeguardingDetails.Any(); // There isn't an equivalent bool but I guess we can work with the details?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolOngoingSafeguardingDetails = academisationApplication.Schools.FirstOrDefault().SafeguardingDetails;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolPartOfLaReorganizationPlan = academisationApplication.Schools.FirstOrDefault().LocalAuthorityReorganisationDetails.Any(); // There isn't an equivalent bool but I guess we can work with the details?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLaReorganizationDetails = academisationApplication.Schools.FirstOrDefault().LocalAuthorityReorganisationDetails;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolPartOfLaClosurePlan = academisationApplication.Schools.FirstOrDefault().LocalAuthorityClosurePlanDetails.Any();
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLaClosurePlanDetails = academisationApplication.Schools.FirstOrDefault().LocalAuthorityClosurePlanDetails; // There isn't an equivalent bool but I guess we can work with the details?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolFaithSchoolDioceseName = academisationApplication.Schools.FirstOrDefault().DioceseName;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolIsPartOfFederation = academisationApplication.Schools.FirstOrDefault().PartOfFederation;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolIsSupportedByFoundation = academisationApplication.Schools.FirstOrDefault().FoundationTrustOrBodyName.Any(); // There isn't an equivalent bool but I guess we can work with the details?
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolSupportedFoundationBodyName = academisationApplication.Schools.FirstOrDefault().FoundationTrustOrBodyName;
         //academiesApplication.ApplyingSchools.FirstOrDefault().SchoolHasSACREException // No equivalent on academisation
         //academiesApplication.ApplyingSchools.FirstOrDefault().SchoolSACREExemptionEndDate // No equivalent on academisation
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdFeederSchools = academisationApplication.Schools.FirstOrDefault().MainFeederSchools;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdEqualitiesImpactAssessmentCompleted = academisationApplication.Schools.FirstOrDefault().ResolutionConsentFolderIdentifier.Any(); // Is this the related property? Not sure 
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdEqualitiesImpactAssessmentDetails = academisationApplication.Schools.FirstOrDefault().ResolutionConsentFolderIdentifier; // Is this the related property? Not sure 
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdditionalInformationAdded = academisationApplication.Schools.FirstOrDefault().FurtherInformation.Any(); // Is this the related property? Not sure 
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolAdditionalInformation = academisationApplication.Schools.FirstOrDefault().FurtherInformation; // Is this the related property? Not sure 

         // FINANCES

         academiesApplication.ApplyingSchools.FirstOrDefault().PreviousFinancialYear.FYEndDate = academisationApplication.Schools.FirstOrDefault().PreviousFinancialYear.FinancialYearEndDate;
         academiesApplication.ApplyingSchools.FirstOrDefault().PreviousFinancialYear.RevenueCarryForward = (decimal?)academisationApplication.Schools.FirstOrDefault().PreviousFinancialYear.Revenue;
         //academiesApplication.ApplyingSchools.FirstOrDefault().PreviousFinancialYear.RevenueIsDeficit = academisationApplication.Schools.FirstOrDefault().PreviousFinancialYear.RevenueStatus; // is this what this might be
         academiesApplication.ApplyingSchools.FirstOrDefault().PreviousFinancialYear.CapitalCarryForward = (decimal?)academisationApplication.Schools.FirstOrDefault().PreviousFinancialYear.CapitalCarryForward;
         //academiesApplication.ApplyingSchools.FirstOrDefault().PreviousFinancialYear.CapitalIsDeficit = academisationApplication.Schools.FirstOrDefault().PreviousFinancialYear.CapitalCarryForwardStatus; // is this what this might be

         academiesApplication.ApplyingSchools.FirstOrDefault().CurrentFinancialYear.FYEndDate = academisationApplication.Schools.FirstOrDefault().CurrentFinancialYear.FinancialYearEndDate;
         academiesApplication.ApplyingSchools.FirstOrDefault().CurrentFinancialYear.RevenueCarryForward = (decimal?)academisationApplication.Schools.FirstOrDefault().CurrentFinancialYear.Revenue;
         //academiesApplication.ApplyingSchools.FirstOrDefault().CurrentFinancialYear.RevenueIsDeficit = academisationApplication.Schools.FirstOrDefault().CurrentFinancialYear.RevenueStatus; // is this what this might be
         academiesApplication.ApplyingSchools.FirstOrDefault().CurrentFinancialYear.CapitalCarryForward = (decimal?)academisationApplication.Schools.FirstOrDefault().CurrentFinancialYear.CapitalCarryForward;
         //academiesApplication.ApplyingSchools.FirstOrDefault().CurrentFinancialYear.CapitalIsDeficit = academisationApplication.Schools.FirstOrDefault().CurrentFinancialYear.CapitalCarryForwardStatus; // is this what this might be

         academiesApplication.ApplyingSchools.FirstOrDefault().NextFinancialYear.FYEndDate = academisationApplication.Schools.FirstOrDefault().NextFinancialYear.FinancialYearEndDate;
         academiesApplication.ApplyingSchools.FirstOrDefault().NextFinancialYear.RevenueCarryForward = (decimal?)academisationApplication.Schools.FirstOrDefault().NextFinancialYear.Revenue;
         //academiesApplication.ApplyingSchools.FirstOrDefault().NextFinancialYear.RevenueIsDeficit = academisationApplication.Schools.FirstOrDefault().NextFinancialYear.RevenueStatus; // is this what this might be
         academiesApplication.ApplyingSchools.FirstOrDefault().NextFinancialYear.CapitalCarryForward = (decimal?)academisationApplication.Schools.FirstOrDefault().NextFinancialYear.CapitalCarryForward;
         //academiesApplication.ApplyingSchools.FirstOrDefault().NextFinancialYear.CapitalIsDeficit = academisationApplication.Schools.FirstOrDefault().NextFinancialYear.CapitalCarryForwardStatus; // is this what this might be

         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLoans.FirstOrDefault().SchoolLoanAmount = academisationApplication.Schools.FirstOrDefault().Loans.FirstOrDefault().SchoolLoanAmount;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLoans.FirstOrDefault().SchoolLoanPurpose = academisationApplication.Schools.FirstOrDefault().Loans.FirstOrDefault().SchoolLoanPurpose;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLoans.FirstOrDefault().SchoolLoanProvider = academisationApplication.Schools.FirstOrDefault().Loans.FirstOrDefault().SchoolLoanProvider;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLoans.FirstOrDefault().SchoolLoanInterestRate = academisationApplication.Schools.FirstOrDefault().Loans.FirstOrDefault().SchoolLoanInterestRate;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLoans.FirstOrDefault().SchoolLoanSchedule = academisationApplication.Schools.FirstOrDefault().Loans.FirstOrDefault().SchoolLoanSchedule;

         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeaseRepaymentValue = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeaseRepaymentValue;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeasePurpose = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeasePurpose;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeasePaymentToDate = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeasePaymentToDate;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeaseInterestRate = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeaseInterestRate;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeaseResponsibleForAssets = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeaseResponsibleForAssets;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeaseTerm = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeaseTerm;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolLeases.FirstOrDefault().SchoolLeaseValueOfAssets = academisationApplication.Schools.FirstOrDefault().Leases.FirstOrDefault().SchoolLeaseValueOfAssets;

         academiesApplication.ApplyingSchools.FirstOrDefault().FinanceOngoingInvestigations = academisationApplication.Schools.FirstOrDefault().FinanceOngoingInvestigations;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolFinancialInvestigationsExplain = academisationApplication.Schools.FirstOrDefault().FinancialInvestigationsExplain;
         academiesApplication.ApplyingSchools.FirstOrDefault().SchoolFinancialInvestigationsTrustAware = academisationApplication.Schools.FirstOrDefault().FinancialInvestigationsTrustAware;

         academiesApplication.ApplyingSchools.FirstOrDefault().ProjectedPupilNumbersYear1 = academisationApplication.Schools.FirstOrDefault().ProjectedPupilNumbersYear1;
         academiesApplication.ApplyingSchools.FirstOrDefault().ProjectedPupilNumbersYear2 = academisationApplication.Schools.FirstOrDefault().ProjectedPupilNumbersYear2;
         academiesApplication.ApplyingSchools.FirstOrDefault().ProjectedPupilNumbersYear3 = academisationApplication.Schools.FirstOrDefault().ProjectedPupilNumbersYear3;






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
      public bool SchoolConversionTargetDateSpecified { get; set; }
      public DateTime SchoolConversionTargetDate { get; set; }
      public string SchoolConversionTargetDateExplained { get; set; }
      public bool ConversionChangeNamePlanned { get; set; }
      public string ProposedNewSchoolName { get; set; }
      public string ApplicationJoinTrustReason { get; set; }
      public int? ProjectedPupilNumbersYear1 { get; set; }
      public int? ProjectedPupilNumbersYear2 { get; set; }
      public int? ProjectedPupilNumbersYear3 { get; set; }
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