using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	public class Lease
	{
		public string SchoolLeaseTerms { get; set; }
		public float SchoolLeaseRepaymentValue { get; set; }
		public float SchoolLeaseInterestRate { get; set; }
		public float SchoolLeasePaymentToDate { get; set; }
		public string SchoolLeasePurpose { get; set; }
		public float SchoolLeaseValueOfAssets { get; set; }
		public string SchoolLeaseResponsibilityForAssets { get; set; }
	}

	public class Loan
	{
		public float SchoolLoanAmount { get; set; }
		public string SchoolLoanPurpose { get; set; }
		public string SchoolLoanProvider { get; set; }
		public float SchoolLoanInterestRate { get; set; }
		public string SchoolLoanSchedule { get; set; }
	}

	public class FinancialYear
	{
		public DateTime FYEndDate { get; set; }
		public float RevenueCarryForward { get; set; }
		public string RevenueStatus { get; set; } // enum - "Surplus" / "Deficit"
		public string RevenueStatusExplained { get; set; }
		public Link RevenueRecoveryPlanEvidenceDocument { get; set; }
		public float CapitalCarryForward { get; set; }
		public string CapitalStatus { get; set; } // enum - "Surplus" / "Deficit"
		public string CapitalStatusExplained { get; set; }
		public Link CapitalRecoveryPlanEvidenceDocument { get; set; } // might be the same as the revenue document link?
	}

	public class SchoolApplication
	{
		public string ApplicationId { get; set; }
		public string FormTrustProposedNameOfTrust { get; set; }
		public string ApplicationLeadAuthorName { get; set; }
		public Link EvidenceDocument { get; set; }
		public bool? ChangesToTrust { get; set; }
		public string ChangesToTrustExplained { get; set; }
		public bool? ChangesToLAGovernance { get; set; }
		public string ChangesToLAGovernanceExplained { get; set; }
		// contact details
		public string SchoolConversionContactHeadName { get; set; }
		public string SchoolConversionContactHeadEmail { get; set; }
		public string SchoolConversionContactHeadTel { get; set; }
		public string SchoolConversionContactChairName { get; set; }
		public string SchoolConversionContactChairEmail { get; set; }
		public string SchoolConversionContactChairTel { get; set; }
		public string MainContactForApplication { get; set; } // enum "headteacher", "chair of governing body", "someone else"
		public string SchoolConversionMainContactOtherName { get; set; }
		public string SchoolConversionMainContactOtherEmail { get; set; }
		public string SchoolConversionMainContactOtherTelephone { get; set; }
		public string SchoolConversionMainContactOtherRole { get; set; }
		public string SchoolConversionApproverContactName { get; set; }
		public string SchoolConversionApproverContactEmail { get; set; }
		// conversion dates
		public bool? SchoolConversionTargetDateSpecified { get; set; }
		public DateTime? SchoolConversionTargetDateDate { get; set; }
		public string SchoolConversionTargetDateExplained { get; set; }
		// reasons for joining
		public string SchoolConversionReasonsForJoining { get; set; }
		// name changes
		public bool? SchoolConversionChangeNamePlanned { get; set; } // int? schoolConversionChangeName
		public string SchoolConversionProposedNewSchoolName { get; set; } // schoolConversionChangeNameValue
		// additional information
		public string SchoolAdSchoolContributionToTrust { get; set; }
		public bool? SchoolOngoingSafeguardingInvestigations { get; set; } // int? SchoolAdSafeguarding
		public string SchoolOngoingSafeguardingDetails { get; set; } // SchoolAdSafeguardingExplained
		public bool? SchoolPartOfLaReorganizationPlan { get; set; } // int? SchoolLaReorganization
		public string SchoolLaReorganizationDetails { get; set; }
		public bool? SchoolPartOfLaClosurePlan { get; set; } //int? SchoolLaClosurePlans
		public string SchoolLaClosurePlanDetails { get; set; }
		public bool? SchoolFaithSchool { get; set; } // int? SchoolFaithSchool
		public string SchoolFaithSchoolDioceseName { get; set; }
		public Link DiocesePermissionEvidenceDocument { get; set; }
		public bool? SchoolIsPartOfFederation { get; set; }
		public bool? SchoolIsSupportedByFoundation { get; set; }
		public string SchoolSupportedFoundationBodyName { get; set; }
		public Link FoundationEvidenceDocument { get; set; }
		public bool? SchoolHasSACREException { get; set; }
		public DateTime SchoolSACREExcepionEndDate { get; set; }
		public string SchoolAdFeederSchools { get; set; }
		public Link GoverningBodyConsentEvidenceDocument { get; set; }
		public bool? SchoolAdEqualitiesImpactAssessmentCompleted { get; set; }
		public string SchoolAdEqualitiesImpactAssessmentDetails { get; set; } // two possible very long string answers here
		public bool? SchoolAdditionalInformationAdded { get; set; } // not clear what this info is for better name
		public string SchoolAdditionalInformation { get; set; }
		// Finances
		public FinancialYear PreviousFinancialYear { get; set; }
		public FinancialYear CurrentFinancialYear { get; set; }
		public FinancialYear NextFinancialYear { get; set; }
		public List<Loan> ExistingLoans { get; set; }
		public List<Lease> ExistingLeases { get; set; }
		public bool? FinanceOngoingInvestigations { get; set; }
		public string SchoolFinancialInvestigationsExplain { get; set; }
		public bool? SchoolFinancialInvestigationsTrustAware { get; set; }
		// future pupil numbers
		public int SchoolCapacityYear1 { get; set; }
		public int SchoolCapacityYear2 { get; set; }
		public int SchoolCapacityYear3 { get; set; }
		public string SchoolCapacityAssumptions { get; set; }
		public int SchoolCapacityPublishedAdmissionsNumber { get; set; }
		// land and buildings
		public string SchoolBuildLandOwnerExplained { get; set; }
		public bool? SchoolBuildLandWorksPlanned { get; set; }
		public string SchoolBuildLandWorksPlannedExplained { get; set; }
		public DateTime? SchoolBuildLandWorksPlannedCompletionDate { get; set; }
		public bool? SchoolBuildLandSharedFacilities { get; set; }
		public string SchoolBuildLandSharedFacilitiesExplained { get; set; }
		public bool? SchoolBuildLandGrants { get; set; }
		public string SchoolBuildLandGrantsExplained { get; set; }
		public bool? SchoolBuildLandPFIScheme { get; set; }
		public string SchoolBuildLandPFISchemeType { get; set; }
		public bool? SchoolBuildLandPriorityBuildingProgramme { get; set; }
		public bool? SchoolBuildLandFutureProgramme { get; set; }
		// pre-opening support grant
		public string SchoolSupportGrantPaidTo { get; set; } // enum "To the school" / "To the trust the school is joining"
		// consultation details
		public bool? SchoolHasConsultedStakeholders { get; set; }
		public string SchoolPlanToConsultStakeholders { get; set; }
		// declaration
		// ! make sure we get this data mapping correct and don't mix it up with the other declarations !
		public bool? SchoolApplicantDeclarationIsApplicationCorrect { get; set; }
		// there are two more fields about the declarations here that aren't needed?
		// public bool? IAmTheChairOrHeadteacher
		// public bool? InfoIsTrueToBestOfMyKnowledge
		public string SignedByName { get; set; } // this signed by has to be headteacher or chair of governors
	}
}
