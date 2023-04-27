using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class WhoDecidedModel : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public WhoDecidedModel(IAcademyConversionProjectRepository repository,
                          ISession session,
                          ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty]
   [Required(ErrorMessage = "Select who made the decision")]
   public DecisionMadeBy? DecisionMadeBy { get; set; }

   public string DecisionText { get; set; }

   public IEnumerable<DecisionMadeBy> DecisionMadeByOptions => Enum.GetValues(typeof(DecisionMadeBy)).Cast<DecisionMadeBy>();

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.RecordDecision, id);
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);

      DecisionMadeBy = decision?.DecisionMadeBy;
      DecisionText = decision == null ? string.Empty : decision.Decision.ToDescription().ToLowerInvariant();
      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(new[] { "DecisionMadeBy" }, ModelState);
         return OnGet(id);
      }

      AdvisoryBoardDecision decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
      decision.DecisionMadeBy = DecisionMadeBy;

      SetDecisionInSession(id, decision);

      return decision.Decision switch
      {
         AdvisoryBoardDecisions.Approved => RedirectToPage(Links.Decision.AnyConditions.Page, LinkParameters),
         AdvisoryBoardDecisions.Declined => RedirectToPage(Links.Decision.DeclineReason.Page, LinkParameters),
         AdvisoryBoardDecisions.Deferred => RedirectToPage(Links.Decision.WhyDeferred.Page, LinkParameters),
         _ => RedirectToPage(Links.Decision.AnyConditions.Page, LinkParameters)
      };
   }
}
