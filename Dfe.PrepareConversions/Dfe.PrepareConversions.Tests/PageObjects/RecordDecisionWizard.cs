using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Tests.PageObjects
{
	public class RecordDecisionWizard
	{
		private readonly IBrowsingContext _browsingContext;

		public RecordDecisionWizard(IBrowsingContext browsingContext)
		{
			_browsingContext = browsingContext;
		}

		public IDocument Document => _browsingContext.Active;

		/// <summary>
		///     Steps through the journey using the details in the provided <see cref="AdvisoryBoardDecision" /> instance for each step.
		/// </summary>
		/// <param name="request">An <see cref="AdvisoryBoardDecision" /> configured appropriately for this test run</param>
		/// <returns>Async <see cref="Void"/></returns>
		/// <remarks>Nullable fields are provided with the appropriate default values if not set</remarks>
		public async Task SubmitThroughTheWizard(AdvisoryBoardDecision request)
		{
			await SetDecisionToAndContinue(request.Decision.GetValueOrDefault());
			await SetDecisionByAndContinue(request.DecisionMadeBy.GetValueOrDefault());
			await SetIsConditionalAndContinue(request.ApprovedConditionsSet.GetValueOrDefault(), request.ApprovedConditionsDetails);
			await SetDecisionDateAndContinue(request.AdvisoryBoardDecisionDate.GetValueOrDefault(DateTime.MinValue));
		}

		public async Task StartFor(int projectId)
		{
			await _browsingContext.OpenAsync($"http://localhost/task-list/{projectId}/decision/record-decision");
		}

		/// <summary>
		///     Finds the submit button and clicks it
		/// </summary>
		/// <remarks>
		///     Not required when calling methods with <b>Set</b> prefix.
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
				string id = $"#{option.ToString().ToLowerInvariant()}";

				Document.QuerySelector<IHtmlInputElement>($"{id}-checkbox").IsChecked = true;
				Document.QuerySelector<IHtmlTextAreaElement>($"{id}-txtarea").TextContent = detail;
			}

			await ClickSubmitButton();
		}

		public async Task SetIsConditionalAndContinue(bool required, string conditionDetails)
		{
			string controlId = required ? "#yes-radio" : "#no-radio";
			Document.QuerySelector<IHtmlInputElement>(controlId).IsChecked = true;
			Document.QuerySelector<IHtmlTextAreaElement>("#ApprovedConditionsDetails").Value = conditionDetails;
			await ClickSubmitButton();
		}

		public async Task SpecifyConditionsAndContinue(string conditions)
		{
			Document.QuerySelector<IHtmlTextAreaElement>("#conditions-textarea").Value = conditions;
			await ClickSubmitButton();
		}

		public async Task SetDecisionDateAndContinue(DateTime date)
		{
			Document.QuerySelector<IHtmlInputElement>("#decision-date-day").Value = date.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#decision-date-month").Value = date.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#decision-date-year").Value = date.Year.ToString();

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
			Document.QuerySelector<IHtmlInputElement>("#decision-date-day").Value = date.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#decision-date-month").Value = date.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#decision-date-year").Value = date.Year.ToString();
			await Document.QuerySelector<IHtmlButtonElement>("#submit-btn").SubmitAsync();
		}
	}
}