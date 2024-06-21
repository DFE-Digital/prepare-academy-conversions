using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;

public class SchoolImprovementPlan { 
   public SchoolImprovementPlan()
   {
      ArrangedBy = new List<SchoolImprovementPlanArranger>();
      StartDate = null;
      ExpectedEndDateOther = null;
   }

   public int ProjectId { get; set; }
   public List<SchoolImprovementPlanArranger> ArrangedBy { get; set; }
   public string ArrangedByOther { get; set; }
   public string ProvidedBy { get; set; }
   public DateTime? StartDate { get; set; }
   public SchoolImprovementPlanExpectedEndDate? ExpectedEndDate { get; set; }
   public DateTime? ExpectedEndDateOther { get; set; }
   public SchoolImprovementPlanConfidenceLevel ConfidenceLevel { get; set; }
   public string PlanComments { get; set; }
} 
