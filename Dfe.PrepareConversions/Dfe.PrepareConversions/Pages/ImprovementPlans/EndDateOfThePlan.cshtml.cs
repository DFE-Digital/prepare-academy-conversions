using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class EndDateOfThePlanModel : SchoolImprovementPlanBaseModel, IDateValidationMessageProvider
{
   private readonly ErrorService _errorService;

   public EndDateOfThePlanModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty(Name = "plan-end-date-other", BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
   [Display(Name = "EndDateOther")]
   [Required]
   public DateTime? PlanEndDateOther { get; set; }

   [BindProperty]
   [Required]
   [Display(Name = "Expected end date")]
   public SchoolImprovementPlanExpectedEndDate? ExpectedEndDate{ get; set; }

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.ImprovementPlans.StartDateOfThePlan, id);

      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      SetModel(improvementPlan);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      if (PlanEndDateOther.HasValue)
      {
         improvementPlan.ExpectedEndDateOther = PlanEndDateOther.Value;
      }
      improvementPlan.ExpectedEndDate = ExpectedEndDate;

      SetSchoolImprovementPlanInSession(id, improvementPlan);

      // Override the validation on the date helper as it is only required when selecting other
      if (ExpectedEndDate is not SchoolImprovementPlanExpectedEndDate.Other)
      {
         this.ViewData.ModelState.Remove("plan-end-date-other");
      }

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.ImprovementPlans.ConfidenceLevelOfThePlan.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      ExpectedEndDate = schoolImprovementPlan.ExpectedEndDate;
      PlanEndDateOther = schoolImprovementPlan.ExpectedEndDateOther;
   }

   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      return $"Enter the expected end date of the plan";
   }
}
