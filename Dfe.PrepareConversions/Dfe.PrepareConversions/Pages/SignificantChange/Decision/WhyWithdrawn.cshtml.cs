
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class WhyWithdrawnModel(ISignificantChangeProjectRepository repository,
                               ISession session,
                               ErrorService errorService) : SignificantChangeDecisionBaseModel(repository, session)
{
   [BindProperty] public string Details { get; set; }


   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      Details = decision.WithdrawnReason;

      SetBackLinkModel(Links.SignificantChangeDecision.RecordDecision, id);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      decision.WithdrawnReason = Details;

      SetDecisionInSession(id, decision);

      if (string.IsNullOrEmpty(Details)) ModelState.AddModelError("WereDetailsProvided", "Enter details");

      errorService.AddErrors(ModelState.Keys, ModelState);
      if (errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.SignificantChangeDecision.WhoDecided.Page, LinkParameters);
   }
}