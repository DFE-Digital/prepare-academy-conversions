using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

		// TODO: move these properties out into separate class. Add required if attribute			
		[BindProperty] public string AdditionalInformationNeededDetails { get; set; }
		[BindProperty] public bool AdditionalInformationNeededIsChecked { get; set; }

		[BindProperty] public string LocalSensitivityConcernsDetails { get; set; }
		[BindProperty] public bool LocalSensitivityConcernsIsChecked { get; set; }

		[BindProperty] public string TrustToEngageMoreWithStakeholdersDetails { get; set; }
		[BindProperty] public bool TrustToEngageMoreWithStakeholdersIsChecked { get; set; }

		[BindProperty] public string PerformanceConcernsDetails { get; set; }
		[BindProperty] public bool PerformanceConcernsIsChecked { get; set; }

		[BindProperty] public string OtherDetails { get; set; }
		[BindProperty] public bool OtherIsChecked { get; set; }
		
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
				.AddReason(LocalSensitivityConcernsIsChecked, AdvisoryBoardDeferredReason.LocalSensitivityConcerns, LocalSensitivityConcernsDetails)
				.AddReason(PerformanceConcernsIsChecked, AdvisoryBoardDeferredReason.PerformanceConcerns, PerformanceConcernsDetails)
				.AddReason(TrustToEngageMoreWithStakeholdersIsChecked, AdvisoryBoardDeferredReason.TrustToEngageMoreWithStakeholders, TrustToEngageMoreWithStakeholdersDetails)
				.AddReason(OtherIsChecked, AdvisoryBoardDeferredReason.Other, OtherDetails);

			SetDecisionInSession(id, decision);

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

			var ofsted = reasons.GetReason(AdvisoryBoardDeferredReason.TrustToEngageMoreWithStakeholders);
			TrustToEngageMoreWithStakeholdersIsChecked = ofsted != null;
			TrustToEngageMoreWithStakeholdersDetails = ofsted?.Details;

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
