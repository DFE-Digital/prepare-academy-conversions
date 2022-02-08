using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	/// <remarks>
	/// should correspond to TramsDataApi.ResponseModels.ApplyToBecome.A2BApplyingSchoolResponse
	/// </remarks>

	public class ApplyingSchool
	{
		public string SchoolName { get; set; }
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
		public bool SchoolConversionTargetDateSpecified { get; set; } // int? SchoolConversionTargetDateDifferent 
		public DateTime? SchoolConversionTargetDate { get; set; } // SchoolConversionTargetDateDate
		public string SchoolConversionTargetDateExplained { get; set; }
		// reasons for joining
		public string SchoolConversionReasonsForJoining { get; set; }
		// name changes
		public bool SchoolConversionChangeNamePlanned { get; set; } // int? schoolConversionChangeName
		public string SchoolConversionProposedNewSchoolName { get; set; } // schoolConversionChangeNameValue
																		  // additional information
		public string SchoolAdSchoolContributionToTrust { get; set; }
		public bool SchoolOngoingSafeguardingInvestigations { get; set; } // int? SchoolAdSafeguarding
		public string SchoolOngoingSafeguardingDetails { get; set; } // SchoolAdSafeguardingExplained
		public bool SchoolPartOfLaReorganizationPlan { get; set; } // int? SchoolLaReorganization
		public string SchoolLaReorganizationDetails { get; set; }
		public bool SchoolPartOfLaClosurePlan { get; set; } //int? SchoolLaClosurePlans
		public string SchoolLaClosurePlanDetails { get; set; }
		public bool SchoolFaithSchool { get; set; } // int? SchoolFaithSchool
		public string SchoolFaithSchoolDioceseName { get; set; }
		public Link DiocesePermissionEvidenceDocument { get; set; }
		public bool SchoolIsPartOfFederation { get; set; } // SchoolPartOfFederation
		public bool SchoolIsSupportedByFoundation { get; set; } // SchoolSupportedFoundation
		public string SchoolSupportedFoundationBodyName { get; set; }
		public Link FoundationEvidenceDocument { get; set; }
		public bool SchoolHasSACREException { get; set; } // SchoolSACREExemption
		public DateTime SchoolSACREExcepionEndDate { get; set; }
		public string SchoolAdFeederSchools { get; set; }
		public Link GoverningBodyConsentEvidenceDocument { get; set; }
		public bool SchoolAdEqualitiesImpactAssessmentCompleted { get; set; } //SchoolAdEqualitiesImpactAssessment
		public string SchoolAdEqualitiesImpactAssessmentDetails { get; set; } // two possible very long proforma string answers here
		public bool SchoolAdInspectedButReportNotPublished { get; set; }
		public bool SchoolAdditionalInformationAdded { get; set; }
		public string SchoolAdditionalInformation { get; set; }
		// Finances
		public FinancialYear PreviousFinancialYear { get; set; }
		public FinancialYear CurrentFinancialYear { get; set; }
		public FinancialYear NextFinancialYear { get; set; }
		public List<Loan> ExistingLoans { get; set; }
		public List<Lease> ExistingLeases { get; set; }
		public bool FinanceOngoingInvestigations { get; set; } // SchoolFinancialInvestigations
		public string SchoolFinancialInvestigationsExplain { get; set; }
		public bool SchoolFinancialInvestigationsTrustAware { get; set; }
		// future pupil numbers
		public int SchoolCapacityYear1 { get; set; }
		public int SchoolCapacityYear2 { get; set; }
		public int SchoolCapacityYear3 { get; set; }
		public string SchoolCapacityAssumptions { get; set; }
		public int SchoolCapacityPublishedAdmissionsNumber { get; set; }
		// land and buildings
		public string SchoolBuildLandOwnerExplained { get; set; }
		public bool SchoolBuildLandWorksPlanned { get; set; }
		public string SchoolBuildLandWorksPlannedExplained { get; set; }
		public DateTime? SchoolBuildLandWorksPlannedCompletionDate { get; set; }
		public bool SchoolBuildLandSharedFacilities { get; set; }
		public string SchoolBuildLandSharedFacilitiesExplained { get; set; }
		public bool SchoolBuildLandGrants { get; set; }
		public string SchoolBuildLandGrantsExplained { get; set; }
		public bool SchoolBuildLandPFIScheme { get; set; }
		public string SchoolBuildLandPFISchemeType { get; set; }
		public bool SchoolBuildLandPriorityBuildingProgramme { get; set; }
		public bool SchoolBuildLandFutureProgramme { get; set; }
		// pre-opening support grant
		public string SchoolSupportGrantFundsPaidTo { get; set; } // int? - actually an enum "To the school" / "To the trust the school is joining"
																  // consultation details
		public bool SchoolHasConsultedStakeholders { get; set; } // SchoolConsultationStakeholdersConsult
		public string SchoolPlanToConsultStakeholders { get; set; }
		// declaration
		// ! make sure we get this data mapping correct and don't mix it up with the other declarations !
		public bool SchoolApplicantDeclarationIsApplicationCorrect { get; set; }
		// there are two more fields about the declarations here that aren't needed?
		// public bool? IAmTheChairOrHeadteacher
		// public bool? InfoIsTrueToBestOfMyKnowledge
		public string SchoolDeclarationSignedByName { get; set; } // this signed by has to be headteacher or chair of governors
	}
}

