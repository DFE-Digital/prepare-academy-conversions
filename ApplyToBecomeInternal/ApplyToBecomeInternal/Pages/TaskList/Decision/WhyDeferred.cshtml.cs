using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

		[BindProperty] public string AdditionalInformationNeededDetails { get; set; }
		[BindProperty] public bool AdditionalInformationNeededIsChecked { get; set; }

		[BindProperty] public string LocalSensitivityConcernsDetails { get; set; }
		[BindProperty] public bool LocalSensitivityConcernsIsChecked { get; set; }

		[BindProperty] public string PerformanceConcernsDetails { get; set; }
		[BindProperty] public bool PerformanceConcernsIsChecked { get; set; }

		[BindProperty] public string OtherDetails { get; set; }
		[BindProperty] public bool OtherIsChecked { get; set; }

		[BindProperty]
		public bool WasReasonGiven => AdditionalInformationNeededIsChecked || LocalSensitivityConcernsIsChecked || PerformanceConcernsIsChecked || OtherIsChecked;

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
				.AddReasonIfValid(LocalSensitivityConcernsIsChecked, AdvisoryBoardDeferredReason.LocalSensitivityConcerns, LocalSensitivityConcernsDetails, _errorService)
				.AddReasonIfValid(PerformanceConcernsIsChecked, AdvisoryBoardDeferredReason.PerformanceConcerns, PerformanceConcernsDetails, _errorService)
				.AddReasonIfValid(OtherIsChecked, AdvisoryBoardDeferredReason.Other, OtherDetails, _errorService);

			SetDecisionInSession(id, decision);

			if (!WasReasonGiven) _errorService.AddError($"WasReasonGiven", "Please select at least one reason");
			
			if (_errorService.HasErrors()) return await OnGetAsync(id);			

			return RedirectToPage(Links.Decision.DecisionDate.Page, new { id });
		}

		private void SetReasonsModel(List<AdvisoryBoardDeferredReasonDetails> reasons)
		{
			var additionalInfo = reasons.GetReason(AdvisoryBoardDeferredReason.AdditionalInformationNeeded);
			AdditionalInformationNeededIsChecked = additionalInfo != null;
			AdditionalInformationNeededDetails = additionalInfo?.Details;

			var local = reasons.GetReason(AdvisoryBoardDeferredReason.LocalSensitivityConcerns);
			LocalSensitivityConcernsIsChecked = local != null;
			LocalSensitivityConcernsDetails = local?.Details;

			var perf = reasons.GetReason(AdvisoryBoardDeferredReason.PerformanceConcerns);
			PerformanceConcernsIsChecked = perf != null;
			PerformanceConcernsDetails = perf?.Details;

			var other = reasons.GetReason(AdvisoryBoardDeferredReason.Other);
			OtherIsChecked = other != null;
			OtherDetails = other?.Details;
		}
	}

	public static class AdvisoryBoardExtensions
	{
		public static List<AdvisoryBoardDeferredReasonDetails> AddReasonIfValid(this List<AdvisoryBoardDeferredReasonDetails> reasons, bool isChecked, AdvisoryBoardDeferredReason reason,
			string detail, ErrorService errorService)
		{
			if (isChecked && string.IsNullOrWhiteSpace(detail))
			{
				errorService.AddError($"{reason}Details", $"Explanation is required for {reason.ToDescription().ToLowerInvariant()}");
			}

			if (isChecked) reasons.Add(new AdvisoryBoardDeferredReasonDetails(reason, detail));

			return reasons;
		}

		public static AdvisoryBoardDeferredReasonDetails GetReason(this List<AdvisoryBoardDeferredReasonDetails> reasons, AdvisoryBoardDeferredReason reason)
		{
			return reasons.FirstOrDefault(r => r.Reason == reason);
		}
	}
}
