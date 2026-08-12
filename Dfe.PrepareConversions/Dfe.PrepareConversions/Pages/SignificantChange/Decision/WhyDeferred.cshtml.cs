
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
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

public class WhyDeferredModel(ISignificantChangeProjectRepository repository,
                              ISession session,
                              ErrorService errorService) : SignificantChangeDecisionBaseModel(repository, session)
{
   [BindProperty] public string AdditionalInformationNeededDetails { get; set; }
   [BindProperty] public bool AdditionalInformationNeededIsChecked { get; set; }

   [BindProperty] public string AwaitingNextOfstedReportDetails { get; set; }
   [BindProperty] public bool AwaitingNextOfstedReportIsChecked { get; set; }

   [BindProperty] public string PerformanceConcernsDetails { get; set; }
   [BindProperty] public bool PerformanceConcernsIsChecked { get; set; }

   [BindProperty] public string OtherDetails { get; set; }
   [BindProperty] public bool OtherIsChecked { get; set; }

   [BindProperty]
   public bool WasReasonGiven => AdditionalInformationNeededIsChecked || AwaitingNextOfstedReportIsChecked || PerformanceConcernsIsChecked || OtherIsChecked;

   public IActionResult OnGet(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(Links.SignificantChange.Decision.RecordDecision, id);
      SetReasonsModel(decision.DeferredReasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      decision.DeferredReasons.Clear();
      decision.DeferredReasons
         .AddReasonIfValid(AdditionalInformationNeededIsChecked, AdvisoryBoardDeferredReason.AdditionalInformationNeeded, AdditionalInformationNeededDetails, ModelState)
         .AddReasonIfValid(AwaitingNextOfstedReportIsChecked, AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, AwaitingNextOfstedReportDetails, ModelState)
         .AddReasonIfValid(PerformanceConcernsIsChecked, AdvisoryBoardDeferredReason.PerformanceConcerns, PerformanceConcernsDetails, ModelState)
         .AddReasonIfValid(OtherIsChecked, AdvisoryBoardDeferredReason.Other, OtherDetails, ModelState);

      SetDecisionInSession(id, decision);

      if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

      errorService.AddErrors(ModelState.Keys, ModelState);
      if (errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.SignificantChange.Decision.WhoDecided.Page, LinkParameters);
   }

   private void SetReasonsModel(List<AdvisoryBoardDeferredReasonDetails> reasons)
   {
      AdvisoryBoardDeferredReasonDetails additionalInfo = reasons.GetReason(AdvisoryBoardDeferredReason.AdditionalInformationNeeded);
      AdditionalInformationNeededIsChecked = additionalInfo != null;
      AdditionalInformationNeededDetails = additionalInfo?.Details;

      AdvisoryBoardDeferredReasonDetails ofsted = reasons.GetReason(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport);
      AwaitingNextOfstedReportIsChecked = ofsted != null;
      AwaitingNextOfstedReportDetails = ofsted?.Details;

      AdvisoryBoardDeferredReasonDetails perf = reasons.GetReason(AdvisoryBoardDeferredReason.PerformanceConcerns);
      PerformanceConcernsIsChecked = perf != null;
      PerformanceConcernsDetails = perf?.Details;

      AdvisoryBoardDeferredReasonDetails other = reasons.GetReason(AdvisoryBoardDeferredReason.Other);
      OtherIsChecked = other != null;
      OtherDetails = other?.Details;
   }
}