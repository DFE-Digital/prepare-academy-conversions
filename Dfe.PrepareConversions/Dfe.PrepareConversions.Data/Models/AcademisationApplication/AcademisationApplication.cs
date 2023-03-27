using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.Application;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication;

public class AcademisationApplication
{
   public int ApplicationId { get; set; }
   public string ApplicationType { get; set; }
   public string ApplicationStatus { get; set; }
   public string ApplicationReference { get; set; }
   public List<Contributor> Contributors { get; set; }
   public JoinTrustDetails JoinTrustDetails { get; set; }
   public FormTrustDetails FormTrustDetails { get; set; }

   public List<School> Schools { get; set; }

   public static Application.Application MapToApplication(AcademisationApplication academisationApplication)
   {
      // Following the fields used by the front end
      Application.Application academiesApplication = PopulateOverview(academisationApplication, out School academisationApplicationSchool, out ApplyingSchool academiesApplicationSchool);
      if(academiesApplication.ApplicationType.Equals(GlobalStrings.FormAMat)) PopulateFormAMatTrustInformation(academiesApplication, academisationApplication);
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

   private static void PopulateFormAMatTrustInformation(Application.Application academiesApplication, AcademisationApplication academisationApplication)
   {
      academiesApplication.FormTrustOpeningDate = academisationApplication.FormTrustDetails.FormTrustOpeningDate;
      academiesApplication.TrustApproverName = academisationApplication.FormTrustDetails.TrustApproverName;
      academiesApplication.TrustApproverEmail = academisationApplication.FormTrustDetails.TrustApproverEmail;
      academiesApplication.FormTrustReasonForming = academisationApplication.FormTrustDetails.FormTrustReasonForming;
      academiesApplication.FormTrustReasonVision = academisationApplication.FormTrustDetails.FormTrustReasonVision;
      academiesApplication.FormTrustReasonGeoAreas = academisationApplication.FormTrustDetails.FormTrustReasonGeoAreas;
      academiesApplication.FormTrustReasonFreedom = academisationApplication.FormTrustDetails.FormTrustReasonFreedom;
      academiesApplication.FormTrustReasonImproveTeaching = academisationApplication.FormTrustDetails.FormTrustReasonImproveTeaching;
      academiesApplication.FormTrustGrowthPlansYesNo = academisationApplication.FormTrustDetails.FormTrustGrowthPlansYesNo;
      academiesApplication.FormTrustPlanForGrowth = academisationApplication.FormTrustDetails.FormTrustPlanForGrowth;
      academiesApplication.FormTrustPlansForNoGrowth = academisationApplication.FormTrustDetails.FormTrustPlansForNoGrowth;
      academiesApplication.FormTrustImprovementSupport = academisationApplication.FormTrustDetails.FormTrustImprovementSupport;
      academiesApplication.FormTrustImprovementStrategy = academisationApplication.FormTrustDetails.FormTrustImprovementStrategy;
      academiesApplication.FormTrustImprovementApprovedSponsor = academisationApplication.FormTrustDetails.FormTrustImprovementApprovedSponsor;
      academiesApplication.KeyPeople = academisationApplication.FormTrustDetails.KeyPeople;

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
      academiesApplicationSchool.SchoolCapacityPublishedAdmissionsNumber = academisationApplicationSchool.SchoolCapacityPublishedAdmissionsNumber;
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
         foreach (Lease lease in academisationApplicationSchool.Leases)
         {
            Application.Lease newLease = new()
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
         foreach (Loan loan in academisationApplicationSchool.Loans)
         {
            Application.Loan newLoan = new()
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
      string deficit = "deficit";
      academiesApplicationSchool.PreviousFinancialYear = new Application.FinancialYear();
      academiesApplicationSchool.CurrentFinancialYear = new Application.FinancialYear();
      academiesApplicationSchool.NextFinancialYear = new Application.FinancialYear();
      academiesApplicationSchool.PreviousFinancialYear.FYEndDate =
         academisationApplicationSchool.PreviousFinancialYear.FinancialYearEndDate;
      academiesApplicationSchool.PreviousFinancialYear.RevenueCarryForward =
         (decimal?)academisationApplicationSchool.PreviousFinancialYear.Revenue;
      academiesApplicationSchool.PreviousFinancialYear.RevenueIsDeficit = academisationApplicationSchool.PreviousFinancialYear.RevenueStatus == deficit;
      academiesApplicationSchool.PreviousFinancialYear.RevenueStatusExplained =
         academisationApplicationSchool.PreviousFinancialYear.RevenueStatusExplained;
      academiesApplicationSchool.PreviousFinancialYear.CapitalCarryForward =
         (decimal?)academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForward;
      academiesApplicationSchool.PreviousFinancialYear.CapitalStatusExplained =
         academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardExplained;
      if (academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardStatus == deficit)
         academiesApplicationSchool.PreviousFinancialYear.CapitalIsDeficit = true;
      academiesApplicationSchool.PreviousFinancialYear.CapitalIsDeficit = academisationApplicationSchool.PreviousFinancialYear.CapitalCarryForwardStatus == deficit;

      academiesApplicationSchool.CurrentFinancialYear.FYEndDate =
         academisationApplicationSchool.CurrentFinancialYear.FinancialYearEndDate;
      academiesApplicationSchool.CurrentFinancialYear.RevenueCarryForward =
         (decimal?)academisationApplicationSchool.CurrentFinancialYear.Revenue;
      academiesApplicationSchool.CurrentFinancialYear.RevenueStatusExplained =
         academisationApplicationSchool.CurrentFinancialYear.RevenueStatusExplained;
      academiesApplicationSchool.CurrentFinancialYear.RevenueIsDeficit = academisationApplicationSchool.CurrentFinancialYear.RevenueStatus == deficit;
      academiesApplicationSchool.CurrentFinancialYear.CapitalCarryForward =
         (decimal?)academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForward;
      academiesApplicationSchool.CurrentFinancialYear.CapitalStatusExplained =
         academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForwardExplained;
      academiesApplicationSchool.CurrentFinancialYear.CapitalIsDeficit = academisationApplicationSchool.CurrentFinancialYear.CapitalCarryForwardStatus == deficit;

      academiesApplicationSchool.NextFinancialYear.FYEndDate =
         academisationApplicationSchool.NextFinancialYear.FinancialYearEndDate;
      academiesApplicationSchool.NextFinancialYear.RevenueCarryForward =
         (decimal?)academisationApplicationSchool.NextFinancialYear.Revenue;
      academiesApplicationSchool.NextFinancialYear.RevenueStatusExplained =
         academisationApplicationSchool.NextFinancialYear.RevenueStatusExplained;
      academiesApplicationSchool.NextFinancialYear.RevenueIsDeficit = academisationApplicationSchool.NextFinancialYear.RevenueStatus == deficit;
      academiesApplicationSchool.NextFinancialYear.CapitalCarryForward =
         (decimal?)academisationApplicationSchool.NextFinancialYear.CapitalCarryForward;
      academiesApplicationSchool.NextFinancialYear.CapitalStatusExplained =
         academisationApplicationSchool.NextFinancialYear.CapitalCarryForwardExplained;
      academiesApplicationSchool.NextFinancialYear.CapitalIsDeficit = academisationApplicationSchool.NextFinancialYear.CapitalCarryForwardStatus == deficit;
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
            .IsNullOrEmpty(); // Paul L - awaiting confirmation
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
         academiesApplicationSchool.SchoolHasSACREException = academisationApplicationSchool.ExemptionEndDate.HasValue;
      if (academisationApplicationSchool.ExemptionEndDate is not null)
         academiesApplicationSchool.SchoolSACREExemptionEndDate = academisationApplicationSchool.ExemptionEndDate.Value.DateTime;
      academiesApplicationSchool.SchoolAdFeederSchools = academisationApplicationSchool.MainFeederSchools;
      academiesApplicationSchool.SchoolAdEqualitiesImpactAssessmentCompleted =
         !academisationApplicationSchool.ProtectedCharacteristics.IsNullOrEmpty();
      academiesApplicationSchool.SchoolAdEqualitiesImpactAssessmentDetails =
         academisationApplicationSchool.ProtectedCharacteristics switch
         {
            "unlikely" => "That the Secretary of State's decision is unlikely to disproportionately affect any particular person or group who share protected characteristics",
            "willnot" =>
               "That there are some impacts but on balance the changes will not disproportionately affect any particular person or group who share protected characteristics",
            _ => string.Empty
         };
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

   public static Application.Application PopulateOverview(AcademisationApplication academisationApplication,
                                                          out School academisationApplicationSchool,
                                                          out ApplyingSchool academiesApplicationSchool)
   {
      Application.Application academiesApplication = new();
      academisationApplication!.Contributors ??= new List<Contributor>();
      academisationApplicationSchool = academisationApplication.Schools.FirstOrDefault();
      academiesApplication.ApplyingSchools = new List<ApplyingSchool> { new() };
      academiesApplicationSchool = academiesApplication.ApplyingSchools.FirstOrDefault();
      academiesApplicationSchool!.SchoolLoans = new List<Application.Loan>();
      academiesApplicationSchool!.SchoolLeases = new List<Application.Lease>();
      academiesApplication.TrustName = academisationApplication.ApplicationType switch
      {
         GlobalStrings.JoinAMat => academisationApplication.JoinTrustDetails.TrustName,
         GlobalStrings.FormAMat => academisationApplication.FormTrustDetails.FormTrustProposedNameOfTrust,
         _ => academiesApplication.TrustName
      };

      academiesApplication.ApplicationType = academisationApplication.ApplicationType;
      academiesApplication.ApplicationId =
         academisationApplication.ApplicationReference;
      Contributor academisationContributors = academisationApplication.Contributors.FirstOrDefault();
      if (academisationContributors?.FirstName.IsNullOrEmpty() is false && academisationContributors.LastName.IsNullOrEmpty() is false)
      {
         academiesApplication.ApplicationLeadAuthorName =
            academisationApplication.Contributors.FirstOrDefault()!.FirstName + " " + academisationApplication.Contributors.FirstOrDefault()!.LastName;
      }

      if (academiesApplication.ApplicationType.Equals(GlobalStrings.JoinAMat))
      {
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
      }
      
      
      return academiesApplication;
   }
}
