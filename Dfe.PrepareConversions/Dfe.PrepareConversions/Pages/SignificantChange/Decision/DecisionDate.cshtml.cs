
using Dfe.Academisation.ExtensionMethods;
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

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class DecisionDateModel(ISignificantChangeProjectRepository repository,
                               ISession session,
                               ErrorService errorService)
   : SignificantChangeDecisionBaseModel(repository, session), IDateValidationMessageProvider
{
   [BindProperty(Name = "decision-date", BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
   [Display(Name = "decision")]
   [Required]
   public DateTime? DateOfDecision { get; set; }

   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      string idRaw = Request.RouteValues["id"] as string;
      if (!int.TryParse(idRaw, out int id)) return "Enter the date of the decision";

      SignificantChangeDecision decision = GetDecisionFromSession(id);

      return decision.Decision == null
         ? "Enter the date of the decision"
         : $"Enter the date when the project was {decision.Decision.ToDescription().ToLowerInvariant()}";
   }

   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(
         decision.DecisionMadeBy == DecisionMadeBy.None
            ? Links.SignificantChangeDecision.WhoDecided
            : Links.SignificantChangeDecision.DecisionMaker,
         id);

      DateOfDecision = decision.DecisionDate;

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);
      decision.DecisionDate = DateOfDecision;

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(Request.Form.Keys, ModelState);
         return OnGet(id);
      }

      SetDecisionInSession(id, decision);

      return RedirectToPage(Links.SignificantChangeDecision.Summary.Page, new { id });
   }
}