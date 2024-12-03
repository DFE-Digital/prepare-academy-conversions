using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class AnyConditionsModel(IAcademyConversionProjectRepository repository,
                          ISession session,
                          ErrorService errorService) : DecisionBaseModel(repository, session)
{
   [BindProperty]
   [Required(ErrorMessage = "Select whether any conditions were set")]
   public bool? ApprovedConditionsSet { get; set; }

   [BindProperty]
   public string ApprovedConditionsDetails { get; set; }

   private bool HasConditions => ApprovedConditionsSet.GetValueOrDefault();

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.WhoDecided, id);

      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      ApprovedConditionsSet = decision.ApprovedConditionsSet;
      ApprovedConditionsDetails = decision.ApprovedConditionsDetails;

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (HasConditions && string.IsNullOrWhiteSpace(ApprovedConditionsDetails))
         ModelState.AddModelError(nameof(ApprovedConditionsDetails), "Add the conditions that were set");

      if (ModelState.IsValid)
      {
         AdvisoryBoardDecision decision = GetDecisionFromSession(id);

         decision.ApprovedConditionsSet = HasConditions;
         decision.ApprovedConditionsDetails = HasConditions ? ApprovedConditionsDetails : string.Empty;

         SetDecisionInSession(id, decision);

         return RedirectToPage(Links.Decision.DecisionMaker.Page, LinkParameters);
      }

      errorService.AddErrors(ModelState.Keys, ModelState);
      return OnGet(id);
   }
}
