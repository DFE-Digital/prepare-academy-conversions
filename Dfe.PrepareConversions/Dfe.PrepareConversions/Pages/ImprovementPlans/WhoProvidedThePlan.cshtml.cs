using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class WhoProvidedThePlanModel : SchoolImprovementPlanBaseModel
{
   private readonly ErrorService _errorService;

   public WhoProvidedThePlanModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty]
   public string PlanProvider { get; set; }


   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.ImprovementPlans.WhoArrangedThePlan, id);

      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);
   
      SetModel(improvementPlan);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      improvementPlan.ProvidedBy = PlanProvider;

      SetSchoolImprovementPlanInSession(id, improvementPlan);

      if (string.IsNullOrWhiteSpace(PlanProvider)) ModelState.AddModelError("PlanProvider", "Please enter who is providing the plan");

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.ImprovementPlans.Index.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      PlanProvider = schoolImprovementPlan.ProvidedBy;    
   }
}
