using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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


   public override async Task<IActionResult> OnGetAsync(int id, int? sipId = null)
   {
      // call base to set School Improvement Plan
      await base.OnGetAsync(id, sipId);
      SetBackLinkModel(Links.SchoolImprovementPlans.WhoArrangedThePlan, id);
   
      SetModel(SchoolImprovementPlan);

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, int? sipId = null)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      improvementPlan.ProvidedBy = PlanProvider;

      SetSchoolImprovementPlanInSession(id, improvementPlan);

      if (string.IsNullOrWhiteSpace(PlanProvider)) ModelState.AddModelError("PlanProvider", "Please enter who is providing the plan");

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return await OnGetAsync(id, sipId);

      return RedirectToPage(Links.SchoolImprovementPlans.StartDateOfThePlan.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      PlanProvider = schoolImprovementPlan.ProvidedBy;    
   }
}
