using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;

public record UpdateSchoolImprovementPlan(
   int Id,
   int ProjectId,
   List<SchoolImprovementPlanArranger> ArrangedBy,
   string ArrangedByOther,
   string ProvidedBy,
   DateTime StartDate,
   SchoolImprovementPlanExpectedEndDate ExpectedEndDate,
   DateTime? ExpectedEndDateOther,
   SchoolImprovementPlanConfidenceLevel ConfidenceLevel,
   string PlanComments);

public static class UpdateSchoolImprovementPlanExtensions
{
   public static SchoolImprovementPlan ToSchoolImprovementPlan(this UpdateSchoolImprovementPlan plan)
   {
      return new SchoolImprovementPlan
      {
         Id = plan.Id,
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


