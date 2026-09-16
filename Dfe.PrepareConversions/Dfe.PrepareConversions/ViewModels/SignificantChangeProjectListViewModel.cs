using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System;

namespace Dfe.PrepareConversions.ViewModels;

public class SignificantChangeProjectViewBaseModel
{
   public int Id { get; set; }
   public int Urn { get; set; }
   public required string SchoolName { get; set; }
   public byte Tier { get; set; }
   public required string TrustName { get; set; }
   public required string TrustUkprn { get; set; }
   public User AssignedUser { get; set; }
   public required string TypeOfSignificantChange { get; set; }
   public required string Status { get; set; }
   public required string StatusColour { get; set; }
   public SignificantChangeTaskStatus StakeholderConsultationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? StakeholderConsultationTrustConsultedStakeholders { get; set; }
   public string StakeholderConsultationTrustConsultedStakeholdersNotConsultedReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus AdmissionVariationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? ConsultationIncludeAdmissionVariation { get; set; }
   public string ConsultationNoAdmissionVariationReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus EqualitiesImpactAssessmentStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? EqualitiesImpactAssessmentCompleted { get; set; }
   public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
   public string EqualitiesImpactIdentifiedMitigation { get; set; } = string.Empty;
   public SignificantChangeTaskStatus ReligiousBodyConsultationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? ReligiousBodyConsultationTrustConsultedReligiousBody { get; set; }
   public string ReligiousBodyConsultationTrustConsultedReligiousBodyNotConsultedReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus ProjectDatesStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public DateTime? ProposedDecisionDate { get; set; }
   public DateTime? ProposedChangeDate { get; set; }
}