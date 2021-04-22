namespace ApplyToBecome.Data.Models.Application
{
	public class Finances
	{
		public FinancialYear PreviousFinancialYear { get; set; }
		public ForecastFinancialYear CurrentFinancialYear { get; set; }
		public ForecastFinancialYear NextFinancialYear { get; set; }
		public bool ExistingLoans { get; set; }
		public bool ExistingLeases { get; set; }
		public bool OngoingInvestigations { get; set; }
	}
}