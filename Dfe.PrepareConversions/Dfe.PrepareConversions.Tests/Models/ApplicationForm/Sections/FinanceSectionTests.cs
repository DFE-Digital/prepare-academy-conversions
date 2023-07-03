using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public static class ExtensionMethods
{
   public static void SetDeficitStatus(this ApplyingSchool application, bool deficit)
   {
      application.PreviousFinancialYear.CapitalIsDeficit = deficit;
      application.PreviousFinancialYear.RevenueIsDeficit = deficit;
      application.CurrentFinancialYear.CapitalIsDeficit = deficit;
      application.CurrentFinancialYear.RevenueIsDeficit = deficit;
      application.NextFinancialYear.CapitalIsDeficit = deficit;
      application.NextFinancialYear.RevenueIsDeficit = deficit;
   }
}

public class FinanceSectionTests
{
   [Fact]
   public void Constructor_Includes_Loans_And_Leases_When_Present()
   {
      Fixture fixture = new();
      Lease lease = fixture.Create<Lease>();
      Loan loan = fixture.Create<Loan>();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SchoolLoans = new List<Loan> { loan };
      application.SchoolLeases = new List<Lease> { lease };

      FinanceSection formSection = new(application);

      FormField[] expectedLoansFields = {
         new("Total amount", loan.SchoolLoanAmount.ToMoneyString(true)),
         new("Purpose of the loan", loan.SchoolLoanPurpose),
         new("Loan provider", loan.SchoolLoanProvider),
         new("Interest rate", loan.SchoolLoanInterestRate),
         new("Schedule of repayment", loan.SchoolLoanSchedule)
      };

      FormField[] expectedFinancialLeasesFields = {
         new("Details of the term of the finance lease agreement", lease.SchoolLeaseTerm),
         new("Repayment value", lease.SchoolLeaseRepaymentValue.ToMoneyString(true)),
         new("Interest rate chargeable", lease.SchoolLeaseInterestRate.ToPercentage()),
         new("Value of payments made to date", lease.SchoolLeasePaymentToDate.ToMoneyString(true)),
         new("What was the finance lease for?", lease.SchoolLeasePurpose),
         new("Value of the assests at the start of the finance lease agreement", lease.SchoolLeaseValueOfAssets),
         new("Who is responsible for the insurance, repair and maintenance of the assets covered?", lease.SchoolLeaseResponsibleForAssets)
      };

      formSection.Heading.Should().Be("Finances");
      formSection.SubSections.Should().HaveCount(6);
      FormSubSection[] subSections = formSection.SubSections.ToArray();
      subSections[0].Heading.Should().Be("Previous financial year");
      subSections[1].Heading.Should().Be("Current financial year");
      subSections[2].Heading.Should().Be("Next financial year");
      subSections[3].Heading.Should().Be("Financial investigations");
      subSections[4].Heading.Should().Be("Loan 1");
      subSections[4].Fields.Should().BeEquivalentTo(expectedLoansFields);
      subSections[5].Heading.Should().Be("Lease 1");
      subSections[5].Fields.Should().BeEquivalentTo(expectedFinancialLeasesFields);
   }

