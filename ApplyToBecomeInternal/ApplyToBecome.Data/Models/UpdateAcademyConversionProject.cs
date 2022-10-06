using System;

namespace ApplyToBecome.Data.Models
{
	public class UpdateAcademyConversionProject
	{
		public string ProjectStatus { get; set; }
		public DateTime? ApplicationReceivedDate { get; set; }
		public DateTime? AssignedDate { get; set; }
		public DateTime? HeadTeacherBoardDate { get; set; }
		//public DateTime? OpeningDate { get; set; }
		public DateTime? BaselineDate { get; set; }

		//la summary page
		public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
		public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
		public string LocalAuthorityInformationTemplateComments { get; set; }
		public string LocalAuthorityInformationTemplateLink { get; set; }
		public bool? LocalAuthorityInformationTemplateSectionComplete { get; set; }

		//school/trust info
		public string RecommendationForProject { get; set; }
		public string Author { get; set; }
		public string ClearedBy { get; set; }
		public string AcademyOrderRequired { get; set; }
		public DateTime? ProposedAcademyOpeningDate { get; set; }
		public bool? SchoolAndTrustInformationSectionComplete { get; set; }
		public string PreviousHeadTeacherBoardDateQuestion { get; set; }
		public DateTime? PreviousHeadTeacherBoardDate { get; set; }
		public decimal? ConversionSupportGrantAmount { get; set; }
		public string ConversionSupportGrantChangeReason { get; set; }

		//general info
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialDeficit { get; set; }
		public bool? IsThisADiocesanTrust { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public string MemberOfParliamentName { get; set; }
		public string MemberOfParliamentParty { get; set; }
		public bool? GeneralInformationSectionComplete { get; set; }

		//school performance ofsted information
		public string SchoolPerformanceAdditionalInformation { get; set; }

		// rationale
		public string RationaleForProject { get; set; }
		public string RationaleForTrust { get; set; }
		public bool? RationaleSectionComplete { get; set; }

		// risk and issues
		public string RisksAndIssues { get; set; }
		public bool? RisksAndIssuesSectionComplete { get; set; }

		// legal requirements
		public string GoverningBodyResolution { get; set; }
		public string Consultation { get; set; }
		public string DiocesanConsent { get; set; }
		public string FoundationConsent { get; set; }
		public bool? LegalRequirementsSectionComplete { get; set; }

		// school budget info
		public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }
		public string SchoolBudgetInformationAdditionalInformation { get; set; }
		public bool? SchoolBudgetInformationSectionComplete { get; set; }

		// pupil schools forecast
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		//key stage performance tables
		public string KeyStage2PerformanceAdditionalInformation { get; set; }
		public string KeyStage4PerformanceAdditionalInformation { get; set; }
		public string KeyStage5PerformanceAdditionalInformation { get; set; }

		// assigned user
		public User AssignedUser { get; set; }
	}
}
