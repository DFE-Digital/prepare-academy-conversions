using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FinanceSection : BaseFormSection
	{
		public FinanceSection(Application application) : base("Finances")
		{
			SubSections = new[]
			{
				new FormSubSection("Previous financial year", GeneratePreviousFinancialYearFields(application)),
				new FormSubSection("Current financial year", GenerateCurrentFinancialYearFields(application)),
				new FormSubSection("Next financial year", GenerateNextFinancialYearFields(application)),
				new FormSubSection("Loans", GenerateLoansFields(application)),
				new FormSubSection("Financial leases", GenerateFinancialLeasesFields(application)),
				new FormSubSection("Financial investigations", GenerateFinancialInvestigationsFields(application))
			};
		}

		private IEnumerable<FormField> GeneratePreviousFinancialYearFields(Application application) =>
			new[]
			{
				new FormField("End of previous financial year", application.Finances.PreviousFinancialYear.Date.ToUkDateString()),
				new FormField("Revenue carry forward at the end of the previous financial year (31 March)", application.Finances.PreviousFinancialYear.Value),
				new FormField("Surplus or deficit?", application.Finances.PreviousFinancialYear.State.ToString())
			};

		private IEnumerable<FormField> GenerateCurrentFinancialYearFields(Application application) =>
			new[]
			{
				new FormField("End of current financial year", application.Finances.CurrentFinancialYear.Date.ToUkDateString()),
				new FormField("Forecasted revenue carry forward at the end of the current financial year (31 March)", application.Finances.CurrentFinancialYear.Value),
				new FormField("Surplus or deficit?", application.Finances.CurrentFinancialYear.State.ToString()),
				new FormField("Forecasted capital carry forward at the end of the current financial year (31 March)", application.Finances.CurrentFinancialYear.CarryForward),
				new FormField("Surplus or deficit?", application.Finances.CurrentFinancialYear.CarryForwardState.ToString())
			};

		private IEnumerable<FormField> GenerateNextFinancialYearFields(Application application) =>
			new[]
			{
				new FormField("End of next financial year", application.Finances.NextFinancialYear.Date.ToUkDateString()),
				new FormField("Forecasted revenue carry forward at the end of the next financial year (31 March)", application.Finances.NextFinancialYear.Value),
				new FormField("Surplus or deficit?", application.Finances.NextFinancialYear.State.ToString()),
				new FormField("Forecasted capital carry forward at the end of the next financial year (31 March)", application.Finances.NextFinancialYear.CarryForward),
				new FormField("Surplus or deficit?", application.Finances.NextFinancialYear.CarryForwardState.ToString())
			};

		private IEnumerable<FormField> GenerateLoansFields(Application application) =>
			new[]
			{
				new FormField("Are there any existing loans?", application.Finances.ExistingLoans.ToYesNoString())
			};

		private IEnumerable<FormField> GenerateFinancialLeasesFields(Application application) =>
			new[]
			{
				new FormField("Are there any existing leases?", application.Finances.ExistingLeases.ToYesNoString())
			};

		private IEnumerable<FormField> GenerateFinancialInvestigationsFields(Application application) =>
			new[]
			{
				new FormField("Are there any financial investigations ongoing at the school?", application.Finances.OngoingInvestigations.ToYesNoString())
			};
	}
}