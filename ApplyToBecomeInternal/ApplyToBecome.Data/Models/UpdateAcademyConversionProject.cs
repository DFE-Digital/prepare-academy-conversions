using System;

namespace ApplyToBecome.Data.Models
{
	public class UpdateAcademyConversionProject
	{
		public string ProjectStatus { get; set; }
		public DateTime? ApplicationReceivedDate { get; set; }
		public DateTime? AssignedDate { get; set; }
		public DateTime? HeadTeacherBoardDate { get; set; }
		public DateTime? OpeningDate { get; set; }
		public DateTime? BaselineDate { get; set; }

		//la summary page
		public DateTime? SentDate { get; set; }
		public DateTime? ReturnedDate { get; set; }
		public string Comments { get; set; }
		public string AdditionalInfo { get; set; }

		//school/trust info
		public string RecommendationForProject { get; set; }
		public string Author { get; set; }
		public string ClearedBy { get; set; }
		public bool? IsAoRequired { get; set; }
		public DateTime? ProposedAcademyOpeningDate { get; set; }
		public bool? SchoolAndTrustInformationMarkAsComplete { get; set; }

		//general info
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialSurplusOrDeficit { get; set; }
		public bool? IsThisADiocesanTrust { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string MemberOfParliamentParty { get; set; }
		public bool? GeneralInformationMarkAsComplete { get; set; }

		//school performance ofsted information
		public string SchoolPerformanceAdditionalInformation { get; set; }

		// rationale
		public string RationaleForProject { get; set; }
		public string RationaleForTrust { get; set; }
		public bool? RationaleMarkAsComplete { get; set; }

		// risk and issues
		public string RisksAndIssues { get; set; }
		public bool? RisksAndIssuesMarkAsComplete { get; set; }

		// school budget info
		public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }
		public string SchoolBudgetInformationAdditionalInformation { get; set; }

		// pupil schools forecast
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		//key stage performance tables
		public bool? KeyStagePerformanceTablesAdditionalInformation { get; set; }
	}
}