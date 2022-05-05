using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApplyToBecomeInternal.Models
{
	public class AcademyConversionProjectPostModel
	{
		public string ProjectStatus { get; set; }
		public DateTime? ApplicationReceivedDate { get; set; }
		public DateTime? AssignedDate { get; set; }

		[BindProperty(Name = "head-teacher-board-date")]
		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[DateValidation(DateRangeValidationService.DateRange.Future)]
		[Display(Name = "Advisory Board")]
		public DateTime? HeadTeacherBoardDate { get; set; }
		public DateTime? BaselineDate { get; set; }

		//la summary page
		[BindProperty(Name = "la-info-template-sent-date")]
		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[Display(Name = "Date you sent the template")]
		public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }

		[BindProperty(Name = "la-info-template-returned-date")]
		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[Display(Name = "Date you want the template returned")]
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
		[BindProperty(Name = "project-recommendation")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RecommendationForProject { get; set; }

		[BindProperty(Name = "author")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string Author { get; set; }

		[BindProperty(Name = "cleared-by")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string ClearedBy { get; set; }

		[BindProperty(Name = "academy-order-required")]
		public string AcademyOrderRequired { get; set; }

		[BindProperty(Name = "proposed-academy-opening-date")]
		public DateTime? ProposedAcademyOpeningDate { get; set; }

		[BindProperty(Name = "school-and-trust-information-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? SchoolAndTrustInformationSectionComplete { get; set; }

		[BindProperty(Name = "previous-head-teacher-board-date-question")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string PreviousHeadTeacherBoardDateQuestion { get; set; }

		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[DateValidation(DateRangeValidationService.DateRange.Past)]
		[BindProperty(Name = "previous-head-teacher-board-date")]
		[Required]
		[Display(Name = "Previous Advisory Board")]
		public DateTime? PreviousHeadTeacherBoardDate { get; set; }
		
		//general info

		[BindProperty(Name = "published-admission-number")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string PublishedAdmissionNumber { get; set; }

		[BindProperty(Name = "viability-issues")]
		public string ViabilityIssues { get; set; }

		[BindProperty(Name = "financial-deficit")]
		public string FinancialDeficit { get; set; }

		[BindProperty(Name = "diocesan-multi-academy-trust")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? IsThisADiocesanTrust { get; set; }

		[BindProperty(Name = "distance-to-trust-headquarters")]
		[ModelBinder(BinderType = typeof(DecimalInputModelBinder))]
		[Display(Name = "Distance from the school to the trust headquarters")]
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }

		[BindProperty(Name = "distance-to-trust-headquarters-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }

		[BindProperty(Name = "member-of-parliament-name")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string MemberOfParliamentName { get; set; }

		[BindProperty(Name = "member-of-parliament-party")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string MemberOfParliamentParty { get; set; }

		[BindProperty(Name = "general-information-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? GeneralInformationSectionComplete { get; set; }

		//school performance ofsted information
		[BindProperty(Name = "school-performance-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
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
		[ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
		[Display(Name = "Revenue carry forward at end-March (current year)")]
		public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }

		[BindProperty(Name = "finance-following-year-2022")]
		[ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
		[Display(Name = "Projected revenue balance at end-March (following year)")]
		public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }

		[BindProperty(Name = "finance-forward-2021")]
		[ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
		[Display(Name = "Capital carry forward at end-March (current year)")]
		public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }

		[BindProperty(Name = "finance-forward-2022")]
		[ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
		[Display(Name = "Projected capital balance at end-March (following year)")]
		public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }

		[BindProperty(Name = "school-budget-information-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string SchoolBudgetInformationAdditionalInformation { get; set; }

		[BindProperty(Name = "school-budget-information-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? SchoolBudgetInformationSectionComplete { get; set; }

		// pupil schools forecast
		[BindProperty(Name = "school-pupil-forecasts-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		// key stage performance tables
		[BindProperty(Name = "key-stage-2-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string KeyStage2PerformanceAdditionalInformation { get; set; }

		[BindProperty(Name = "key-stage-4-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string KeyStage4PerformanceAdditionalInformation { get; set; }

		[BindProperty(Name = "key-stage-5-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string KeyStage5PerformanceAdditionalInformation { get; set; }
	}
}
