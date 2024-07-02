using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;

public record AddSchoolImprovementPlan(
   int ProjectId,
   List<SchoolImprovementPlanArranger> ArrangedBy,
   string ArrangedByOther,
   string ProvidedBy,
   DateTime StartDate,
   SchoolImprovementPlanExpectedEndDate ExpectedEndDate,
   DateTime? ExpectedEndDateOther,
   SchoolImprovementPlanConfidenceLevel ConfidenceLevel,
   string PlanComments);

public static class AddSchoolImprovementPlanExtensions
{
   public static SchoolImprovementPlan ToSchoolImprovementPlan(this AddSchoolImprovementPlan plan)
   {
      return new SchoolImprovementPlan
      {
         ProjectId = plan.ProjectId,
         ArrangedBy = plan.ArrangedBy,
         ArrangedByOther = plan.ArrangedByOther,
         ProvidedBy = plan.ProvidedBy,
         StartDate = plan.StartDate,
         ExpectedEndDate = plan.ExpectedEndDate,
         ExpectedEndDateOther = plan.ExpectedEndDateOther,
         ConfidenceLevel = plan.ConfidenceLevel,
         PlanComments = plan.PlanComments
      };
   }
}


