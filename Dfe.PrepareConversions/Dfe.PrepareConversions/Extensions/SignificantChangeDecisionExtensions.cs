
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Extensions;

public static class SignificantChangeDecisionExtensions
{
   public static List<SignificantChangeDeclinedReasonDetails> AddReasonIfValid(
      this List<SignificantChangeDeclinedReasonDetails> reasons,
      bool isChecked,
      SignificantChangeDeclinedReason reason,
      string detail,
      ModelStateDictionary modelState)
   {
      if (isChecked && string.IsNullOrWhiteSpace(detail))
      {
         modelState.AddModelError($"{reason}Details", $"Enter a reason for selecting {reason.ToDescription()}");
      }

      if (isChecked) reasons.Add(new SignificantChangeDeclinedReasonDetails(reason, detail));

      return reasons;
   }

   public static SignificantChangeDeclinedReasonDetails GetReason(
      this List<SignificantChangeDeclinedReasonDetails> reasons,
      SignificantChangeDeclinedReason reason)
   {
      return reasons.FirstOrDefault(r => r.Reason == reason);
   }
}