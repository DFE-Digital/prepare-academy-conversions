using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FinanceSection : BaseFormSection
	{
		public FinanceSection(ApplyingSchool application) : base("Finances")
		{
			SubSections = new[]
			{
				new FormSubSection("Previous financial year", GenerateFinancialYearFields("previous", application.PreviousFinancialYear)),
				new FormSubSection("Current financial year", GenerateFinancialYearFields("current", application.CurrentFinancialYear)),
				new FormSubSection("Next financial year", GenerateFinancialYearFields("next", application.NextFinancialYear)),
				new FormSubSection("Loans", GenerateLoansFields(application.ExistingLoans)),
				new FormSubSection("Financial leases", GenerateFinancialLeasesFields(application.ExistingLeases)),
				new FormSubSection("Financial investigations", GenerateFinancialInvestigationsFields(application))
			};
		}

		private IEnumerable<FormField> GenerateFinancialYearFields(string name, FinancialYear applicationFinancialYear) =>
			new[]
			{
						new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate.ToUkDateString()),
						new FormField($"Forecasted revenue carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.RevenueCarryForward.ToString()),
						new FormField("Surplus or deficit?", applicationFinancialYear.RevenueStatus), //.ToSurplusDeficitString()),
						new FormField($"Forecasted capital carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.CapitalCarryForward.ToString()),
						new FormField("Surplus or deficit?", applicationFinancialYear.CapitalStatus) // .ToSurplusDeficitString())
			};

		private IEnumerable<FormField> GenerateLoansFields(List<Loan> applicationLoans)
		{
			bool loansExist = applicationLoans?.Count > 0;
			var loansFields = new List<FormField> {
				new FormField("Are there any existing loans?", loansExist.ToYesNoString()) // CML
				};

			if (loansExist)
			{
				foreach (var loan in applicationLoans)
				{
					loansFields.Add(new FormField("Total amount", loan.SchoolLoanAmount.ToString())); // CML needs a ToMoneyString() extension method?
					loansFields.Add(new FormField("Purpose of the loan(s)", loan.SchoolLoanPurpose));
					loansFields.Add(new FormField("Loan provider", loan.SchoolLoanProvider));
					loansFields.Add(new FormField("Interest rate(s)", $"{loan.SchoolLoanInterestRate}%"));
					loansFields.Add(new FormField("Schedule of repayment", loan.SchoolLoanSchedule));
				}
			}
			return loansFields;
		}

		private IEnumerable<FormField> GenerateFinancialLeasesFields(List<Lease> applicationLeases)
		{
			bool leasesExist = applicationLeases?.Count > 0;
			var leaseFields = new List<FormField>
			{
				new FormField("Are there any existing leases?", leasesExist.ToYesNoString()) 
			};

			if (leasesExist)
			{
				foreach (var lease in applicationLeases)
				{
					leaseFields.Add(new FormField("Details of the term of the finance lease agreement", lease.SchoolLeaseTerms));
					leaseFields.Add(new FormField("Repayment value", lease.SchoolLeaseRepaymentValue.ToString())); // CML needs a ToMoneyString() extension method?
					leaseFields.Add(new FormField("Interest rate chargeable", $"{lease.SchoolLeaseInterestRate}%"));
					leaseFields.Add(new FormField("Value of payments made to date", lease.SchoolLeasePaymentToDate.ToString())); // CML needs a ToMoneyString() extension method?
					leaseFields.Add(new FormField("What was the finance lease for?", lease.SchoolLeasePurpose));
					leaseFields.Add(new FormField("Value of the assests at the start of the finance lease agreement", lease.SchoolLeaseValueOfAssets.ToString())); // CML needs a ToMoneyString() extension method?
					leaseFields.Add(new FormField("Who is responsible for the insurance, repair and maintenance of the assets covered?", lease.SchoolLeaseResponsibilityForAssets));
				}
			}
			return leaseFields;
		}

		private IEnumerable<FormField> GenerateFinancialInvestigationsFields(ApplyingSchool application) =>
			new[]
			{
				new FormField("Are there any financial investigations ongoing at the school?", application.FinanceOngoingInvestigations.ToYesNoString()),
				new FormField("Provide a brief summary of the investigation", application.SchoolFinancialInvestigationsExplain),
				new FormField("Is the trust you are joining aware of the investigation", application.SchoolFinancialInvestigationsTrustAware.ToYesNoString())
			};
	}
}