using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using System;
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
