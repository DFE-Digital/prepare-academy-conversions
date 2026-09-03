
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class AnyConditionsModel(ISignificantChangeProjectRepository repository,
                                ISession session,
                                ErrorService errorService) : SignificantChangeDecisionBaseModel(repository, session)
{
   [BindProperty]
   [Required(ErrorMessage = "Select whether any conditions were set")]
   public bool? ApprovedConditionsSet { get; set; }

   [BindProperty]
   public string ApprovedConditionsDetails { get; set; }

   private bool HasConditions => ApprovedConditionsSet.GetValueOrDefault();

   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(Links.SignificantChangeDecision.RecordDecision, id);

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
         SignificantChangeDecision decision = GetDecisionFromSession(id);

         decision.ApprovedConditionsSet = HasConditions;
         decision.ApprovedConditionsDetails = HasConditions ? ApprovedConditionsDetails : string.Empty;

         SetDecisionInSession(id, decision);

         return RedirectToPage(Links.SignificantChangeDecision.WhoDecided.Page, LinkParameters);
      }

      errorService.AddErrors(ModelState.Keys, ModelState);
      return OnGet(id);
   }
}