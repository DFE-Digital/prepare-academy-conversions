using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Tests.PageObjects;

public class SignificantChangeDecisionWizard
{
   private readonly IBrowsingContext _browsingContext;

   public SignificantChangeDecisionWizard(IBrowsingContext browsingContext)
   {
      _browsingContext = browsingContext;
   }

   private IDocument Document => _browsingContext.Active;

   public async Task StartFor(int projectId)
   {
      await _browsingContext.OpenAsync($"https://localhost/significant-change/{projectId}/decision/record-decision");
   }

   public async Task ClickSubmitButton()
   {
      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();
   }

   public async Task SetDecisionToAndContinue(SignificantChangeDecisions decision)
   {
      Document.QuerySelector<IHtmlInputElement>($"#{decision.ToString().ToLowerInvariant()}-radio")!.IsChecked = true;
      await ClickSubmitButton();
   }

   public async Task SetIsConditionalAndContinue(bool conditionsSet, string conditionDetails = null)
   {
      Document.QuerySelector<IHtmlInputElement>(conditionsSet ? "#yes-radio" : "#no-radio")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#ApprovedConditionsDetails")!.Value = conditionDetails ?? string.Empty;
      await ClickSubmitButton();
   }

   public async Task SetDeclinedReasonsAndContinue(params (SignificantChangeDeclinedReason Reason, string Details)[] reasons)
   {
      foreach ((SignificantChangeDeclinedReason reason, string details) in reasons)
      {
         CheckReason(reason.ToString(), details);
      }

      await ClickSubmitButton();
   }

   public async Task SetDeferredReasonsAndContinue(params (AdvisoryBoardDeferredReason Reason, string Details)[] reasons)
   {
      foreach ((AdvisoryBoardDeferredReason reason, string details) in reasons)
      {
         CheckReason(reason.ToString(), details);
      }

      await ClickSubmitButton();
   }

   public async Task SetWithdrawnReasonsAndContinue(params (AdvisoryBoardWithdrawnReason Reason, string Details)[] reasons)
   {
      foreach ((AdvisoryBoardWithdrawnReason reason, string details) in reasons)
      {
         CheckReason(reason.ToString(), details);
      }

      await ClickSubmitButton();
   }

   public async Task SetDecisionByAndContinue(DecisionMadeBy decisionMadeBy)
   {
      Document.QuerySelector<IHtmlInputElement>($"#{decisionMadeBy.ToString().ToLowerInvariant()}-radio")!.IsChecked = true;
      await ClickSubmitButton();
   }

   public async Task SetDecisionMakerNameAndContinue(string name)
   {
      Document.QuerySelector<IHtmlInputElement>("#decision-maker-name")!.Value = name;
      await ClickSubmitButton();
   }

   public async Task SetDecisionDateAndContinue(DateTime date)
   {
      Document.QuerySelector<IHtmlInputElement>("#decision-date-day")!.Value = date.Day.ToString();
      Document.QuerySelector<IHtmlInputElement>("#decision-date-month")!.Value = date.Month.ToString();
      Document.QuerySelector<IHtmlInputElement>("#decision-date-year")!.Value = date.Year.ToString();
      await ClickSubmitButton();
   }

   private void CheckReason(string reasonName, string details)
   {
      string id = $"#{reasonName.ToLowerInvariant()}";
      Document.QuerySelector<IHtmlInputElement>($"{id}-checkbox")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>($"{id}-txtarea")!.Value = details;
   }
}