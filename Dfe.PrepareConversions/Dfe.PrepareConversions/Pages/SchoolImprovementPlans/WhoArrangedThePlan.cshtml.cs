using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class WhoArrangedThePlanModel : SchoolImprovementPlanBaseModel
{
   private readonly ErrorService _errorService;

   public WhoArrangedThePlanModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty]
   public bool LocalAuthorityIsChecked { get; set; }

   [BindProperty]
   public bool RegionalDirectorIsChecked { get; set; }

   [BindProperty]
   public bool DioceseIsChecked { get; set; }

   [BindProperty]
   public string OtherDetails { get; set; }

   [BindProperty]
   public bool OtherIsChecked { get; set; }

   [BindProperty]
   public bool WasArrangerGiven => LocalAuthorityIsChecked || RegionalDirectorIsChecked || DioceseIsChecked || OtherIsChecked;

   public override async Task<IActionResult> OnGetAsync(int id, int? sipId = null)
   {
      // call base to set School Improvement Plan
      await base.OnGetAsync(id, sipId);

      SetBackLinkModel(Links.SchoolImprovementPlans.Index, id);
   
      SetModel(SchoolImprovementPlan);

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, int? sipId = null)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      improvementPlan.ArrangedBy = new List<SchoolImprovementPlanArranger>();
      improvementPlan.ArrangedByOther = null;

      if (RegionalDirectorIsChecked)
      {
         improvementPlan.ArrangedBy.Add(SchoolImprovementPlanArranger.RegionalDirector);
      }

      if (LocalAuthorityIsChecked)
      {
         improvementPlan.ArrangedBy.Add(SchoolImprovementPlanArranger.LocalAuthority);
      }

      if (DioceseIsChecked)
      {
         improvementPlan.ArrangedBy.Add(SchoolImprovementPlanArranger.Diocese);
      }

      if (OtherIsChecked)
      {
         improvementPlan.ArrangedBy.Add(SchoolImprovementPlanArranger.Other);
         improvementPlan.ArrangedByOther = OtherDetails;
      }

      if (OtherIsChecked && string.IsNullOrWhiteSpace(OtherDetails))
      {
         ModelState.AddModelError($"Other Details", $"Enter a value for selecting {SchoolImprovementPlanArranger.Other.ToDescription()}");
      }

      SetSchoolImprovementPlanInSession(id, improvementPlan);

      if (!WasArrangerGiven) ModelState.AddModelError("WasArrangerGiven", "Select at least one arranger");

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return await OnGetAsync(id, sipId);

      return RedirectToPage(Links.SchoolImprovementPlans.WhoProvidedThePlan.Page, LinkParameters);
   }

   private void SetModel(SchoolImprovementPlan schoolImprovementPlan)
   {
      LocalAuthorityIsChecked = schoolImprovementPlan.ArrangedBy.Contains(SchoolImprovementPlanArranger.LocalAuthority);
      RegionalDirectorIsChecked = schoolImprovementPlan.ArrangedBy.Contains(SchoolImprovementPlanArranger.RegionalDirector);
      DioceseIsChecked = schoolImprovementPlan.ArrangedBy.Contains(SchoolImprovementPlanArranger.Diocese);
      OtherIsChecked = schoolImprovementPlan.ArrangedBy.Contains(SchoolImprovementPlanArranger.Other);
      OtherDetails = schoolImprovementPlan.ArrangedByOther;
   }
}
