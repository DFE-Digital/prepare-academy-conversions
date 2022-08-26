using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Extensions
{
	public static class AdvisoryBoardExtensions
	{
		public static List<AdvisoryBoardDeferredReasonDetails> AddReasonIfValid(this List<AdvisoryBoardDeferredReasonDetails> reasons, bool isChecked, AdvisoryBoardDeferredReason reason,
			string detail, ModelStateDictionary modelState)
		{
			if (isChecked && string.IsNullOrWhiteSpace(detail))
			{
				modelState.AddModelError($"conditional-{reason}", $"Enter a reason for selecting {reason.ToDescription()}");
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