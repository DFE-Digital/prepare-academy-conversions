using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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

   public override async Task<IActionResult> OnGetAsync(int id, int? sipId = null)
   {
      // call base to set School Improvement Plan
      await base.OnGetAsync(id, sipId);
      SetBackLinkModel(Links.SchoolImprovementPlans.EndDateOfThePlan, id);

      SetModel(SchoolImprovementPlan);

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, int? sipId = null)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      improvementPlan.ConfidenceLevel = ConfidenceLevel;

      SetSchoolImprovementPlanInSession(id, improvementPlan);

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return await OnGetAsync(id, sipId);

      return RedirectToPage(Links.SchoolImprovementPlans.CommentsOnThePlan.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      ConfidenceLevel = schoolImprovementPlan.ConfidenceLevel;
   }
}
