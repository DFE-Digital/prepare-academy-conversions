using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class CommentsOnThePlanModel : SchoolImprovementPlanBaseModel
{
   private readonly ErrorService _errorService;

   public CommentsOnThePlanModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty]
   public string PlanComments { get; set; }


   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.ImprovementPlans.ConfidenceLevelOfThePlan, id);

      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);
   
      SetModel(improvementPlan);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      improvementPlan.PlanComments = PlanComments;

      SetSchoolImprovementPlanInSession(id, improvementPlan);


      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.ImprovementPlans.Summary.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      PlanComments = schoolImprovementPlan.PlanComments;    
   }
}
