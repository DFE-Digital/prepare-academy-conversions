using ApplyToBecome.Data.Models.AcademisationApplication;
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
				new FormSubSection("Previous financial year", GeneratePreviousFinancialYearFields("previous", application.PreviousFinancialYear)),
				new FormSubSection("Current financial year", GenerateFinancialYearFields("current", application.CurrentFinancialYear)),
				new FormSubSection("Next financial year", GenerateFinancialYearFields("next", application.NextFinancialYear)),
				new FormSubSection("Loans", GenerateLoansFields(application.SchoolLoans)),
				new FormSubSection("Financial leases", GenerateFinancialLeasesFields(application.SchoolLeases)),
				new FormSubSection("Financial investigations", GenerateFinancialInvestigationsFields(application))
			};
		}

		private static IEnumerable<FormField> GeneratePreviousFinancialYearFields(string name, FinancialYear applicationFinancialYear)
		{

			var previousFinancialYearFields = new List<FormField>();
			previousFinancialYearFields.Add(new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate?.ToDateString()));
			previousFinancialYearFields.Add(new FormField($"Revenue carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.RevenueCarryForward.ToMoneyString(true)));
			previousFinancialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()));
			if (applicationFinancialYear.RevenueIsDeficit == true)
			{
				previousFinancialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan", applicationFinancialYear.RevenueStatusExplained));
			}
			previousFinancialYearFields.Add(new FormField($"Capital carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.CapitalCarryForward.ToMoneyString(true)));
			previousFinancialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()));
			if (applicationFinancialYear.CapitalIsDeficit == true)
			{
				previousFinancialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan", applicationFinancialYear.CapitalStatusExplained));
			}

			return previousFinancialYearFields;
		}

		private static IEnumerable<FormField> GenerateFinancialYearFields(string name, FinancialYear applicationFinancialYear)
		{
			var financialYearFields = new List<FormField>();

			financialYearFields.Add(new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate?.ToDateString()));
			financialYearFields.Add(new FormField($"Forecasted revenue carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.RevenueCarryForward.ToMoneyString(true)));
			financialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()));
			if (applicationFinancialYear.RevenueIsDeficit == true)
			{
				financialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan", applicationFinancialYear.RevenueStatusExplained));
			}
			financialYearFields.Add(new FormField($"Forecasted capital carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.CapitalCarryForward.ToMoneyString(true)));
			financialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()));
			if (applicationFinancialYear.CapitalIsDeficit == true)
			{
				financialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan", applicationFinancialYear.CapitalStatusExplained));
			}

			return financialYearFields;
		}

		private static IEnumerable<FormField> GenerateLoansFields(ICollection<Loan> applicationLoans)
		{
			bool loansExist = applicationLoans?.Count > 0;
			var loansFields = new List<FormField> {
				new FormField("Are there any existing loans?", loansExist.ToYesNoString())
				};

			if (loansExist)
			{
				foreach (var loan in applicationLoans)
				{
					loansFields.Add(new FormField("Total amount", loan.SchoolLoanAmount.ToMoneyString(true)));
					loansFields.Add(new FormField("Purpose of the loan", loan.SchoolLoanPurpose));
					loansFields.Add(new FormField("Loan provider", loan.SchoolLoanProvider));
					loansFields.Add(new FormField("Interest rate", loan.SchoolLoanInterestRate));
					loansFields.Add(new FormField("Schedule of repayment", loan.SchoolLoanSchedule));
				}
			}
			return loansFields;
		}

		private static IEnumerable<FormField> GenerateFinancialLeasesFields(ICollection<Lease> applicationLeases)
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
					leaseFields.Add(new FormField("Details of the term of the finance lease agreement", lease.SchoolLeaseTerm));
					leaseFields.Add(new FormField("Repayment value", lease.SchoolLeaseRepaymentValue.ToMoneyString(true)));
					leaseFields.Add(new FormField("Interest rate chargeable", $"{lease.SchoolLeaseInterestRate}%"));
					leaseFields.Add(new FormField("Value of payments made to date", lease.SchoolLeasePaymentToDate.ToMoneyString(true)));
					leaseFields.Add(new FormField("What was the finance lease for?", lease.SchoolLeasePurpose));
					leaseFields.Add(new FormField("Value of the assests at the start of the finance lease agreement", lease.SchoolLeaseValueOfAssets));
					leaseFields.Add(new FormField("Who is responsible for the insurance, repair and maintenance of the assets covered?", lease.SchoolLeaseResponsibleForAssets));
				}
			}
			return leaseFields;
		}

		private static IEnumerable<FormField> GenerateFinancialInvestigationsFields(ApplyingSchool application)
		{
			var financialInvestigationsFields = new List<FormField>
			{
				new FormField("Are there any financial investigations ongoing at the school?", application.FinanceOngoingInvestigations?.ToYesNoString()),
			};
			if (application.FinanceOngoingInvestigations == true)
			{
				financialInvestigationsFields.Add(new FormField("Provide a brief summary of the investigation", application.SchoolFinancialInvestigationsExplain));
				financialInvestigationsFields.Add(new FormField("Is the trust you are joining aware of the investigation", application.SchoolFinancialInvestigationsTrustAware.ToYesNoString()));
			}
			return financialInvestigationsFields;
		}

	}
}