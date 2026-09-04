namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeConsultationDurationResponse
{
   public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
   public string ConsultationDurationNotMetReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}