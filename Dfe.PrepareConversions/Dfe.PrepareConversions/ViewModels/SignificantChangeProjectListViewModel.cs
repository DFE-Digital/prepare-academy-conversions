using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;

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
}