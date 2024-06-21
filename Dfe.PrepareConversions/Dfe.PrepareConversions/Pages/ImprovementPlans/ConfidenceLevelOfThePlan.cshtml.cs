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

public class ConfidenceLevelOfThePlanModel : SchoolImprovementPlanBaseModel
{
   private readonly ErrorService _errorService;

   public ConfidenceLevelOfThePlanModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty]
   [Required]
   [Display(Name = "Confidence Level")]
   public SchoolImprovementPlanConfidenceLevel? ConfidenceLevel{ get; set; }

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.ImprovementPlans.EndDateOfThePlan, id);

      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      SetModel(improvementPlan);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      improvementPlan.ConfidenceLevel = ConfidenceLevel;

      SetSchoolImprovementPlanInSession(id, improvementPlan);

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.ImprovementPlans.CommentsOnThePlan.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      ConfidenceLevel = schoolImprovementPlan.ConfidenceLevel;
   }
}
