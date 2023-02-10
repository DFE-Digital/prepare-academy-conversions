using Dfe.PrepareConversions.Data.Models.Application;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
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

      public static void PopulateDeclaration(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.DeclarationBodyAgree = academisationApplicationSchool.DeclarationBodyAgree;
         academiesApplicationSchool.DeclarationSignedByName = academisationApplicationSchool.DeclarationSignedByName;
      }

      public static void PopulateConsultation(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.SchoolHasConsultedStakeholders =
            academisationApplicationSchool.SchoolHasConsultedStakeholders;
         academiesApplicationSchool.SchoolPlanToConsultStakeholders =
            academisationApplicationSchool.SchoolPlanToConsultStakeholders;
      }

      public static void PopulateLandAndBuildings(ApplyingSchool academiesApplicationSchool,
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

      public static void PopulateFuturePupilNumbers(ApplyingSchool academiesApplicationSchool,
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

      public static void PopulateFinancialInvestigations(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         academiesApplicationSchool.FinanceOngoingInvestigations =
            academisationApplicationSchool.FinanceOngoingInvestigations;
         academiesApplicationSchool.SchoolFinancialInvestigationsExplain =
            academisationApplicationSchool.FinancialInvestigationsExplain;
         academiesApplicationSchool.SchoolFinancialInvestigationsTrustAware =
            academisationApplicationSchool.FinancialInvestigationsTrustAware;
      }

      public static void PopulateSchoolLeases(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         if (academisationApplicationSchool.Leases.IsNullOrEmpty() is false)
         {
            foreach (var lease in academisationApplicationSchool.Leases)
            {
               var newLease = new Lease()
               {
                  SchoolLeasePurpose = lease.Purpose,
                  SchoolLeaseRepaymentValue = lease.RepaymentAmount,
                  SchoolLeaseInterestRate = lease.InterestRate,
                  SchoolLeasePaymentToDate = lease.PaymentsToDate,
                  SchoolLeaseResponsibleForAssets = lease.ResponsibleForAssets,
                  SchoolLeaseValueOfAssets = lease.ValueOfAssets,
                  SchoolLeaseTerm = lease.LeaseTerm
               };
               academiesApplicationSchool.SchoolLeases.Add(newLease);
            }
         }
      }

      public static void PopulateSchoolLoans(ApplyingSchool academiesApplicationSchool,
         School academisationApplicationSchool)
      {
         if (academisationApplicationSchool.Loans.IsNullOrEmpty() is false)
         {
            foreach (var loan in academisationApplicationSchool.Loans)
            {
               var newLoan = new Loan
               {
                  SchoolLoanAmount = loan.Amount,
                  SchoolLoanPurpose = loan.Purpose,
                  SchoolLoanProvider = loan.Provider,
                  SchoolLoanInterestRate = loan.InterestRate.ToString(CultureInfo.InvariantCulture),
                  SchoolLoanSchedule = loan.Schedule
               };
               academiesApplicationSchool.SchoolLoans.Add(newLoan);
            }
         }
      }

      public static void PopulateSchoolFinances(ApplyingSchool academiesApplicationSchool,
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
         academiesApplicationSchool.PreviousFinancialYear.RevenueIsDeficit = academisationApplicationSchool.PreviousFinancialYear.RevenueStatus == "Deficit";
         academiesApplicationSchool.PreviousFinancialYear.RevenueStatusExplained =
            academisationApplicationSchool.PreviousFinancialYear.RevenueStatusExplained;
         academiesApplicationSchool.PreviousFinancialYear.CapitalCarryForward =
            (decimal?)academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForward;
         academiesApplicationSchool.PreviousFinancialYear.CapitalStatusExplained =
            academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardExplained;
         if (academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardStatus == "Deficit")
            academiesApplicationSchool.PreviousFinancialYear.CapitalIsDeficit = true;
         academiesApplicationSchool.PreviousFinancialYear.CapitalIsDeficit = academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardStatus == "Deficit";

         academiesApplicationSchool.CurrentFinancialYear.FYEndDate =
            academisationApplicationSchool.CurrentFinancialYear.FinancialYearEndDate;
         academiesApplicationSchool.CurrentFinancialYear.RevenueCarryForward =
            (decimal?)academisationApplicationSchool.CurrentFinancialYear.Revenue;
         academiesApplicationSchool.CurrentFinancialYear.RevenueStatusExplained =
            academisationApplicationSchool.CurrentFinancialYear.RevenueStatusExplained;
         academiesApplicationSchool.CurrentFinancialYear.RevenueIsDeficit = academisationApplicationSchool.CurrentFinancialYear.RevenueStatus == "Deficit";
         academiesApplicationSchool.CurrentFinancialYear.CapitalCarryForward =
            (decimal?)academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForward;
         academiesApplicationSchool.CurrentFinancialYear.CapitalStatusExplained =
            academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForwardExplained;
         academiesApplicationSchool.CurrentFinancialYear.CapitalIsDeficit = academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForwardStatus == "Deficit";

         academiesApplicationSchool.NextFinancialYear.FYEndDate =
            academisationApplicationSchool.NextFinancialYear.FinancialYearEndDate;
         academiesApplicationSchool.NextFinancialYear.RevenueCarryForward =
            (decimal?)academisationApplicationSchool.NextFinancialYear.Revenue;
         academiesApplicationSchool.NextFinancialYear.RevenueStatusExplained =
            academisationApplicationSchool.NextFinancialYear.RevenueStatusExplained;
         academiesApplicationSchool.NextFinancialYear.RevenueIsDeficit = academisationApplicationSchool.NextFinancialYear.RevenueStatus == "Deficit";
         academiesApplicationSchool.NextFinancialYear.CapitalCarryForward =
            (decimal?)academisationApplicationSchool.NextFinancialYear.CapitalCarryForward;
         academiesApplicationSchool.NextFinancialYear.CapitalStatusExplained =
            academisationApplicationSchool.NextFinancialYear.CapitalCarryForwardExplained;
         academiesApplicationSchool.NextFinancialYear.CapitalIsDeficit = academisationApplicationSchool.NextFinancialYear.CapitalCarryForwardStatus == "Deficit";
      }

      public static void PopulateFurtherInformation(ApplyingSchool academiesApplicationSchool,
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

      public static void PopulateSchoolDetails(ApplyingSchool academiesApplicationSchool,
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

      public static Application PopulateOverview(AcademisationApplication academisationApplication,
         out School academisationApplicationSchool, out ApplyingSchool academiesApplicationSchool)
      {
         var academiesApplication = new Application();
         academisationApplication!.Contributors = new List<Contributor>();
         academisationApplicationSchool = academisationApplication.Schools.FirstOrDefault();
         academiesApplication.ApplyingSchools = new List<ApplyingSchool>(){ new() };
         academiesApplicationSchool = academiesApplication.ApplyingSchools.FirstOrDefault();
         academiesApplicationSchool!.SchoolLoans = new List<Loan>();
         academiesApplicationSchool!.SchoolLeases = new List<Lease>();
         academiesApplication.TrustName = academisationApplication.JoinTrustDetails.TrustName;
         academiesApplication.ApplicationType = academisationApplication.ApplicationType;
         academiesApplication.ApplicationId =
            academisationApplication.ApplicationReference;
         var academisationContributors = academisationApplication.Contributors.FirstOrDefault();
         if (academisationContributors?.FirstName.IsNullOrEmpty() is false && academisationContributors.LastName.IsNullOrEmpty() is false)
         {
            academiesApplication.ApplicationLeadAuthorName =
               academisationApplication.Contributors.FirstOrDefault()!.FirstName + " " + academisationApplication.Contributors.FirstOrDefault()!.LastName;
         }
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
}