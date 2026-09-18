namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeAdmissionVariationConsultationCommand(
   bool? ConsultationIncludeAdmissionVariation,
   string NoAdmissionVariationReason);
