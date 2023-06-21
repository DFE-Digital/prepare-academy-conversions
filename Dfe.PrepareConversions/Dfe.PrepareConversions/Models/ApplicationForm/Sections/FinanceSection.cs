using AngleSharp.Common;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using DocumentFormat.OpenXml.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections;

public class FinanceSection : BaseFormSection
{
   public FinanceSection(ApplyingSchool application) : base("Finances")
   {
      SubSections = new[]
      {
         new FormSubSection("Previous financial year", GeneratePreviousFinancialYearFields("previous", application.PreviousFinancialYear)),
         new FormSubSection("Current financial year", GenerateFinancialYearFields("current", application.CurrentFinancialYear)),
         new FormSubSection("Next financial year", GenerateFinancialYearFields("next", application.NextFinancialYear)),
         new FormSubSection("Financial investigations", GenerateFinancialInvestigationsFields(application))
      }.Concat(GenerateLoansFields(application.SchoolLoans)).Concat(GenerateFinancialLeasesFields(application.SchoolLeases));
   }

   private static IEnumerable<FormField> GeneratePreviousFinancialYearFields(string name, FinancialYear applicationFinancialYear)
   {
      List<FormField> previousFinancialYearFields = new();
      previousFinancialYearFields.Add(new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate?.ToDateString()));
      previousFinancialYearFields.Add(new FormField($"Revenue carry forward at the end of the {name} financial year (31 March)",
         applicationFinancialYear.RevenueCarryForward.ToMoneyString(true)));
      previousFinancialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()));
      if (applicationFinancialYear.RevenueIsDeficit == true)
      {
         previousFinancialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            applicationFinancialYear.RevenueStatusExplained));
      }

      previousFinancialYearFields.Add(new FormField($"Capital carry forward at the end of the {name} financial year (31 March)",
         applicationFinancialYear.CapitalCarryForward.ToMoneyString(true)));
      previousFinancialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()));
      if (applicationFinancialYear.CapitalIsDeficit == true)
      {
         previousFinancialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            applicationFinancialYear.CapitalStatusExplained));
      }

      return previousFinancialYearFields;
   }

   private static IEnumerable<FormField> GenerateFinancialYearFields(string name, FinancialYear applicationFinancialYear)
   {
      List<FormField> financialYearFields = new();

      financialYearFields.Add(new FormField($"End of {name} financial year", applicationFinancialYear.FYEndDate?.ToDateString()));
      financialYearFields.Add(new FormField($"Forecasted revenue carry forward at the end of the {name} financial year (31 March)",
         applicationFinancialYear.RevenueCarryForward.ToMoneyString(true)));
      financialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()));
      if (applicationFinancialYear.RevenueIsDeficit == true)
      {
         financialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            applicationFinancialYear.RevenueStatusExplained));
      }

      financialYearFields.Add(new FormField($"Forecasted capital carry forward at the end of the {name} financial year (31 March)",
         applicationFinancialYear.CapitalCarryForward.ToMoneyString(true)));
      financialYearFields.Add(new FormField("Surplus or deficit?", applicationFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()));
      if (applicationFinancialYear.CapitalIsDeficit == true)
      {
         financialYearFields.Add(new FormField("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            applicationFinancialYear.CapitalStatusExplained));
      }

      return financialYearFields;
   }

   private static IEnumerable<FormSubSection> GenerateLoansFields(ICollection<Loan> applicationLoans)
   {
      bool loansExist = applicationLoans?.Count > 0;
      List<FormSubSection> loanSubSections = new();

      if (loansExist)
      {
         int loanCounter = 1;
         loanSubSections.AddRange(applicationLoans.Select(loan => new List<FormField>()
            {
               new("Total amount", loan.SchoolLoanAmount.ToMoneyString(true)),
               new("Purpose of the loan", loan.SchoolLoanPurpose),
               new("Loan provider", loan.SchoolLoanProvider),
               new("Interest rate", loan.SchoolLoanInterestRate),
               new("Schedule of repayment", loan.SchoolLoanSchedule)
            })
            .Select(loansFields => new FormSubSection($"Loan {loanCounter++}", loansFields)));
      }
      else
      {
         loanSubSections.Add(new FormSubSection("Loans", new List<FormField>() { new("Are there any existing loans?", "No") }));
      }

      return loanSubSections;
   }

   private static IEnumerable<FormSubSection> GenerateFinancialLeasesFields(ICollection<Lease> applicationLeases)
   {
      bool leasesExist = applicationLeases?.Count > 0;
      List<FormSubSection> leaseSubSections = new();

      if (leasesExist)
      {
         int leaseCounter = 1;
         leaseSubSections.AddRange(applicationLeases.Select(lease => new List<FormField>()
            {
               new("Details of the term of the finance lease agreement", lease.SchoolLeaseTerm),
               new("Repayment value", lease.SchoolLeaseRepaymentValue.ToMoneyString(true)),
               new("Interest rate chargeable", $"{lease.SchoolLeaseInterestRate}%"),
               new("Value of payments made to date", lease.SchoolLeasePaymentToDate.ToMoneyString(true)),
               new("What was the finance lease for?", lease.SchoolLeasePurpose),
               new("Value of the assests at the start of the finance lease agreement", lease.SchoolLeaseValueOfAssets),
               new("Who is responsible for the insurance, repair and maintenance of the assets covered?", lease.SchoolLeaseResponsibleForAssets)
            })
            .Select(leaseFields => new FormSubSection($"Lease {leaseCounter++}", leaseFields)));
      }
      else
      {
         leaseSubSections.Add(new FormSubSection("Leases", new List<FormField>() { new("Are there any existing leases?", "No") }));
      }

      return leaseSubSections;
   }


   private static IEnumerable<FormField> GenerateFinancialInvestigationsFields(ApplyingSchool application)
   {
      List<FormField> financialInvestigationsFields = new()
      {
         new("Are there any financial investigations ongoing at the school?", application.FinanceOngoingInvestigations?.ToYesNoString())
      };
      if (application.FinanceOngoingInvestigations == true)
      {
         financialInvestigationsFields.Add(new FormField("Provide a brief summary of the investigation", application.SchoolFinancialInvestigationsExplain));
         financialInvestigationsFields.Add(new FormField("Is the trust you are joining aware of the investigation",
            application.SchoolFinancialInvestigationsTrustAware.ToYesNoString()));
      }

      return financialInvestigationsFields;
   }
}
