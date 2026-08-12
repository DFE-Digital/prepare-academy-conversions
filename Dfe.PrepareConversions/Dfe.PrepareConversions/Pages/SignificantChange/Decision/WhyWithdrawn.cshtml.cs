
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

public class WhyWithdrawnModel(ISignificantChangeProjectRepository repository,
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
      SetReasonsModel(decision.WithdrawnReasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      decision.WithdrawnReasons.Clear();
      decision.WithdrawnReasons
         .AddReasonIfValid(AdditionalInformationNeededIsChecked, AdvisoryBoardWithdrawnReason.AdditionalInformationNeeded, AdditionalInformationNeededDetails, ModelState)
         .AddReasonIfValid(AwaitingNextOfstedReportIsChecked, AdvisoryBoardWithdrawnReason.AwaitingNextOfstedReport, AwaitingNextOfstedReportDetails, ModelState)
         .AddReasonIfValid(PerformanceConcernsIsChecked, AdvisoryBoardWithdrawnReason.PerformanceConcerns, PerformanceConcernsDetails, ModelState)
         .AddReasonIfValid(OtherIsChecked, AdvisoryBoardWithdrawnReason.Other, OtherDetails, ModelState);

      SetDecisionInSession(id, decision);

      if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

      errorService.AddErrors(ModelState.Keys, ModelState);
      if (errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.SignificantChange.Decision.WhoDecided.Page, LinkParameters);
   }

   private void SetReasonsModel(List<AdvisoryBoardWithdrawnReasonDetails> reasons)
   {
      AdvisoryBoardWithdrawnReasonDetails additionalInfo = reasons.GetReason(AdvisoryBoardWithdrawnReason.AdditionalInformationNeeded);
      AdditionalInformationNeededIsChecked = additionalInfo != null;
      AdditionalInformationNeededDetails = additionalInfo?.Details;

      AdvisoryBoardWithdrawnReasonDetails ofsted = reasons.GetReason(AdvisoryBoardWithdrawnReason.AwaitingNextOfstedReport);
      AwaitingNextOfstedReportIsChecked = ofsted != null;
      AwaitingNextOfstedReportDetails = ofsted?.Details;

      AdvisoryBoardWithdrawnReasonDetails perf = reasons.GetReason(AdvisoryBoardWithdrawnReason.PerformanceConcerns);
      PerformanceConcernsIsChecked = perf != null;
      PerformanceConcernsDetails = perf?.Details;

      AdvisoryBoardWithdrawnReasonDetails other = reasons.GetReason(AdvisoryBoardWithdrawnReason.Other);
      OtherIsChecked = other != null;
      OtherDetails = other?.Details;
   }
}