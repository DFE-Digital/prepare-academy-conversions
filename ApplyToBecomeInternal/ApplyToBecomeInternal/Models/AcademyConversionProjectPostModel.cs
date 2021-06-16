using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApplyToBecomeInternal.Models
{
	public class AcademyConversionProjectPostModel
	{
		public string ProjectStatus { get; set; }
		public DateTime? ApplicationReceivedDate { get; set; }
		public DateTime? AssignedDate { get; set; }
		public DateTime? HeadTeacherBoardDate { get; set; }
		public DateTime? OpeningDate { get; set; }
		public DateTime? BaselineDate { get; set; }

		//la summary page
		[BindProperty(Name = "la-info-template-sent-date")]
		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }

		[BindProperty(Name = "la-info-template-returned-date")]
		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }

		[BindProperty(Name = "la-info-template-comments")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string LocalAuthorityInformationTemplateComments { get; set; }

		[BindProperty(Name = "la-info-template-sharepoint-link")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string LocalAuthorityInformationTemplateLink { get; set; }

		[BindProperty(Name = "la-info-template-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? LocalAuthorityInformationTemplateSectionComplete { get; set; }

		//school/trust info
		public string RecommendationForProject { get; set; }
		public string Author { get; set; }
		public string ClearedBy { get; set; }
		public bool? IsAoRequired { get; set; }
		public DateTime? ProposedAcademyOpeningDate { get; set; }
		public bool? SchoolAndTrustInformationSectionComplete { get; set; }

		//general info

		[BindProperty(Name = "published-admission-number")]
		public string PublishedAdmissionNumber { get; set; }

		[BindProperty(Name = "viability-issues")]
		public string ViabilityIssues { get; set; }

		[BindProperty(Name = "financial-deficit")]
		public string FinancialDeficit { get; set; }

		[BindProperty(Name = "diocesan-multi-academy-trust")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? IsThisADiocesanTrust { get; set; }

		[BindProperty(Name = "distance-to-trust-headquarters")]
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }

		[BindProperty(Name = "distance-to-trust-headquarters-additional-information")]
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }

		[BindProperty(Name = "general-information-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? GeneralInformationSectionComplete { get; set; }

		//school performance ofsted information
		public string SchoolPerformanceAdditionalInformation { get; set; }

		// rationale
		[BindProperty(Name = "project-rationale")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RationaleForProject { get; set; }

		[BindProperty(Name = "trust-rationale")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RationaleForTrust { get; set; }

		[BindProperty(Name = "rationale-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? RationaleSectionComplete { get; set; }

		// risk and issues
		[BindProperty(Name = "risks-and-issues")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RisksAndIssues { get; set; }

		[BindProperty(Name = "risks-and-issues-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? RisksAndIssuesSectionComplete { get; set; }

		// school budget info
		[BindProperty(Name = "finance-current-year-2021")]
		public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }

		[BindProperty(Name = "finance-following-year-2022")]
		public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }

		[BindProperty(Name = "finance-forward-2021")]
		public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }

		[BindProperty(Name = "finance-forward-2022")]
		public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }

		[BindProperty(Name = "school-budget-information-additional-information")]
		public string SchoolBudgetInformationAdditionalInformation { get; set; }

		[BindProperty(Name = "school-budget-information-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? SchoolBudgetInformationSectionComplete { get; set; }

		// pupil schools forecast
		[BindProperty(Name = "school-pupil-forecasts-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		//key stage performance tables
		public bool? KeyStagePerformanceTablesAdditionalInformation { get; set; }
	}
}
