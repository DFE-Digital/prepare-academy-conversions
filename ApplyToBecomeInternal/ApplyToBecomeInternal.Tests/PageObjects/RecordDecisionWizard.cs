using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Tests.PageObjects
{
	public class RecordDecisionWizard
	{
		private readonly IBrowsingContext _browsingContext;

		public RecordDecisionWizard(IBrowsingContext browsingContext)
		{
			_browsingContext = browsingContext;
		}

		public IDocument Document => _browsingContext.Active;

		public async Task SubmitThroughTheWizard(AdvisoryBoardDecision request)
		{
			await SelectRadioAndSubmit(request.Decision.ToString().ToLower());
			await SelectRadioAndSubmit(request.DecisionMadeBy.ToString().ToLower());
			await SelectRadioAndSubmit("yes");
			await InputTextAreaAndSubmit(request.ApprovedConditionsDetails);
			await InputDateAndSubmit(request.AdvisoryBoardDecisionDate.Value);
		}

		public async Task StartFor(int projectId)
		{
			await _browsingContext.OpenAsync($"http://localhost/task-list/{projectId}/decision/record-decision");
		}

		/// <summary>
		/// Finds the submit button and clicks it
		/// </summary>
		/// <remarks>
		/// Not required when calling methods with <b>Set</b> prefix.
		/// </remarks>
		/// <returns></returns>
		public async Task ClickSubmitButton()
		{
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}

		public async Task SetDecisionToAndContinue(AdvisoryBoardDecisions decision)
		{
			Document.QuerySelector<IHtmlInputElement>($"#{decision.ToString().ToLowerInvariant()}-radio").IsChecked = true;
			await ClickSubmitButton();
		}

		public async Task SetDecisionByAndContinue(DecisionMadeBy by)
		{
			Document.QuerySelector<IHtmlInputElement>($"#{by.ToString().ToLowerInvariant()}-radio").IsChecked = true;
			await ClickSubmitButton();
		}

		public async Task SetDeclinedReasonsAndContinue(Tuple<AdvisoryBoardDeclinedReasons, string> reason, params Tuple<AdvisoryBoardDeclinedReasons, string>[] furtherReasons)
		{
			foreach ((AdvisoryBoardDeclinedReasons option, string detail) in new[] { reason }.Concat(furtherReasons))
			{
				Document.QuerySelector<IHtmlInputElement>($"#declined-reasons-{option.ToString().ToLowerInvariant()}").IsChecked = true;
				Document.QuerySelector<IHtmlTextAreaElement>($"#reason-{option.ToString().ToLowerInvariant()}").TextContent = detail;
			}

			await ClickSubmitButton();
		}

		public async Task SetDeferredReasonsAndContinue(Tuple<AdvisoryBoardDeferredReason, string> reason, params Tuple<AdvisoryBoardDeferredReason, string>[] furtherReasons)
		{
			foreach ((AdvisoryBoardDeferredReason option, string detail) in new[] { reason }.Concat(furtherReasons))
			{
				var id = $"#{option.ToString().ToLowerInvariant()}";

				Document.QuerySelector<IHtmlInputElement>($"{id}-checkbox").IsChecked = true;
				Document.QuerySelector<IHtmlTextAreaElement>($"{id}-txtarea").TextContent = detail;
			}

			await ClickSubmitButton();
		}

		public async Task SetIsConditionalAndContinue(bool required)
		{
			var controlId = required ? "#yes-radio" : "#no-radio";
			Document.QuerySelector<IHtmlInputElement>(controlId).IsChecked = true;
			await ClickSubmitButton();
		}

		public async Task SpecifyConditionsAndContinue(string conditions)
		{
			Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea").Value = conditions;
			await ClickSubmitButton();
		}

		public async Task SetDecisionDateAndContinue(DateTime date)
		{			
			Document.QuerySelector<IHtmlInputElement>("#-day").Value = date.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#-month").Value = date.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#-year").Value = date.Year.ToString();

			await ClickSubmitButton();
		}

		private async Task SelectRadioAndSubmit(string enumAsString)
		{
			Document.QuerySelector<IHtmlInputElement>($"#{enumAsString}-radio").IsChecked = true;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}

		private async Task InputTextAreaAndSubmit(string conditions)
		{
			Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea").Value = conditions;
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}

		private async Task InputDateAndSubmit(DateTime date)
		{
			Document.QuerySelector<IHtmlInputElement>("#-day").Value = date.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#-month").Value = date.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#-year").Value = date.Year.ToString();
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}
	}
}