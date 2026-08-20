using Dfe.PrepareConversions.ViewModels;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models;

public record FinancialHealthAssessmentPrerequisite(string Description, LinkItem Link);

public static class FinancialHealthAssessmentPrerequisites
{
   public static IReadOnlyList<FinancialHealthAssessmentPrerequisite> GetMissing(ProjectViewModel project)
   {
      List<FinancialHealthAssessmentPrerequisite> missing = [];

      if (project is null)
      {
         return missing;
      }

      if (!project.HeadTeacherBoardDate.HasValue)
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a proposed decision date",
            Links.ProjectDates.ConfirmProjectDates));
      }

      if (!project.ProposedConversionDate.HasValue)
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a proposed conversion date",
            Links.ProjectDates.PropsedConversionDate));
      }

      if (!project.RevenueCarryForwardAtEndMarchCurrentYear.HasValue)
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a forecasted revenue carry forward at the end of the current financial year",
            Links.SchoolBudgetInformationSection.UpdateSchoolBudgetInformation));
      }

      if (!project.CapitalCarryForwardAtEndMarchCurrentYear.HasValue)
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a forecasted capital carry forward at the end of the current financial year",
            Links.SchoolBudgetInformationSection.UpdateSchoolBudgetInformation));
      }

      if (!project.ProjectedRevenueBalanceAtEndMarchNextYear.HasValue)
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a forecasted revenue carry forward at the end of the next financial year",
            Links.SchoolBudgetInformationSection.UpdateSchoolBudgetInformation));
      }

      if (!project.CapitalCarryForwardAtEndMarchNextYear.HasValue)
      {
         missing.Add(new FinancialHealthAssessmentPrerequisite(
            "a forecasted capital carry forward at the end of the next financial year",
            Links.SchoolBudgetInformationSection.UpdateSchoolBudgetInformation));
      }

      return missing;
   }

   public static bool IsComplete(ProjectViewModel project) => GetMissing(project).Count == 0;
}