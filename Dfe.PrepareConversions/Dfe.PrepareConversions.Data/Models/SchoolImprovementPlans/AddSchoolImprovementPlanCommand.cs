using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;

public record AddSchoolImprovementPlanCommand(
   int ProjectId,
   List<SchoolImprovementPlanArranger> ArrangedBy,
   string ArrangedByOther,
   string ProvidedBy,
   DateTime StartDate,
   SchoolImprovementPlanExpectedEndDate ExpectedEndDate,
   DateTime ExpectedEndDateOther,
   SchoolImprovementPlanConfidenceLevel ConfidenceLevel,
   string PlanComments);

