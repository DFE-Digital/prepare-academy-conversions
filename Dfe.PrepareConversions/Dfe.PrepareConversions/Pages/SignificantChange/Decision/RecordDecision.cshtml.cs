
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

public class RecordDecisionModel : SignificantChangeDecisionBaseModel
{
   private readonly ErrorService _errorService;

   public RecordDecisionModel(ISignificantChangeProjectRepository repository,
                              ISession session,
                              ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;

      // Changing the decision must re-walk the branch, so never jump straight back to summary.
      PropagateBackLinkOverride = false;
   }

   [BindProperty]
   [Required(ErrorMessage = "Select a decision")]
   public SignificantChangeDecisions? SignificantChangeDecision { get; set; }

   public IEnumerable<SignificantChangeDecisions> DecisionOptions =>
      Enum.GetValues(typeof(SignificantChangeDecisions)).Cast<SignificantChangeDecisions>();

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.SignificantChange.SignificantChangeTaskList, id);

      SignificantChangeDecision = GetDecisionFromSession(id).Decision;

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(["SignificantChangeDecision"], ModelState);
         return OnGet(id);
      }

      Data.Models.SignificantChange.SignificantChangeDecision decision = GetDecisionFromSession(id);
      decision.Decision = SignificantChangeDecision.Value;
      SetDecisionInSession(id, decision);

      return decision.Decision switch
      {
         SignificantChangeDecisions.Approved => RedirectToPage(Links.SignificantChange.Decision.AnyConditions.Page, LinkParameters),
         SignificantChangeDecisions.Declined => RedirectToPage(Links.SignificantChange.Decision.DeclineReason.Page, LinkParameters),
         SignificantChangeDecisions.Deferred => RedirectToPage(Links.SignificantChange.Decision.WhyDeferred.Page, LinkParameters),
         SignificantChangeDecisions.Withdrawn => RedirectToPage(Links.SignificantChange.Decision.WhyWithdrawn.Page, LinkParameters),
         _ => RedirectToPage(Links.SignificantChange.Decision.AnyConditions.Page, LinkParameters)
      };
   }
}