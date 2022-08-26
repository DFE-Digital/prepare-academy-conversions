using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhyDeferredModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public WhyDeferredModel(IAcademyConversionProjectRepository repository, ISession session,
			ErrorService errorService)
			: base(repository, session)
		{
			_errorService = errorService;
		}

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

		public string DecisionText { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);

			AdvisoryBoardDecision decision = GetDecisionFromSession(id);
			DecisionText = decision.Decision.ToDescription().ToLowerInvariant();

			var reasons = decision.DeferredReasons;
			SetReasonsModel(reasons);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession(id);

			decision.DeferredReasons.Clear();
			decision.DeferredReasons
				.AddReasonIfValid(AdditionalInformationNeededIsChecked, AdvisoryBoardDeferredReason.AdditionalInformationNeeded, AdditionalInformationNeededDetails, _errorService)
				.AddReasonIfValid(AwaitingNextOfstedReportIsChecked, AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, AwaitingNextOfstedReportDetails, _errorService)
				.AddReasonIfValid(PerformanceConcernsIsChecked, AdvisoryBoardDeferredReason.PerformanceConcerns, PerformanceConcernsDetails, _errorService)
				.AddReasonIfValid(OtherIsChecked, AdvisoryBoardDeferredReason.Other, OtherDetails, _errorService);

			SetDecisionInSession(id, decision);

			if (!WasReasonGiven) _errorService.AddError($"WasReasonGiven", "Select at least one reason");
			
			if (_errorService.HasErrors()) return await OnGetAsync(id);			

			return RedirectToPage(Links.Decision.DecisionDate.Page, new { id });
		}

		private void SetReasonsModel(List<AdvisoryBoardDeferredReasonDetails> reasons)
		{
			var additionalInfo = reasons.GetReason(AdvisoryBoardDeferredReason.AdditionalInformationNeeded);
			AdditionalInformationNeededIsChecked = additionalInfo != null;
			AdditionalInformationNeededDetails = additionalInfo?.Details;

			var ofsted = reasons.GetReason(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport);
			AwaitingNextOfstedReportIsChecked = ofsted != null;
			AwaitingNextOfstedReportDetails = ofsted?.Details;

			var perf = reasons.GetReason(AdvisoryBoardDeferredReason.PerformanceConcerns);
			PerformanceConcernsIsChecked = perf != null;
			PerformanceConcernsDetails = perf?.Details;

			var other = reasons.GetReason(AdvisoryBoardDeferredReason.Other);
			OtherIsChecked = other != null;
			OtherDetails = other?.Details;
		}
	}
}
