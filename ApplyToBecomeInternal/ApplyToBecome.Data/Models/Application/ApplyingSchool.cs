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
		public string SchoolConversionContactRole { get; set; } // "headteacher", "chair of governing body", "someone else"
		public string SchoolConversionMainContactOtherName { get; set; }
		public string SchoolConversionMainContactOtherEmail { get; set; }
		public string SchoolConversionMainContactOtherTelephone { get; set; }
		public string SchoolConversionMainContactOtherRole { get; set; }
		public string SchoolConversionApproverContactName { get; set; }
		public string SchoolConversionApproverContactEmail { get; set; }
		// conversion dates
		public bool? SchoolConversionTargetDateSpecified { get; set; }
		public DateTime? SchoolConversionTargetDate { get; set; }
		public string SchoolConversionTargetDateExplained { get; set; }
		// reasons for joining
		public string SchoolConversionReasonsForJoining { get; set; }
		// name changes
		public bool? SchoolConversionChangeNamePlanned { get; set; }
		public string SchoolConversionProposedNewSchoolName { get; set; }
		// additional information
		public string SchoolAdSchoolContributionToTrust { get; set; }
		public bool? SchoolOngoingSafeguardingInvestigations { get; set; }
		public string SchoolOngoingSafeguardingDetails { get; set; }
		public bool? SchoolPartOfLaReorganizationPlan { get; set; }
		public string SchoolLaReorganizationDetails { get; set; }
		public bool? SchoolPartOfLaClosurePlan { get; set; }
		public string SchoolLaClosurePlanDetails { get; set; }
		public bool? SchoolFaithSchool { get; set; }
		public string SchoolFaithSchoolDioceseName { get; set; }
		public string DiocesePermissionEvidenceDocumentLink { get; set; }
		public bool? SchoolIsPartOfFederation { get; set; }
		public bool? SchoolIsSupportedByFoundation { get; set; }
		public string SchoolSupportedFoundationBodyName { get; set; }
		public string FoundationEvidenceDocumentLink { get; set; }
		public bool? SchoolHasSACREException { get; set; }
		public DateTime? SchoolSACREExemptionEndDate { get; set; }
		public string SchoolAdFeederSchools { get; set; }
		public string GoverningBodyConsentEvidenceDocumentLink { get; set; }
		public bool? SchoolAdEqualitiesImpactAssessmentCompleted { get; set; }
		public string SchoolAdEqualitiesImpactAssessmentDetails { get; set; }
		public bool? SchoolAdInspectedButReportNotPublished { get; set; }
		public string SchoolAdInspectedButReportNotPublishedExplain { get; set; }
		public bool? SchoolAdditionalInformationAdded { get; set; }
		public string SchoolAdditionalInformation { get; set; }
		// Finances
		public FinancialYear PreviousFinancialYear { get; set; }
		public FinancialYear CurrentFinancialYear { get; set; }
		public FinancialYear NextFinancialYear { get; set; }
		public ICollection<Loan> SchoolLoans { get; set; }
		public ICollection<Lease> SchoolLeases { get; set; }
		public bool? FinanceOngoingInvestigations { get; set; }
		public string SchoolFinancialInvestigationsExplain { get; set; }
		public bool? SchoolFinancialInvestigationsTrustAware { get; set; }
		// future pupil numbers
		public int? ProjectedPupilNumbersYear1 { get; set; }
		public int? ProjectedPupilNumbersYear2 { get; set; }
		public int? ProjectedPupilNumbersYear3 { get; set; }
		public string SchoolCapacityAssumptions { get; set; }
		public int? SchoolCapacityPublishedAdmissionsNumber { get; set; }
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
		public string SchoolSupportGrantFundsPaidTo { get; set; } // either "To the school" or "To the trust the school is joining"
		// consultation details
		public bool? SchoolHasConsultedStakeholders { get; set; }
		public string SchoolPlanToConsultStakeholders { get; set; }
		// declaration
		// two questions from the application form would be easy to mix up here
		// 1. I agree with all of these statements, and belive that the facts stated in this application are true (summary page)
		// 2. The information in this application is true to the best of my kowledge (actual question)
		public bool? DeclarationBodyAgree { get; set; }
		public bool? DeclarationIAmTheChairOrHeadteacher { get; set; }
		public string DeclarationSignedByName { get; set; }
	}
}

