using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class DAOBeforeYouStartModel : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public DAOBeforeYouStartModel(IAcademyConversionProjectRepository repository,
                          ISession session,
                          ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   public DecisionMadeBy? DecisionMadeBy { get; set; }

   public string DecisionText { get; set; }

   public IEnumerable<DecisionMadeBy> DecisionMadeByOptions => Enum.GetValues(typeof(DecisionMadeBy)).Cast<DecisionMadeBy>();

   [BindProperty]
   public bool MinisterApproval { get; set; }

   [BindProperty]
   public bool LetterSent { get; set; }

   [BindProperty]
   public bool LetterSaved { get; set; }
   public bool ShowBanner { get; set; } = false;

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.RecordDecision, id);
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);

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
      decision.DecisionMadeBy = Data.Models.AdvisoryBoardDecision.DecisionMadeBy.Minister;
      decision.DecisionMakerName = decision.DecisionMadeBy == Data.Models.AdvisoryBoardDecision.DecisionMadeBy.None ? null : decision.DecisionMakerName;

      // Use the bound properties to check the checkbox values
      if (MinisterApproval && LetterSent && LetterSaved)
      {
         // All checkboxes are checked, proceed with the decision
         SetDecisionInSession(id, decision);
         return DetermineRedirectPage(decision);
      }
      else
      {
         // Handle the case where not all checkboxes are checked
         ShowBanner = true;
         return OnGet(id);
      }
   }

   private IActionResult DetermineRedirectPage(AdvisoryBoardDecision decision)
   {
      var pageToReturnTo = decision.Decision switch
      {
         AdvisoryBoardDecisions.Approved => RedirectToPage(Links.Decision.AnyConditions.Page, LinkParameters),
         AdvisoryBoardDecisions.Declined => RedirectToPage(Links.Decision.DeclineReason.Page, LinkParameters),
         AdvisoryBoardDecisions.Deferred => RedirectToPage(Links.Decision.WhyDeferred.Page, LinkParameters),
         AdvisoryBoardDecisions.Withdrawn => RedirectToPage(Links.Decision.WhyWithdrawn.Page, LinkParameters),
         AdvisoryBoardDecisions.DAORevoked => RedirectToPage(Links.Decision.WhyDAORevoked.Page, LinkParameters),
         _ => RedirectToPage(Links.Decision.AnyConditions.Page, LinkParameters)
      };

      return decision.DecisionMadeBy == Data.Models.AdvisoryBoardDecision.DecisionMadeBy.None ? pageToReturnTo : RedirectToPage(Links.Decision.DecisionMaker.Page, LinkParameters);
   }
}
