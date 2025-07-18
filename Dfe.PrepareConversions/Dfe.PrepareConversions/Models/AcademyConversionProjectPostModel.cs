﻿using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Models;

public class AcademyConversionProjectPostModel
{
   public string ProjectStatus { get; set; }
   public DateTime? ApplicationReceivedDate { get; set; }
   public DateTime? AssignedDate { get; set; }

   [BindProperty(Name = "head-teacher-board-date")]
   [ModelBinder(BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
   [Display(Name = "Proposed decision")]
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

   [BindProperty(Name = "form-7-received")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string Form7Received { get; set; }

   [BindProperty(Name = "form-7-received-date")]
   [ModelBinder(BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.Past)]
   [Display(Name = "Form 7 Received Date")]
   public DateTime? Form7ReceivedDate { get; set; }

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
   [Display(Name = "Previously considered")]
   public DateTime? PreviousHeadTeacherBoardDate { get; set; }

   [ModelBinder(BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.Past)]
   [BindProperty(Name = "dao-pack-sent-date")]
   [Display(Name = "DAO pack sent")]
   public DateTime? DaoPackSentDate { get; set; }

   [ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
   [Display(Name = "Conversion support grant")]
   [Range(typeof(decimal), "0", "150000", ErrorMessage = "Enter an amount that is £150,000 or less, for example £25,000")]
   [BindProperty(Name = "conversion-support-grant-amount")]
   public decimal? ConversionSupportGrantAmount { get; set; }

   [BindProperty(Name = "conversion-support-grant-change-reason")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]

   public string ConversionSupportGrantChangeReason { get; set; }
   [BindProperty(Name = "conversion-support-grant-type")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]

   public string ConversionSupportGrantType { get; set; }

   [BindProperty(Name = "conversion-support-number-of-sites")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]

   public string ConversionSupportNumberOfSites { get; set; }

   [BindProperty(Name = "conversion-support-grant-environmental-improvement-grant")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]

   public string ConversionSupportGrantEnvironmentalImprovementGrant { get; set; }

   [BindProperty(Name = "conversion-support-grant-amount-changed")]
   public bool? ConversionSupportGrantAmountChanged { get; set; }

   // School Overview

   [BindProperty(Name = "published-admission-number")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string PublishedAdmissionNumber { get; set; }

   [BindProperty(Name = "viability-issues")]
   public string ViabilityIssues { get; set; }
   [BindProperty(Name = "number-of-places-funded-for")]
   public int? NumberOfPlacesFundedFor { get; set; }
   [BindProperty(Name = "number-of-residential-places")]
   public int? NumberOResidentialPlaces { get; set; }
   [BindProperty(Name = "number-of-funded-residential-places")]
   public int? NumberOfFundedResidentialPlaces { get; set; }

   [BindProperty(Name = "financial-deficit")]
   public string FinancialDeficit { get; set; }

   [BindProperty(Name = "diocesan-multi-academy-trust")]
   [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
   public bool? IsThisADiocesanTrust { get; set; }

   [BindProperty(Name = "distance-to-trust-headquarters")]
   [ModelBinder(BinderType = typeof(DecimalInputModelBinder))]
   [Display(Name = "Distance from the converting school to the trust or other schools in the trust")]
   public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }

   [BindProperty(Name = "distance-to-trust-headquarters-additional-information")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }

   [BindProperty(Name = "member-of-parliament-name-and-party")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string MemberOfParliamentNameAndParty { get; set; }

   [BindProperty(Name = "school-overview-complete")]
   [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
   public bool? SchoolOverviewSectionComplete { get; set; }

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

   [BindProperty(Name = "number-of-alternative-provisions-places")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string NumberOfAlternativeProvisionPlaces { get; set; }

   [BindProperty(Name = "number-of-medical-places")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string NumberOfMedicalPlaces { get; set; }

   [BindProperty(Name = "number-of-post-16-places")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string NumberOfPost16Places { get; set; }

   [BindProperty(Name = "number-of-sen-unit-places")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string NumberOfSENUnitPlaces { get; set; }

   // risk and issues
   [BindProperty(Name = "risks-and-issues")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string RisksAndIssues { get; set; }

   [BindProperty(Name = "risks-and-issues-complete")]
   [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
   public bool? RisksAndIssuesSectionComplete { get; set; }

   // legal requirements
   [BindProperty(Name = "legal-requirements-complete")]
   [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
   public bool? LegalRequirementsSectionComplete { get; set; }

   [BindProperty(Name = "governing-body-approved")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string GoverningBodyResolution { get; set; }

   [BindProperty(Name = "consultation")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string Consultation { get; set; }

   [BindProperty(Name = "diocesan-consent")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string DiocesanConsent { get; set; }

   [BindProperty(Name = "foundation-consent")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string FoundationConsent { get; set; }


   // school budget info
   [ModelBinder(BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
   [BindProperty(Name = "financial-year")]
   [Display(Name = "End of current financial year")]
   public DateTime? EndOfCurrentFinancialYear { get; set; }

   [ModelBinder(BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
   [BindProperty(Name = "next-financial-year")]
   [Display(Name = "End of next financial year")]
   public DateTime? EndOfNextFinancialYear { get; set; }

   [BindProperty(Name = "finance-year-current")]
   [ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
   [Display(Name = "Forecasted revenue carry forward at the end of the current financial year")]
   public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }

   [BindProperty(Name = "finance-year-following")]
   [ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
   [Display(Name = "Forecasted revenue carry forward at the end of the next financial year")]
   public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }

   [BindProperty(Name = "finance-current-capital")]
   [ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
   [Display(Name = "Forecasted capital carry forward at the end of the current financial year")]
   public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }

   [BindProperty(Name = "finance-projected-capital")]
   [ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
   [Display(Name = "Forecasted capital carry forward at the end of the next financial year")]
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

   // Public sector equality duty
   [BindProperty(Name = "public-equality-duty-impact")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string PublicEqualityDutyImpact { get; set; }

   [BindProperty(Name = "public-equality-duty-reduce-impact-reason")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string PublicEqualityDutyReduceImpactReason { get; set; }

   [BindProperty(Name = "public-equality-duty-section-complete")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public bool PublicEqualityDutySectionComplete { get; init; }
}
