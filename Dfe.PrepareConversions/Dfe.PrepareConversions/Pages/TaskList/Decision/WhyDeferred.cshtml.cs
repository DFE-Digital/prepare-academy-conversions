using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class WhyDeferredModel : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public WhyDeferredModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty]
   public string AdditionalInformationNeededDetails { get; set; }

   [BindProperty]
   public bool AdditionalInformationNeededIsChecked { get; set; }

   [BindProperty]
   public string AwaitingNextOfstedReportDetails { get; set; }

   [BindProperty]
   public bool AwaitingNextOfstedReportIsChecked { get; set; }

   [BindProperty]
   public string PerformanceConcernsDetails { get; set; }

   [BindProperty]
   public bool PerformanceConcernsIsChecked { get; set; }

   [BindProperty]
   public string OtherDetails { get; set; }

   [BindProperty]
   public bool OtherIsChecked { get; set; }

   [BindProperty]
   public bool WasReasonGiven => AdditionalInformationNeededIsChecked || AwaitingNextOfstedReportIsChecked || PerformanceConcernsIsChecked || OtherIsChecked;

   public string DecisionText { get; set; }

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.WhoDecided, id);

      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      DecisionText = decision.Decision.ToDescription().ToLowerInvariant();

      List<AdvisoryBoardDeferredReasonDetails> reasons = decision.DeferredReasons;
      SetReasonsModel(reasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);

      decision.DeferredReasons.Clear();
      decision.DeferredReasons
         .AddReasonIfValid(AdditionalInformationNeededIsChecked, AdvisoryBoardDeferredReason.AdditionalInformationNeeded, AdditionalInformationNeededDetails, ModelState)
         .AddReasonIfValid(AwaitingNextOfstedReportIsChecked, AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, AwaitingNextOfstedReportDetails, ModelState)
         .AddReasonIfValid(PerformanceConcernsIsChecked, AdvisoryBoardDeferredReason.PerformanceConcerns, PerformanceConcernsDetails, ModelState)
         .AddReasonIfValid(OtherIsChecked, AdvisoryBoardDeferredReason.Other, OtherDetails, ModelState);

      SetDecisionInSession(id, decision);

      if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.Decision.DecisionMaker.Page, LinkParameters);
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
