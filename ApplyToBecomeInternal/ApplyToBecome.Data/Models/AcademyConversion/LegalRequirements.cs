namespace ApplyToBecome.Data.Models.AcademyConversion
{
	public class LegalRequirements
	{
		public bool IsComplete { get; set; }
		public ThreeOptions? GoverningBodyApproved { get; set; }
		public ThreeOptions? ConsultationDone { get; set; }
		public ThreeOptions? DiocesanConsent { get; set; }
		public ThreeOptions? FoundationConsent { get; set; }

		public Status Status =>
			IsComplete ? Status.Complete :
			GoverningBodyApproved.HasValue ||
			ConsultationDone.HasValue ||
			DiocesanConsent.HasValue ||
			FoundationConsent.HasValue ? Status.InProgress :
			Status.NotStarted;
	}
}
