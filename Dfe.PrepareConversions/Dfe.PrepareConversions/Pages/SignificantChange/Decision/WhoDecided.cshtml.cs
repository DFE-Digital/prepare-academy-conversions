
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class WhoDecidedModel(ISignificantChangeProjectRepository repository,
                             ISession session,
                             ErrorService errorService) : SignificantChangeDecisionBaseModel(repository, session)
{
   [BindProperty]
   [Required(ErrorMessage = "Select who made the decision")]
   public DecisionMadeBy? DecisionMadeBy { get; set; }

   public IEnumerable<DecisionMadeBy> DecisionMadeByOptions =>
      Enum.GetValues(typeof(DecisionMadeBy)).Cast<DecisionMadeBy>();

   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(GetPageForBackLink(decision), id);
      DecisionMadeBy = decision.DecisionMadeBy;

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         errorService.AddErrors(["DecisionMadeBy"], ModelState);
         return OnGet(id);
      }

      SignificantChangeDecision decision = GetDecisionFromSession(id);
      decision.DecisionMadeBy = DecisionMadeBy;

      // The API only requires a decision maker's name when someone actually made the decision.
      bool skipName = DecisionMadeBy == Data.Models.AdvisoryBoardDecision.DecisionMadeBy.None;
      if (skipName) decision.DecisionMakerName = null;

      SetDecisionInSession(id, decision);

      return skipName
         ? RedirectToPage(Links.SignificantChange.Decision.DecisionDate.Page, LinkParameters)
         : RedirectToPage(Links.SignificantChange.Decision.DecisionMaker.Page, LinkParameters);
   }

   private static LinkItem GetPageForBackLink(SignificantChangeDecision decision)
   {
      return decision.Decision switch
      {
         SignificantChangeDecisions.Approved => Links.SignificantChange.Decision.AnyConditions,
         SignificantChangeDecisions.Declined => Links.SignificantChange.Decision.DeclineReason,
         SignificantChangeDecisions.Deferred => Links.SignificantChange.Decision.WhyDeferred,
         SignificantChangeDecisions.Withdrawn => Links.SignificantChange.Decision.WhyWithdrawn,
         _ => Links.SignificantChange.Decision.RecordDecision
      };
   }
}