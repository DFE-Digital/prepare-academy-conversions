using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class SummaryModel : SchoolImprovementPlanBaseModel
{
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
   private readonly IAcademyConversionAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

   public SummaryModel(IAcademyConversionProjectRepository repository,
                       ISession session,
                       IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
                       IAcademyConversionProjectRepository academyConversionProjectRepository)
      : base(repository, session)
   {
      _advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
      _academyConversionProjectRepository = academyConversionProjectRepository;
   }

   public override async Task<IActionResult> OnGetAsync(int id, int? sipId = null)
   {
      // call base to set School Improvement Plan
      await base.OnGetAsync(id, sipId);

      if (SchoolImprovementPlan == null) return RedirectToPage(Links.TaskList.Index.Page, LinkParameters);

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id, int? sipId = null)
   {
      if (!ModelState.IsValid) return await OnGetAsync(id);

      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);
      improvementPlan.ProjectId = id;

      await CreateOrUpdateSchoolImprovementPlan(id, improvementPlan, sipId);

      SetSchoolImprovementPlanInSession(id, null);

      TempData.SetNotification(NotificationType.Success, "Done", "Improvement plan saved");

      return RedirectToPage(Links.SchoolImprovementPlans.Index.Page, new { id });
   }

   private async Task CreateOrUpdateSchoolImprovementPlan(int id, SchoolImprovementPlan plan, int? sipId = null)
   {
      if (sipId.HasValue && sipId.Value == plan.Id)
      {
         // update existing plan
         await _academyConversionProjectRepository.UpdateSchoolImprovementPlan(id, new UpdateSchoolImprovementPlan(
            plan.Id,
            plan.ArrangedBy,
            plan.ArrangedByOther,
            plan.ProvidedBy,
            plan.StartDate.Value,
            plan.ExpectedEndDate.Value,
            plan.ExpectedEndDateOther,
            plan.ConfidenceLevel.Value,
            plan.PlanComments));
      }
      else
      {
         // create new plan
         await _academyConversionProjectRepository.AddSchoolImprovementPlan(id, new AddSchoolImprovementPlan(
            plan.ProjectId,
            plan.ArrangedBy,
            plan.ArrangedByOther,
            plan.ProvidedBy,
            plan.StartDate.Value,
            plan.ExpectedEndDate.Value,
            plan.ExpectedEndDateOther,
            plan.ConfidenceLevel.Value,
            plan.PlanComments));
      }


   }
}
