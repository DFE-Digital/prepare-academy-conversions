using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FinanceSection : BaseFormSection
	{
		public FinanceSection(SchoolApplication application) : base("Finances")
		{
			SubSections = new[]
			{
				//new FormSubSection("Previous financial year", GeneratePreviousFinancialYearFields(application)),
				//new FormSubSection("Current financial year", GenerateCurrentFinancialYearFields(application)),
				//new FormSubSection("Next financial year", GenerateNextFinancialYearFields(application)),
				new FormSubSection("Previous financial year", GenerateFinancialYearFields("previous", application.PreviousFinancialYear)),
				new FormSubSection("Current financial year", GenerateFinancialYearFields("current", application.CurrentFinancialYear)),
				new FormSubSection("Next financial year", GenerateFinancialYearFields("next", application.NextFinancialYear)),
				new FormSubSection("Loans", GenerateLoansFields(application.ExistingLoans)),
				new FormSubSection("Financial leases", GenerateFinancialLeasesFields(application.ExistingLeases)),
				new FormSubSection("Financial investigations", GenerateFinancialInvestigationsFields(application))
			};
		}

		//private IEnumerable<FormField> GeneratePreviousFinancialYearFields(SchoolApplication application) =>
		//	new[]
		//	{
		//		new FormField("End of previous financial year", application.SchoolPFYEndDate.Value.ToUkDateString()),
		//		new FormField("Revenue carry forward at the end of the previous financial year (31 March)", application.SchoolPFYRevenue.ToString()),
		//		new FormField("Surplus or deficit?", application.SchoolPFYRevenueStatus.ToSurplusDeficitString())
		//	};

		//private IEnumerable<FormField> GenerateCurrentFinancialYearFields(Application application) =>
		//	new[]
		//	{
		//		new FormField("End of current financial year", application.SchoolCFYEndDate.Value.ToUkDateString()),
		//		new FormField("Forecasted revenue carry forward at the end of the current financial year (31 March)", application.SchoolCFYRevenue.ToString()),
		//		new FormField("Surplus or deficit?", application.SchoolCFYRevenueStatus.ToSurplusDeficitString()),
		//		new FormField("Forecasted capital carry forward at the end of the current financial year (31 March)", application.SchoolCFYCapitalForward.ToString()),
		//		new FormField("Surplus or deficit?", application.SchoolCFYCapitalForwardStatus.ToSurplusDeficitString())
		//	};

		private IEnumerable<FormField> GenerateFinancialYearFields(string name, FinancialYear applicationFinancialYear) =>
			new[]
			{
						new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate.ToUkDateString()),
						new FormField($"Forecasted revenue carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.RevenueCarryForward.ToString()),
						new FormField("Surplus or deficit?", applicationFinancialYear.RevenueStatus), //.ToSurplusDeficitString()),
						new FormField($"Forecasted capital carry forward at the end of the {name} financial year (31 March)", applicationFinancialYear.CapitalCarryForward.ToString()),
						new FormField("Surplus or deficit?", applicationFinancialYear.CapitalStatus) // .ToSurplusDeficitString())
			};

		//private IEnumerable<FormField> GenerateNextFinancialYearFields(Application application) =>
		//	new[]
		//	{
		//		new FormField("End of next financial year", application.SchoolNFYEndDate.Value.ToUkDateString()),
		//		new FormField("Forecasted revenue carry forward at the end of the next financial year (31 March)", application.SchoolNFYRevenue.ToString()),
		//		new FormField("Surplus or deficit?", application.SchoolNFYRevenueStatus.ToSurplusDeficitString()),
		//		new FormField("Forecasted capital carry forward at the end of the next financial year (31 March)", application.SchoolNFYCapitalForward.ToString()),
		//		new FormField("Surplus or deficit?", application.SchoolNFYCapitalForwardStatus.ToSurplusDeficitString())
		//	};

		private IEnumerable<FormField> GenerateLoansFields(List<Loan> applicationLoans)
		{
			bool loansExist = (applicationLoans == null) || (applicationLoans.Count == 0);
			var loansFields = new List<FormField> {
				new FormField("Are there any existing loans?", loansExist.ToYesNoString()) // CML
				};

			if (loansExist)
			{
				float totalAmount = 0.0F;
				string purposeCombined = "";
				string providersCombined = "";
				string interestRatesCombined = "";
				string totalRepayments = ""; // string rather than an amount?
				foreach (var loan in applicationLoans)
				{
					totalAmount += loan.SchoolLoanAmount;
					purposeCombined = purposeCombined + " " + loan.SchoolLoanPurpose; // CML better string concat needed
					providersCombined = providersCombined + " " + loan.SchoolLoanProvider; // CML better string concat needed
					interestRatesCombined = interestRatesCombined + "% " + loan.SchoolLoanInterestRate; // CML better string concat needed
					totalRepayments = totalRepayments + " " + loan.SchoolLoanSchedule; // CML better string concat needed - or it should be a float?
				}
				loansFields.Add(new FormField("Total amount", totalAmount.ToString())); // CML needs a ToMoneyString() extension method?
				loansFields.Add(new FormField("Purpose of the loan(s)", purposeCombined));
				loansFields.Add(new FormField("Loan provider", providersCombined));
				loansFields.Add(new FormField("Interest rate(s)", interestRatesCombined));
				loansFields.Add(new FormField("Schedule of repayment", totalRepayments));
			}
			return loansFields;
		}

		private IEnumerable<FormField> GenerateFinancialLeasesFields(List<Lease> applicationLeases)
		{
			var leasesExist = (applicationLeases == null) || (applicationLeases.Count == 0);
			var leaseFields = new List<FormField>
			{
				new FormField("Are there any existing leases?", leasesExist.ToYesNoString()) 
			};

			if (leasesExist)
			{
				string detailsOfTerms = "";
				float repaymentValue = 0.0F;
				string interestRatesCombined = "";
				float paymentsMadeToDate = 0.0F;
				string purposeCombined = "";
				float valueOfAssets = 0.0F;
				string responsibilityForAssets = "";
				foreach (var lease in applicationLeases)
				{
					detailsOfTerms = detailsOfTerms + " " + lease.SchoolLeaseTerms;
					repaymentValue += lease.SchoolLeaseRepaymentValue;
					interestRatesCombined = interestRatesCombined + " " + lease.SchoolLeaseInterestRate;
					paymentsMadeToDate += lease.SchoolLeasePaymentToDate;
					purposeCombined = purposeCombined + " " + lease.SchoolLeasePurpose;
					valueOfAssets += lease.SchoolLeaseValueOfAssets;
					responsibilityForAssets = responsibilityForAssets + " " + lease.SchoolLeaseResponsibilityForAssets;
				}
				leaseFields.Add(new FormField("Details of the term of the finance lease agreement", detailsOfTerms));
				leaseFields.Add(new FormField("Repayment value", repaymentValue.ToString())); // CML needs a ToMoneyString() extension method?
				leaseFields.Add(new FormField("Interest rate chargeable", interestRatesCombined));
				leaseFields.Add(new FormField("Value of payments made to date", paymentsMadeToDate.ToString()));
				leaseFields.Add(new FormField("What was the finance lease for?", purposeCombined));
				leaseFields.Add(new FormField("Value of the assests at the start of the finance lease agreement", valueOfAssets.ToString()));
				leaseFields.Add(new FormField("Who is responsible for the insurance, repair and maintenance of the assets covered?", responsibilityForAssets));
			}

			return leaseFields;
		}

		private IEnumerable<FormField> GenerateFinancialInvestigationsFields(SchoolApplication application) =>
			new[]
			{
				new FormField("Are there any financial investigations ongoing at the school?", application.FinanceOngoingInvestigations.ToYesNoString()),
				new FormField("Provide a brief summary of the investigation", application.SchoolFinancialInvestigationsExplain),
				new FormField("Is the trust you are joining aware of the investigation", application.SchoolFinancialInvestigationsTrustAware.ToYesNoString())
			};
	}
}