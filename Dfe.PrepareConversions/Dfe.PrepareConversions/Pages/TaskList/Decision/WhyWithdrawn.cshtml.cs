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

public class WhyWithdrawnModel : DecisionBaseModel
{
    private readonly ErrorService _errorService;

    public WhyWithdrawnModel(IAcademyConversionProjectRepository repository,
                           ISession session,
                           ErrorService errorService
       )
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

      List<AdvisoryBoardWithdrawnReasonDetails> reasons = decision.WithdrawnReasons;
      SetReasonsModel(reasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
        AdvisoryBoardDecision decision = GetDecisionFromSession(id);

        decision.WithdrawnReasons.Clear();
        decision.WithdrawnReasons
           .AddReasonIfValid(AdditionalInformationNeededIsChecked, AdvisoryBoardWithdrawnReason.AdditionalInformationNeeded, AdditionalInformationNeededDetails, ModelState)
           .AddReasonIfValid(AwaitingNextOfstedReportIsChecked, AdvisoryBoardWithdrawnReason.AwaitingNextOfstedReport, AwaitingNextOfstedReportDetails, ModelState)
           .AddReasonIfValid(PerformanceConcernsIsChecked, AdvisoryBoardWithdrawnReason.PerformanceConcerns, PerformanceConcernsDetails, ModelState)
           .AddReasonIfValid(OtherIsChecked, AdvisoryBoardWithdrawnReason.Other, OtherDetails, ModelState);

        SetDecisionInSession(id, decision);

        if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

        _errorService.AddErrors(ModelState.Keys, ModelState);
        if (_errorService.HasErrors()) return OnGet(id);

        return RedirectToPage(Links.Decision.DecisionDate.Page, LinkParameters);
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