   [Fact]
   public void Constructor_Does_not_Include_Conditional_Rows_Following_No_Answers_And_Surplus_Answers()
   {
      Fixture fixture = new();
      
      
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SetDeficitStatus(false);
      application.SchoolLoans = null;
      application.SchoolLeases = null;
      application.FinanceOngoingInvestigations = false;

      FinanceSection formSection = new(application);

      FormField[] expectedPreviousFinancialYearFields = {
         new("End of previous financial year", application.PreviousFinancialYear.FYEndDate?.ToDateString()),
         new("Revenue carry forward at the end of the previous financial year (31 March)", application.PreviousFinancialYear.RevenueCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.PreviousFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
         new("Capital carry forward at the end of the previous financial year (31 March)", application.PreviousFinancialYear.CapitalCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.PreviousFinancialYear.CapitalIsDeficit.ToSurplusDeficitString())
      };
      FormField[] expectedCurrentFinancialYearFields = {
         new("End of current financial year", application.CurrentFinancialYear.FYEndDate?.ToDateString()),
         new("Forecasted revenue carry forward at the end of the current financial year (31 March)",
            application.CurrentFinancialYear.RevenueCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.CurrentFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
         new("Forecasted capital carry forward at the end of the current financial year (31 March)",
            application.CurrentFinancialYear.CapitalCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.CurrentFinancialYear.CapitalIsDeficit.ToSurplusDeficitString())
      };
      FormField[] expectedNextFinancialYearFields = {
         new("End of next financial year", application.NextFinancialYear.FYEndDate?.ToDateString()),
         new("Forecasted revenue carry forward at the end of the next financial year (31 March)",
            application.NextFinancialYear.RevenueCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.NextFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
         new("Forecasted capital carry forward at the end of the next financial year (31 March)",
            application.NextFinancialYear.CapitalCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.NextFinancialYear.CapitalIsDeficit.ToSurplusDeficitString())
      };
      FormField[] expectedLoansFields = { new("Are there any existing loans?", "No") };
      FormField[] expectedFinancialLeasesFields = { new("Are there any existing leases?", "No") };
      FormField[] expectedFinancialInvestigationsFields = { new("Are there any financial investigations ongoing at the school?", "No") };

      formSection.Heading.Should().Be("Finances");
      formSection.SubSections.Should().HaveCount(6);
      FormSubSection[] subSections = formSection.SubSections.ToArray();
      subSections[0].Heading.Should().Be("Previous financial year");
      subSections[0].Fields.Should().BeEquivalentTo(expectedPreviousFinancialYearFields);
      subSections[1].Heading.Should().Be("Current financial year");
      subSections[1].Fields.Should().BeEquivalentTo(expectedCurrentFinancialYearFields);
      subSections[2].Heading.Should().Be("Next financial year");
      subSections[2].Fields.Should().BeEquivalentTo(expectedNextFinancialYearFields);
      subSections[3].Heading.Should().Be("Financial investigations");
      subSections[3].Fields.Should().BeEquivalentTo(expectedFinancialInvestigationsFields);
      subSections[4].Heading.Should().Be("Loans");
      subSections[4].Fields.Should().BeEquivalentTo(expectedLoansFields);
      subSections[5].Heading.Should().Be("Leases");
      subSections[5].Fields.Should().BeEquivalentTo(expectedFinancialLeasesFields);
   }

   [Fact]
   public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers_And_Deficit_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SetDeficitStatus(true);
      application.SchoolLoans = null;
      application.SchoolLeases = null;
      application.FinanceOngoingInvestigations = true;

      FinanceSection formSection = new(application);

      FormField[] expectedPreviousFinancialYearFields = {
         new("End of previous financial year", application.PreviousFinancialYear.FYEndDate?.ToDateString()),
         new("Revenue carry forward at the end of the previous financial year (31 March)", application.PreviousFinancialYear.RevenueCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.PreviousFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
         new("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            application.PreviousFinancialYear.RevenueStatusExplained),
         new("Capital carry forward at the end of the previous financial year (31 March)", application.PreviousFinancialYear.CapitalCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.PreviousFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()),
         new("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            application.PreviousFinancialYear.CapitalStatusExplained)
      };
      FormField[] expectedCurrentFinancialYearFields = {
         new("End of current financial year", application.CurrentFinancialYear.FYEndDate?.ToDateString()),
         new("Forecasted revenue carry forward at the end of the current financial year (31 March)",
            application.CurrentFinancialYear.RevenueCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.CurrentFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
         new("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            application.CurrentFinancialYear.RevenueStatusExplained),
         new("Forecasted capital carry forward at the end of the current financial year (31 March)",
            application.CurrentFinancialYear.CapitalCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.CurrentFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()),
         new("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan",
            application.CurrentFinancialYear.CapitalStatusExplained)
      };
      FormField[] expectedNextFinancialYearFields = {
         new("End of next financial year", application.NextFinancialYear.FYEndDate?.ToDateString()),
         new("Forecasted revenue carry forward at the end of the next financial year (31 March)",
            application.NextFinancialYear.RevenueCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.NextFinancialYear.RevenueIsDeficit.ToSurplusDeficitString()),
         new("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan", application.NextFinancialYear.RevenueStatusExplained),
         new("Forecasted capital carry forward at the end of the next financial year (31 March)",
            application.NextFinancialYear.CapitalCarryForward.ToMoneyString(true)),
         new("Surplus or deficit?", application.NextFinancialYear.CapitalIsDeficit.ToSurplusDeficitString()),
         new("Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan", application.NextFinancialYear.CapitalStatusExplained)
      };
      FormField[] expectedLoansFields = { new("Are there any existing loans?", "No") };
      FormField[] expectedFinancialLeasesFields = { new("Are there any existing leases?", "No") };
      FormField[] expectedFinancialInvestigationsFields = {
         new("Are there any financial investigations ongoing at the school?", "Yes"),
         new("Provide a brief summary of the investigation", application.SchoolFinancialInvestigationsExplain),
         new("Is the trust you are joining aware of the investigation", application.SchoolFinancialInvestigationsTrustAware.ToYesNoString())
      };

      formSection.Heading.Should().Be("Finances");
      formSection.SubSections.Should().HaveCount(6);
      FormSubSection[] subSections = formSection.SubSections.ToArray();
      subSections[0].Heading.Should().Be("Previous financial year");
      subSections[0].Fields.Should().BeEquivalentTo(expectedPreviousFinancialYearFields);
      subSections[1].Heading.Should().Be("Current financial year");
      subSections[1].Fields.Should().BeEquivalentTo(expectedCurrentFinancialYearFields);
      subSections[2].Heading.Should().Be("Next financial year");
      subSections[2].Fields.Should().BeEquivalentTo(expectedNextFinancialYearFields);
      subSections[3].Heading.Should().Be("Financial investigations");
      subSections[3].Fields.Should().BeEquivalentTo(expectedFinancialInvestigationsFields);
      subSections[4].Heading.Should().Be("Loans");
      subSections[4].Fields.Should().BeEquivalentTo(expectedLoansFields);
      subSections[5].Heading.Should().Be("Leases");
      subSections[5].Fields.Should().BeEquivalentTo(expectedFinancialLeasesFields);
   }
}
