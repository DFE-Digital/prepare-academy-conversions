
namespace Dfe.PrepareConversions.Data.Models.SignificantChange
{
	public class SignificantChangeAdmissionVariationConsultationResponse
	{
		public bool? ConsultationIncludeAdmissionVariation { get; set; }
      public string ConsultationNoAdmissionVariationReason { get; set; } = string.Empty;
      public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
   }
}
