using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.OverviewSections
{
	public class FinanceSectionTab : BaseFormSection
	{
		public FinanceSectionTab(ApplyingSchool application) : base("Finances")
		{
			SubSections = new[]
			{
				new FormSubSection("Previous financial year", GeneratePreviousFinancialYearFields("previous", application.PreviousFinancialYear)),
				new FormSubSection("Current financial year", GenerateFinancialYearFields("current", application.CurrentFinancialYear)),
				new FormSubSection("Next financial year", GenerateFinancialYearFields("next", application.NextFinancialYear)),
				new FormSubSection("Loans", GenerateLoansFields(application.SchoolLoans)),
				new FormSubSection("Financial leases", GenerateFinancialLeasesFields(application.SchoolLeases)),
				new FormSubSection("Financial investigations", GenerateFinancialInvestigationsFields(application))
			};
		}

		private IEnumerable<FormField> GeneratePreviousFinancialYearFields(string name, FinancialYear applicationFinancialYear)
		{
			return new[]
			{
						new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate?.ToUkDateString()),
						new FormField($"Revenue carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.RevenueCarryForward.ToMoneyString(true)),
						new FormField("Surplus or deficit?", applicationFinancialYear.RevenueIsDeficit.ToSurplusDeficitString())
						
			};
		}

		private IEnumerable<FormField> GenerateFinancialYearFields(string name, FinancialYear applicationFinancialYear)
		{
			return new[]
			{
						new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate?.ToUkDateString()),
						new FormField($"Forecasted revenue carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.RevenueCarryForward.ToMoneyString(true)),
						new FormField("Surplus or deficit?", applicationFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
						new FormField($"Forecasted capital carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.CapitalCarryForward.ToMoneyString(true)),
						new FormField("Surplus or deficit?", applicationFinancialYear.CapitalIsDeficit.ToSurplusDeficitString())
			};
		}

		private IEnumerable<FormField> GenerateLoansFields(ICollection<Loan> applicationLoans)
		{
			bool loansExist = applicationLoans?.Count > 0;
			var loansFields = new List<FormField> {
				new FormField("Are there any existing loans?", loansExist.ToYesNoString())
				};

			return loansFields;
		}

		private IEnumerable<FormField> GenerateFinancialLeasesFields(ICollection<Lease> applicationLeases)
		{
			bool leasesExist = applicationLeases?.Count > 0;
			var leaseFields = new List<FormField>
			{
				new FormField("Are there any existing leases?", leasesExist.ToYesNoString())
			};
			return leaseFields;
		}

		private IEnumerable<FormField> GenerateFinancialInvestigationsFields(ApplyingSchool application)
		{
			var financialInvestigationsFields = new List<FormField>
			{
				new FormField("Are there any financial investigations ongoing at the school?", application.FinanceOngoingInvestigations?.ToYesNoString()),
			};
			
			return financialInvestigationsFields;
		}

	}
}