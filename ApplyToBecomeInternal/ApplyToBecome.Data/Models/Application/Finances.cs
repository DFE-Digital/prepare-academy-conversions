namespace ApplyToBecome.Data.Models.Application
{
	public class Finances
	{
		public FinancialYear PreviousFinancialYear { get; set; }
		public FinancialYear CurrentFinancialYear { get; set; }
		public FinancialYear NextFinancialYear { get; set; }
		public bool ExistingLoans { get; set; }
		public bool ExistingLeases { get; set; }
		public bool OngoingInvestigations { get; set; }
	}
}