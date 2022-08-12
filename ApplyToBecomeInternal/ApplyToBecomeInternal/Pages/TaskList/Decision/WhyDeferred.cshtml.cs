using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

		// TODO: move these properties out into separate class. Add required if attribute.
		[BindProperty] public string AdditionalInformationNeededDetails { get; set; }
		[BindProperty] public bool AdditionalInformationNeededIsChecked { get; set; }

		[BindProperty] public string AwaitingNextOfstedReportDetails { get; set; }
		[BindProperty] public bool AwaitingNextOfstedReportIsChecked { get; set; }

		[BindProperty] public string PerformanceConcernsDetails { get; set; }
		[BindProperty] public bool PerformanceConcernsIsChecked { get; set; }

		[BindProperty] public string OtherDetails { get; set; }
		[BindProperty] public bool OtherIsChecked { get; set; }


		public IEnumerable<AdvisoryBoardDeferredReason> DecisionMadeByOptions => Enum.GetValues(typeof(AdvisoryBoardDeferredReason))
																					 .Cast<AdvisoryBoardDeferredReason>();

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);

			var reasons = GetDecisionFromSession(id).DeferredReasons;
			SetReasonsModel(reasons);

			return Page();
		}		

		public async Task<IActionResult> OnPostAsync(int id)
		{	
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(Request.Form.Keys, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id);

			decision.DeferredReasons.Clear();
			decision.DeferredReasons
				.AddReason(AdditionalInformationNeededIsChecked, AdvisoryBoardDeferredReason.AdditionalInformationNeeded, AdditionalInformationNeededDetails)
				.AddReason(AwaitingNextOfstedReportIsChecked, AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, AwaitingNextOfstedReportDetails)
				.AddReason(PerformanceConcernsIsChecked, AdvisoryBoardDeferredReason.PerformanceConcerns, PerformanceConcernsDetails)
				.AddReason(OtherIsChecked, AdvisoryBoardDeferredReason.Other, OtherDetails);

			SetDecisionInSession(id, decision);

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

	// TODO: move to another file
	public static class AdvisoryBoardExtensions
	{
		public static List<AdvisoryBoardDeferredReasonDetails> AddReason(this List<AdvisoryBoardDeferredReasonDetails> reasons, bool isChecked, AdvisoryBoardDeferredReason reason, string detail)
		{
			if (isChecked) reasons.Add(new AdvisoryBoardDeferredReasonDetails(reason, detail));

			return reasons;
		}

		public static AdvisoryBoardDeferredReasonDetails GetReason(this List<AdvisoryBoardDeferredReasonDetails> reasons, AdvisoryBoardDeferredReason reason)
		{
			return reasons.FirstOrDefault(r => r.Reason == reason);
		}
	}
}
