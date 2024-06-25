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

   public SchoolImprovementPlan ImprovementPlan { get; set; }

   public IActionResult OnGet(int id)
   {
      ImprovementPlan = GetSchoolImprovementPlanFromSession(id);

      if (ImprovementPlan == null) return RedirectToPage(Links.TaskList.Index.Page, LinkParameters);

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      if (!ModelState.IsValid) return OnGet(id);

      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);
      improvementPlan.ProjectId = id;

      await CreateOrUpdateSchoolImprovementPlan(id, improvementPlan);

      SetSchoolImprovementPlanInSession(id, null);

      TempData.SetNotification(NotificationType.Success, "Done", "Improvement plan saved");

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }

   private async Task CreateOrUpdateSchoolImprovementPlan(int id, SchoolImprovementPlan plan)
   {
      //ApiResponse<AdvisoryBoardDecision> savedDecision = await _advisoryBoardDecisionRepository.Get(id);

      //if (savedDecision.StatusCode == HttpStatusCode.NotFound)
      //   await _advisoryBoardDecisionRepository.Create(schoolImprovementPlan);
      //else
      //   await _advisoryBoardDecisionRepository.Update(schoolImprovementPlan);

      //await _academyConversionProjectRepository.UpdateProject(id, new UpdateAcademyConversionProject { ProjectStatus = schoolImprovementPlan.GetDecisionAsFriendlyName() });

     await _academyConversionProjectRepository.AddSchoolImprovementPlan(id, new AddSchoolImprovementPlan(plan.ProjectId,
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
