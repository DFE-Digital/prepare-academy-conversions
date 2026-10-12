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
   public SignificantChangeFundingResponse Funding { get; set; } = new();
   public SignificantChangePlanningPermissionResponse PlanningPermission { get; set; } = new();
}
