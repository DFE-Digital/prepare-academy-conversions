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
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialSurplusOrDeficit { get; set; }
		public bool? IsThisADiocesanTrust { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string MemberOfParliamentParty { get; set; }
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

		/*[BindProperty(Name = "rationale-complete-hidden")]
		public bool? RationaleSectionCompleteHidden { get; set; }*/

		// risk and issues
		[BindProperty(Name = "risks-and-issues")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RisksAndIssues { get; set; }

		[BindProperty(Name = "risks-and-issues-complete")]
		[ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
		public bool? RisksAndIssuesSectionComplete { get; set; }

		/*[BindProperty(Name = "risks-and-issues-complete-hidden")]
		public bool? RisksAndIssuesSectionCompleteHidden { get; set; }*/

		// school budget info
		public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }
		public string SchoolBudgetInformationAdditionalInformation { get; set; }

		// pupil schools forecast
		[BindProperty(Name = "school-pupil-forecasts-additional-information")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		//key stage performance tables
		public bool? KeyStagePerformanceTablesAdditionalInformation { get; set; }
	}
}
