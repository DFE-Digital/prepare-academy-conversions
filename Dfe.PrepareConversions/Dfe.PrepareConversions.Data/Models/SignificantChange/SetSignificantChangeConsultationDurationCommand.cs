namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeConsultationDurationCommand(
   ConsultationDurationAnswer? consultationLastedMinimumThreeWeeks,
   string consultationDurationNotMetReason)
{
   public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; } = consultationLastedMinimumThreeWeeks;
   public string ConsultationDurationNotMetReason { get; set; } = consultationDurationNotMetReason;
}