namespace ApplyToBecome.Data.Models.AcademyConversion
{
	public class LegalRequirements
	{
		public bool IsComplete { get; set; }
		public ThreeOptions? GoverningBodyApproved { get; set; }
		public ThreeOptions? ConsultationDone { get; set; }
		public ThreeOptions? DiocesanConsent { get; set; }
		public ThreeOptions? FoundationConsent { get; set; }

		public Status Status
		{
			get
			{
				if (IsComplete) return Status.Completed;

				if (GoverningBodyApproved.HasValue ||
				    ConsultationDone.HasValue ||
				    DiocesanConsent.HasValue ||
				    FoundationConsent.HasValue)
					return Status.InProgress;

				return Status.NotStarted;
			}
		}
	}
}
