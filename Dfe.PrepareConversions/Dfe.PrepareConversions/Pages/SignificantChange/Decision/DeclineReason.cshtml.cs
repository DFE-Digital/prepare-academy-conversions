using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class DeclineReasonModel(ISignificantChangeProjectRepository repository,
                                ISession session,
                                ErrorService errorService) : SignificantChangeDecisionBaseModel(repository, session)
{
   [BindProperty] public string FinanceDetails { get; set; }
   [BindProperty] public bool FinanceIsChecked { get; set; }

   [BindProperty] public string PerformanceDetails { get; set; }
   [BindProperty] public bool PerformanceIsChecked { get; set; }

   [BindProperty] public string GovernanceDetails { get; set; }
   [BindProperty] public bool GovernanceIsChecked { get; set; }

   [BindProperty] public string ChoiceOfTrustDetails { get; set; }
   [BindProperty] public bool ChoiceOfTrustIsChecked { get; set; }

   [BindProperty] public string OtherDetails { get; set; }
   [BindProperty] public bool OtherIsChecked { get; set; }

   [BindProperty]
   public bool WasReasonGiven => FinanceIsChecked || PerformanceIsChecked || GovernanceIsChecked || ChoiceOfTrustIsChecked || OtherIsChecked;

   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(Links.SignificantChange.Decision.RecordDecision, id);
      SetReasonsModel(decision.DeclinedReasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      decision.DeclinedReasons.Clear();
      decision.DeclinedReasons
         .AddReasonIfValid(FinanceIsChecked, SignificantChangeDeclinedReason.Finance, FinanceDetails, ModelState)
         .AddReasonIfValid(PerformanceIsChecked, SignificantChangeDeclinedReason.Performance, PerformanceDetails, ModelState)
         .AddReasonIfValid(GovernanceIsChecked, SignificantChangeDeclinedReason.Governance, GovernanceDetails, ModelState)
         .AddReasonIfValid(ChoiceOfTrustIsChecked, SignificantChangeDeclinedReason.ChoiceOfTrust, ChoiceOfTrustDetails, ModelState)
         .AddReasonIfValid(OtherIsChecked, SignificantChangeDeclinedReason.Other, OtherDetails, ModelState);

      SetDecisionInSession(id, decision);

      if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

      errorService.AddErrors(ModelState.Keys, ModelState);
      if (errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.SignificantChange.Decision.WhoDecided.Page, LinkParameters);
   }

   private void SetReasonsModel(List<SignificantChangeDeclinedReasonDetails> reasons)
   {
      SignificantChangeDeclinedReasonDetails finance = reasons.GetReason(SignificantChangeDeclinedReason.Finance);
      FinanceIsChecked = finance != null;
      FinanceDetails = finance?.Details;

      SignificantChangeDeclinedReasonDetails performance = reasons.GetReason(SignificantChangeDeclinedReason.Performance);
      PerformanceIsChecked = performance != null;
      PerformanceDetails = performance?.Details;

      SignificantChangeDeclinedReasonDetails governance = reasons.GetReason(SignificantChangeDeclinedReason.Governance);
      GovernanceIsChecked = governance != null;
      GovernanceDetails = governance?.Details;

      SignificantChangeDeclinedReasonDetails choiceOfTrust = reasons.GetReason(SignificantChangeDeclinedReason.ChoiceOfTrust);
      ChoiceOfTrustIsChecked = choiceOfTrust != null;
      ChoiceOfTrustDetails = choiceOfTrust?.Details;

      SignificantChangeDeclinedReasonDetails other = reasons.GetReason(SignificantChangeDeclinedReason.Other);
      OtherIsChecked = other != null;
      OtherDetails = other?.Details;
   }
}