using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeProjectResponse : SignificantChangeProjectSearchResponse
{
   public SignificantChangeStakeholderConsultationResponse StakeholderConsultation { get; set; } = new();
   public SignificantChangeStakeholderObjectionsResponse StakeholderObjections { get; set; } = new();
   public SignificantChangeConsultationDurationResponse ConsultationDuration { get; set; } = new();
   public SignificantChangeAdmissionVariationConsultationResponse AdmissionVariationConsultation { get; set;} = new();
   public SignificantChangeEqualitiesImpactAssessmentResponse EqualitiesImpactAssessment { get; set; } = new();
   public SignificantChangeReligiousBodyConsultationResponse ReligiousBodyConsultation { get; set; } = new();
   public SignificantChangeProjectDatesResponse ProjectDates { get; set; } = new();
   public SignificantChangeAdmissionsVariationResponse AdmissionsVariation { get; set; } = new();
   public SignificantChangeFundingResponse Funding { get; set; } = new();
   public SignificantChangePlanningPermissionResponse PlanningPermission { get; set; } = new();
   public SignificantChangeRecommendationResponse Recommendation { get; set; } = new();
   public SignificantChangeHqtfConsiderationResponse HQTFConsideration { get; set; } = new();
   public SignificantChangeOfstedInspectionResponse OfstedInspection { get; set; } = new();
   public SignificantChangeDecisions? Decision { get; set; }
   public DateTime? DecisionDate { get; set; }
}
