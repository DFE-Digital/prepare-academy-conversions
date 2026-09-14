using Dfe.PrepareTransfers.Data.Models;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Web.Models;

/// <summary>
/// One piece of mandatory information an FHA request depends on.
/// </summary>
/// <param name="Description">Lower-case fragment, rendered after "you need to enter".</param>
/// <param name="PageName">Razor page to link to; the view supplies the urn.</param>
public record FinancialHealthAssessmentPrerequisite(string Description, string PageName);

/// <summary>
/// The mandatory information that must be present before the Academisation API will
/// request a Financial Health Assessment for a transfer (user story 298244).
///
/// Budget/carry-forward figures are deliberately absent: transfer projects hold no
/// budget data in Prepare or in the Academisation API, so the budget gate applies to
/// conversions only. Add the four checks here if transfers gain budget capture.
/// </summary>
public static class FinancialHealthAssessmentPrerequisites
{
   public static IReadOnlyList<FinancialHealthAssessmentPrerequisite> GetMissing(Project project)
   {
      List<FinancialHealthAssessmentPrerequisite> missing = [];

      if (project is null)
      {
         return missing;
      }

      if (string.IsNullOrEmpty(project.Dates?.Htb))
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a proposed decision date", "/Projects/TransferDates/Index"));
      }

      // Target is a dd/MM/yyyy string. HasTargetDateForTransfer == false means the user
      // explicitly answered "I do not know this" — that does not satisfy the AC, so it
      // still counts as missing (assumption A4).
      if (string.IsNullOrEmpty(project.Dates?.Target))
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a proposed transfer date", "/Projects/TransferDates/Target"));
      }

      return missing;
   }

   public static bool IsComplete(Project project) => GetMissing(project).Count == 0;
}