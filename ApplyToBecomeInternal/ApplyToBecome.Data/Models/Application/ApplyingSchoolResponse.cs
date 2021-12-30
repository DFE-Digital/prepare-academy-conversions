using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	/// <remarks>
	/// Copy of TramsDataApi.ResponseModels.ApplyToBecome.A2BApplyingSchoolResponse
	/// </remarks>

	public class ApplyingSchoolResponse
	{
		public string ApplyingSchoolId { get; set; }
		public string UpdatedTrustFields { get; set; }
		public string SchoolDeclarationSignedById { get; set; }
		public A2BSelectOption SchoolDeclarationBodyAgree { get; set; }
		public A2BSelectOption SchoolDeclarationTeacherChair { get; set; }
		public string SchoolDeclarationSignedByEmail { get; set; }
		public string Name { get; set; }
		public string UpdatedSchoolFields { get; set; }
		public string SchoolConversionReasonsForJoining { get; set; }
		public int? SchoolConversionTargetDateDifferent { get; set; }
		public DateTime? SchoolConversionTargetDateDate { get; set; }
		public string SchoolConversionTargetDateExplained { get; set; }
		public int? SchoolConversionChangeName { get; set; }
		public string SchoolConversionChangeNameValue { get; set; }
		public string SchoolConversionContactHeadName { get; set; }
		public string SchoolConversionContactHeadEmail { get; set; }
		public string SchoolConversionContactHeadTel { get; set; }
		public string SchoolConversionContactChairName { get; set; }
		public string SchoolConversionContactChairEmail { get; set; }
		public string SchoolConversionContactChairTel { get; set; }
		public int? SchoolConversionMainContact { get; set; }
		public string SchoolConversionMainContactOtherName { get; set; }
		public string SchoolConversionMainContactOtherEmail { get; set; }
		public string SchoolConversionMainContactOtherTelephone { get; set; }
		public string SchoolConversionMainContactOtherRole { get; set; }
		public string SchoolConversionApproverContactName { get; set; }
		public string SchoolConversionApproverContactEmail { get; set; }
		public int? SchoolAdInspectedButReportNotPublished { get; set; }
		public string SchoolAdInspectedReportNotPublishedExplain { get; set; }
		public int? SchoolLaReorganization { get; set; }
		public string SchoolLaReorganizationExplain { get; set; }
		public int? SchoolLaClosurePlans { get; set; }
		public string SchoolLaClosurePlansExplain { get; set; }
		public int? SchoolPartOfFederation { get; set; }
		public int? SchoolAddFurtherInformation { get; set; }
		public string SchoolFurtherInformation { get; set; }
		public string SchoolAdSchoolContributionToTrust { get; set; }
		public int? SchoolAdSafeguarding { get; set; }
		public string SchoolAdSafeguardingExplained { get; set; }
		public int? SchoolSACREExemption { get; set; }
		public DateTime? SchoolSACREExemptionEndDate { get; set; }
		public int? SchoolFaithSchool { get; set; }
		public string SchoolFaithSchoolDioceseName { get; set; }
		public int? SchoolSupportedFoundation { get; set; }
		public string SchoolSupportedFoundationBodyName { get; set; }
		public string SchoolAdFeederSchools { get; set; }
		public int? SchoolAdEqualitiesImpactAssessment { get; set; }
		public double? SchoolPFYRevenue { get; set; }
		public int? SchoolPFYRevenueStatus { get; set; }
		public string SchoolPFYRevenueStatusExplained { get; set; }
		public double? SchoolPFYCapitalForward { get; set; }
		public int? SchoolPFYCapitalForwardStatus { get; set; }
		public string SchoolPFYCapitalForwardStatusExplained { get; set; }
		public double? SchoolCFYRevenue { get; set; }
		public int? SchoolCFYRevenueStatus { get; set; }
		public string SchoolCFYRevenueStatusExplained { get; set; }
		public double? SchoolCFYCapitalForward { get; set; }
		public int? SchoolCFYCapitalForwardStatus { get; set; }
		public string SchoolCFYCapitalForwardStatusExplained { get; set; }
		public double? SchoolNFYRevenue { get; set; }
		public int? SchoolNFYRevenueStatus { get; set; }
		public string SchoolNFYRevenueStatusExplained { get; set; }
		public double? SchoolNFYCapitalForward { get; set; }
		public int? SchoolNFYCapitalForwardStatus { get; set; }
		public string SchoolNFYCapitalForwardStatusExplained { get; set; }
		public int? SchoolFinancialInvestigations { get; set; }
		public string SchoolFinancialInvestigationsExplain { get; set; }
		public int? SchoolFinancialInvestigationsTrustAware { get; set; }
		public DateTime? SchoolNFYEndDate { get; set; }
		public DateTime? SchoolPFYEndDate { get; set; }
		public DateTime? SchoolCFYEndDate { get; set; }
		public A2BSelectOption SchoolLoanExists { get; set; }
		public A2BSelectOption SchoolLeaseExists { get; set; }
		public int? SchoolCapacityYear1 { get; set; }
		public int? SchoolCapacityYear2 { get; set; }
		public int? SchoolCapacityYear3 { get; set; }
		public string SchoolCapacityAssumptions { get; set; }
		public string SchoolCapacityPublishedAdmissionsNumber { get; set; }
		public string SchoolBuildLandOwnerExplained { get; set; }
		public int? SchoolBuildLandSharedFacilities { get; set; }
		public string SchoolBuildLandSharedFacilitiesExplained { get; set; }
		public int? SchoolBuildLandWorksPlanned { get; set; }
		public string SchoolBuildLandWorksPlannedExplained { get; set; }
		public DateTime? SchoolBuildLandWorksPlannedDate { get; set; }
		public int? SchoolBuildLandGrants { get; set; }
		public string SchoolBuildLandGrantsBody { get; set; }
		public int? SchoolBuildLandPriorityBuildingProgramme { get; set; }
		public int? SchoolBuildLandFutureProgramme { get; set; }
		public int? SchoolBuildLandPFIScheme { get; set; }
		public string SchoolBuildLandPFISchemeType { get; set; }
		public int? SchoolConsultationStakeholders { get; set; }
		public string SchoolConsultationStakeholdersConsult { get; set; }
		public int? SchoolSupportGrantFundsPaidTo { get; set; }
		public string SchoolDeclarationSignedByName { get; set; }
	}
}

