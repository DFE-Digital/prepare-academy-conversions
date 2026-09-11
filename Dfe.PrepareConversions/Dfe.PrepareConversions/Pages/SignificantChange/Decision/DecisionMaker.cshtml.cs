
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class DecisionMakerModel(ISignificantChangeProjectRepository repository,
                                ISession session,
                                ErrorService errorService) : SignificantChangeDecisionBaseModel(repository, session)
{
   [BindProperty(Name = "decision-maker-name")]
   [Required]
   public string DecisionMakerName { get; set; }

   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(Links.SignificantChangeDecision.WhoDecided, id);
      DecisionMakerName = decision.DecisionMakerName;

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         errorService.AddError("decision-maker-name", "Enter the decision maker's name");
         return OnGet(id);
      }

      SignificantChangeDecision decision = GetDecisionFromSession(id);
      decision.DecisionMakerName = DecisionMakerName;

      SetDecisionInSession(id, decision);

      return RedirectToPage(Links.SignificantChangeDecision.DecisionDate.Page, LinkParameters);
   }
}