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
				new FormField("End of previous financial year", application.SchoolPFYEndDate.Value.ToUkDateString()),
				new FormField("Revenue carry forward at the end of the previous financial year (31 March)", application.SchoolPFYRevenue.ToString()),
				new FormField("Surplus or deficit?", application.SchoolPFYRevenueStatus.ToSurplusDeficitString())
			};

		private IEnumerable<FormField> GenerateCurrentFinancialYearFields(Application application) =>
			new[]
			{
				new FormField("End of current financial year", application.SchoolCFYEndDate.Value.ToUkDateString()),
				new FormField("Forecasted revenue carry forward at the end of the current financial year (31 March)", application.SchoolCFYRevenue.ToString()),
				new FormField("Surplus or deficit?", application.SchoolCFYRevenueStatus.ToSurplusDeficitString()),
				new FormField("Forecasted capital carry forward at the end of the current financial year (31 March)", application.SchoolCFYCapitalForward.ToString()),
				new FormField("Surplus or deficit?", application.SchoolCFYCapitalForwardStatus.ToSurplusDeficitString())
			};

		private IEnumerable<FormField> GenerateNextFinancialYearFields(Application application) =>
			new[]
			{
				new FormField("End of next financial year", application.SchoolNFYEndDate.Value.ToUkDateString()),
				new FormField("Forecasted revenue carry forward at the end of the next financial year (31 March)", application.SchoolNFYRevenue.ToString()),
				new FormField("Surplus or deficit?", application.SchoolNFYRevenueStatus.ToSurplusDeficitString()),
				new FormField("Forecasted capital carry forward at the end of the next financial year (31 March)", application.SchoolNFYCapitalForward.ToString()),
				new FormField("Surplus or deficit?", application.SchoolNFYCapitalForwardStatus.ToSurplusDeficitString())
			};

		private IEnumerable<FormField> GenerateLoansFields(Application application) =>
			new[]
			{
				new FormField("Are there any existing loans?", application.SchoolLoanExists.Id.ToYesNoString()) // CML id or name?
			};

		private IEnumerable<FormField> GenerateFinancialLeasesFields(Application application) =>
			new[]
			{
				new FormField("Are there any existing leases?", application.SchoolLeaseExists.Id.ToYesNoString()) // CML id or name?
			};

		private IEnumerable<FormField> GenerateFinancialInvestigationsFields(Application application) =>
			new[]
			{
				new FormField("Are there any financial investigations ongoing at the school?", application.SchoolFinancialInvestigations.ToYesNoString())
			};
	}
}