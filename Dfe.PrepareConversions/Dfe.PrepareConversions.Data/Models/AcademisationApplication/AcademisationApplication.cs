
using Dfe.PrepareConversions.Data.Models.Application;
using Microsoft.IdentityModel.Tokens;
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
      public JoinTrustDetails JoinTrustDetails { get; set; }

      public List<School> Schools { get; set; }

      public static Application MapToApplication(AcademisationApplication academisationApplication)
      {
         // Following the fields used by the front end
         Application academiesApplication = PopulateOverview(academisationApplication, out School academisationApplicationSchool, out ApplyingSchool academiesApplicationSchool);
         PopulateSchoolDetails(academiesApplicationSchool, academisationApplicationSchool);
         PopulateFurtherInformation(academiesApplicationSchool, academisationApplicationSchool);
         PopulateSchoolFinances(academiesApplicationSchool, academisationApplicationSchool);
         PopulateSchoolLoans(academiesApplicationSchool, academisationApplicationSchool);
         PopulateSchoolLeases(academiesApplicationSchool, academisationApplicationSchool);
         PopulateFinancialInvestigations(academiesApplicationSchool, academisationApplicationSchool);
         PopulateFuturePupilNumbers(academiesApplicationSchool, academisationApplicationSchool);
         PopulateLandAndBuildings(academiesApplicationSchool, academisationApplicationSchool);
         PopulateConsultation(academiesApplicationSchool, academisationApplicationSchool);
         PopulateDeclaration(academiesApplicationSchool, academisationApplicationSchool);


         return academiesApplication;
      }

      private static void PopulateDeclaration(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.DeclarationBodyAgree = academisationApplicationSchool.DeclarationBodyAgree;
         academiesApplicationSchool.DeclarationSignedByName = academisationApplicationSchool.DeclarationSignedByName;
      }

      private static void PopulateConsultation(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.SchoolHasConsultedStakeholders =
            academisationApplicationSchool.SchoolHasConsultedStakeholders;
         academiesApplicationSchool.SchoolPlanToConsultStakeholders =
            academisationApplicationSchool.SchoolPlanToConsultStakeholders;
      }

      private static void PopulateLandAndBuildings(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.SchoolBuildLandOwnerExplained =
            academisationApplicationSchool.LandAndBuildings.OwnerExplained;
         academiesApplicationSchool.SchoolBuildLandWorksPlanned =
            academisationApplicationSchool.LandAndBuildings.WorksPlanned;
         academiesApplicationSchool.SchoolBuildLandWorksPlannedExplained =
            academisationApplicationSchool.LandAndBuildings.WorksPlannedExplained;
         academiesApplicationSchool.SchoolBuildLandWorksPlannedCompletionDate =
            academisationApplicationSchool.LandAndBuildings.WorksPlannedDate;
         academiesApplicationSchool.SchoolBuildLandSharedFacilities =
            academisationApplicationSchool.LandAndBuildings.FacilitiesShared;
         academiesApplicationSchool.SchoolBuildLandSharedFacilitiesExplained =
            academisationApplicationSchool.LandAndBuildings.FacilitiesSharedExplained;
         academiesApplicationSchool.SchoolBuildLandGrants = academisationApplicationSchool.LandAndBuildings.Grants;
         academiesApplicationSchool.SchoolBuildLandGrantsExplained =
            academisationApplicationSchool.LandAndBuildings.GrantsAwardingBodies; // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolBuildLandPFIScheme =
            academisationApplicationSchool.LandAndBuildings.PartOfPfiScheme;
         academiesApplicationSchool.SchoolBuildLandPFISchemeType =
            academisationApplicationSchool.LandAndBuildings.PartOfPfiSchemeType;
         academiesApplicationSchool.SchoolBuildLandPriorityBuildingProgramme =
            academisationApplicationSchool.LandAndBuildings.PartOfPrioritySchoolsBuildingProgramme;
         academiesApplicationSchool.SchoolBuildLandFutureProgramme =
            academisationApplicationSchool.LandAndBuildings.PartOfBuildingSchoolsForFutureProgramme;

         academiesApplicationSchool.SchoolSupportGrantFundsPaidTo =
            academisationApplicationSchool.SchoolSupportGrantFundsPaidTo;
      }

      private static void PopulateFuturePupilNumbers(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.ProjectedPupilNumbersYear1 = academisationApplicationSchool.ProjectedPupilNumbersYear1;
         academiesApplicationSchool.ProjectedPupilNumbersYear2 = academisationApplicationSchool.ProjectedPupilNumbersYear2;
         academiesApplicationSchool.ProjectedPupilNumbersYear3 = academisationApplicationSchool.ProjectedPupilNumbersYear3;
         academiesApplicationSchool.SchoolCapacityAssumptions = academisationApplicationSchool.SchoolCapacityAssumptions;
         academiesApplicationSchool.SchoolCapacityPublishedAdmissionsNumber =
            Convert.ToInt32(academisationApplicationSchool
               .SchoolCapacityPublishedAdmissionNumber);
      }

      private static void PopulateFinancialInvestigations(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.FinanceOngoingInvestigations =
            academisationApplicationSchool.FinanceOngoingInvestigations;
         academiesApplicationSchool.SchoolFinancialInvestigationsExplain =
            academisationApplicationSchool.FinancialInvestigationsExplain;
         academiesApplicationSchool.SchoolFinancialInvestigationsTrustAware =
            academisationApplicationSchool.FinancialInvestigationsTrustAware;
      }

      private static void PopulateSchoolLeases(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         if (academiesApplicationSchool.SchoolLeases.IsNullOrEmpty() is false)
         {
            foreach (var lease in academisationApplicationSchool.Leases)
            {
               var newLease = new Lease()
               {
                  SchoolLeasePurpose = lease.SchoolLeasePurpose,
                  SchoolLeaseRepaymentValue = lease.SchoolLeaseRepaymentValue,
                  SchoolLeaseInterestRate = lease.SchoolLeaseInterestRate,
                  SchoolLeasePaymentToDate = lease.SchoolLeasePaymentToDate,
                  SchoolLeaseResponsibleForAssets = lease.SchoolLeasePurpose,
                  SchoolLeaseValueOfAssets = lease.SchoolLeaseValueOfAssets,
                  SchoolLeaseTerm = lease.SchoolLeaseTerm
               };
               academiesApplicationSchool.SchoolLeases.Add(newLease);
            }
         }
      }

      private static void PopulateSchoolLoans(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         if (academiesApplicationSchool.SchoolLoans.IsNullOrEmpty() is false)
         {
            foreach (var loan in academisationApplicationSchool.Loans)
            {
               var newLoan = new Loan
               {
                  SchoolLoanAmount = loan.SchoolLoanAmount,
                  SchoolLoanPurpose = loan.SchoolLoanPurpose,
                  SchoolLoanProvider = loan.SchoolLoanProvider,
                  SchoolLoanInterestRate = loan.SchoolLoanInterestRate,
                  SchoolLoanSchedule = loan.SchoolLoanSchedule
               };
               academiesApplicationSchool.SchoolLoans.Add(newLoan);
            }
         }
      }

      private static void PopulateSchoolFinances(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         // FINANCES
         academiesApplicationSchool.PreviousFinancialYear = new Dfe.PrepareConversions.Data.Models.Application.FinancialYear();
         academiesApplicationSchool.CurrentFinancialYear = new Dfe.PrepareConversions.Data.Models.Application.FinancialYear();
         academiesApplicationSchool.NextFinancialYear = new Dfe.PrepareConversions.Data.Models.Application.FinancialYear();
         academiesApplicationSchool.PreviousFinancialYear.FYEndDate =
            academisationApplicationSchool.PreviousFinancialYear.FinancialYearEndDate;
         academiesApplicationSchool.PreviousFinancialYear.RevenueCarryForward =
            (decimal?)academisationApplicationSchool.PreviousFinancialYear.Revenue;
         if (academisationApplicationSchool.PreviousFinancialYear.RevenueStatus == "Deficit")
            academiesApplicationSchool.PreviousFinancialYear.RevenueIsDeficit = true;
         academiesApplicationSchool.PreviousFinancialYear.RevenueStatusExplained =
            academisationApplicationSchool.PreviousFinancialYear.RevenueStatusExplained;
         academiesApplicationSchool.PreviousFinancialYear.CapitalCarryForward =
            (decimal?)academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForward;
         academiesApplicationSchool.PreviousFinancialYear.CapitalStatusExplained =
            academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardExplained;
         if (academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardStatus == "Deficit")
            academiesApplicationSchool.PreviousFinancialYear.CapitalIsDeficit = true;
         academiesApplicationSchool.CurrentFinancialYear.FYEndDate =
            academisationApplicationSchool.CurrentFinancialYear.FinancialYearEndDate;
         academiesApplicationSchool.CurrentFinancialYear.RevenueCarryForward =
            (decimal?)academisationApplicationSchool.CurrentFinancialYear.Revenue;
         academiesApplicationSchool.CurrentFinancialYear.RevenueStatusExplained =
            academisationApplicationSchool.CurrentFinancialYear.RevenueStatusExplained;
         if (academisationApplicationSchool.CurrentFinancialYear.RevenueStatus == "Deficit")
            academiesApplicationSchool.CurrentFinancialYear.RevenueIsDeficit = true;
         academiesApplicationSchool.CurrentFinancialYear.CapitalCarryForward =
            (decimal?)academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForward;
         academiesApplicationSchool.CurrentFinancialYear.CapitalStatusExplained =
            academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForwardExplained;
         if (academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForwardStatus == "Deficit")
            academiesApplicationSchool.CurrentFinancialYear.CapitalIsDeficit = true;

         academiesApplicationSchool.NextFinancialYear.FYEndDate =
            academisationApplicationSchool.NextFinancialYear.FinancialYearEndDate;
         academiesApplicationSchool.NextFinancialYear.RevenueCarryForward =
            (decimal?)academisationApplicationSchool.NextFinancialYear.Revenue;
         academiesApplicationSchool.NextFinancialYear.RevenueStatusExplained =
            academisationApplicationSchool.NextFinancialYear.RevenueStatusExplained;
         if (academisationApplicationSchool.NextFinancialYear.RevenueStatus == "Deficit")
            academiesApplicationSchool.NextFinancialYear.RevenueIsDeficit = true;
         academiesApplicationSchool.NextFinancialYear.CapitalCarryForward =
            (decimal?)academisationApplicationSchool.NextFinancialYear.CapitalCarryForward;
         academiesApplicationSchool.NextFinancialYear.CapitalStatusExplained =
            academisationApplicationSchool.NextFinancialYear.CapitalCarryForwardExplained;
         if (academisationApplicationSchool.NextFinancialYear.CapitalCarryForwardStatus == "Deficit")
            academiesApplicationSchool.NextFinancialYear.CapitalIsDeficit = true;
      }

      private static void PopulateFurtherInformation(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         // Further Information
         academiesApplicationSchool.SchoolAdSchoolContributionToTrust =
            academisationApplicationSchool
               .TrustBenefitDetails; // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolAdInspectedButReportNotPublished = !academisationApplicationSchool.OfstedInspectionDetails.IsNullOrEmpty(); // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolAdInspectedButReportNotPublishedExplain =
            academisationApplicationSchool.OfstedInspectionDetails;
         academiesApplicationSchool.SchoolOngoingSafeguardingInvestigations =
            !academisationApplicationSchool.SafeguardingDetails
               .IsNullOrEmpty(); // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolOngoingSafeguardingDetails = academisationApplicationSchool.SafeguardingDetails;
         academiesApplicationSchool.SchoolPartOfLaReorganizationPlan =
            !academisationApplicationSchool.LocalAuthorityReorganisationDetails
               .IsNullOrEmpty();// Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolLaReorganizationDetails =
            academisationApplicationSchool.LocalAuthorityReorganisationDetails;
         academiesApplicationSchool.SchoolPartOfLaClosurePlan =
            !academisationApplicationSchool.LocalAuthorityClosurePlanDetails.IsNullOrEmpty();
         academiesApplicationSchool.SchoolLaClosurePlanDetails =
            academisationApplicationSchool
               .LocalAuthorityClosurePlanDetails; // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolFaithSchool =
            !academisationApplicationSchool.DioceseName
               .IsNullOrEmpty(); // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolFaithSchoolDioceseName = academisationApplicationSchool.DioceseName;
         academiesApplicationSchool.SchoolIsPartOfFederation = academisationApplicationSchool.PartOfFederation;
         academiesApplicationSchool.SchoolIsSupportedByFoundation =
            !academisationApplicationSchool.FoundationTrustOrBodyName.IsNullOrEmpty(); // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolSupportedFoundationBodyName =
            academisationApplicationSchool.FoundationTrustOrBodyName;
         if (academisationApplicationSchool.ExemptionEndDate is not null)
            academiesApplicationSchool.SchoolHasSACREException = true; // Paul L - awaiting confirmation
         if (academisationApplicationSchool.ExemptionEndDate is not null) academiesApplicationSchool.SchoolSACREExemptionEndDate = academisationApplicationSchool.ExemptionEndDate.Value.DateTime; // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolAdFeederSchools = academisationApplicationSchool.MainFeederSchools;
         academiesApplicationSchool.SchoolAdEqualitiesImpactAssessmentCompleted =
            !academisationApplicationSchool.ResolutionConsentFolderIdentifier.IsNullOrEmpty(); // Paul L - awaiting confirmation 
         academiesApplicationSchool.SchoolAdEqualitiesImpactAssessmentDetails =
            academisationApplicationSchool.ResolutionConsentFolderIdentifier; // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolAdditionalInformationAdded =
            !academisationApplicationSchool.FurtherInformation.IsNullOrEmpty(); // Paul L - awaiting confirmation
         academiesApplicationSchool.SchoolAdditionalInformation =
            academisationApplicationSchool.FurtherInformation; // Paul L - awaiting confirmation
      }

      private static void PopulateSchoolDetails(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         // School
         academiesApplicationSchool.SchoolName = academisationApplicationSchool.SchoolName;
         academiesApplicationSchool.SchoolConversionContactHeadName =
            academisationApplicationSchool.SchoolConversionContactHeadName;
         academiesApplicationSchool.SchoolConversionContactHeadEmail =
            academisationApplicationSchool.SchoolConversionContactHeadEmail;
         academiesApplicationSchool.SchoolConversionContactHeadTel =
            academisationApplicationSchool.SchoolConversionContactHeadTel;
         academiesApplicationSchool.SchoolConversionContactRole =
            academisationApplicationSchool.SchoolConversionContactRole;
         academiesApplicationSchool.SchoolConversionContactChairName =
            academisationApplicationSchool.SchoolConversionContactChairName;
         academiesApplicationSchool.SchoolConversionContactChairEmail =
            academisationApplicationSchool.SchoolConversionContactChairEmail;
         academiesApplicationSchool.SchoolConversionContactChairTel =
            academisationApplicationSchool.SchoolConversionContactChairTel;
         academiesApplicationSchool.SchoolConversionMainContactOtherName =
            academisationApplicationSchool.SchoolConversionMainContactOtherName;
         academiesApplicationSchool.SchoolConversionMainContactOtherEmail =
            academisationApplicationSchool.SchoolConversionMainContactOtherEmail;
         academiesApplicationSchool.SchoolConversionMainContactOtherTelephone =
            academisationApplicationSchool.SchoolConversionMainContactOtherTelephone;
         academiesApplicationSchool.SchoolConversionApproverContactName =
            academisationApplicationSchool.SchoolConversionApproverContactName;
         academiesApplicationSchool.SchoolConversionApproverContactEmail =
            academisationApplicationSchool.SchoolConversionApproverContactEmail;
         academiesApplicationSchool.SchoolConversionTargetDateSpecified =
            academisationApplicationSchool.SchoolConversionTargetDateSpecified;
         academiesApplicationSchool.SchoolConversionTargetDate = academisationApplicationSchool.SchoolConversionTargetDate;
         academiesApplicationSchool.SchoolConversionTargetDateExplained =
            academisationApplicationSchool.SchoolConversionTargetDateExplained;
         academiesApplicationSchool.SchoolConversionReasonsForJoining =
            academisationApplicationSchool.ApplicationJoinTrustReason;
         academiesApplicationSchool.SchoolConversionChangeNamePlanned =
            academisationApplicationSchool.ConversionChangeNamePlanned;
         academiesApplicationSchool.SchoolConversionProposedNewSchoolName =
            academisationApplicationSchool.ProposedNewSchoolName;
      }

      private static Application PopulateOverview(AcademisationApplication academisationApplication,
         out School academisationApplicationSchool, out ApplyingSchool academiesApplicationSchool)
      {
         var academiesApplication = new Application();
         academisationApplicationSchool = academisationApplication.Schools.FirstOrDefault();
         academiesApplication.ApplyingSchools = new List<ApplyingSchool>(){ new() };
         academiesApplicationSchool = academiesApplication.ApplyingSchools.FirstOrDefault();
         academiesApplication.TrustName = academisationApplication.JoinTrustDetails.TrustName;
         academiesApplication.ApplicationType = academisationApplication.ApplicationType;
         academiesApplication.ApplicationId =
            academisationApplication.ApplicationReference;
         //academiesApplication.ApplicationLeadAuthorName = academisationApplication.Contributors (we have contributors on academisation but how do we know their the lead) // Paul L - awaiting confirmation
         academiesApplication.ChangesToTrust = academisationApplication.JoinTrustDetails.ChangesToTrust switch
         {
            "yes" => true,
            "no" => false,
            _ => academiesApplication.ChangesToTrust
         };
         academiesApplication.ChangesToTrustExplained = academisationApplication.JoinTrustDetails.ChangesToTrustExplained;
         academiesApplication.ChangesToLaGovernance = academisationApplication.JoinTrustDetails.ChangesToLaGovernance;
         academiesApplication.ChangesToLaGovernanceExplained =
            academisationApplication.JoinTrustDetails.ChangesToLaGovernanceExplained;
         return academiesApplication;
      }
   }

   public class JoinTrustDetails
   {
      public int UKPRN { get; set; }

      public string TrustName { get; set; }

      public string ChangesToTrust { get; set; }

      public string ChangesToTrustExplained { get; set; }

      public bool? ChangesToLaGovernance { get; set; }

      public string ChangesToLaGovernanceExplained { get; set; }
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
      public DateTimeOffset? ExemptionEndDate { get; set; }
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
      public string? PartOfPfiSchemeType { get; init; }
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