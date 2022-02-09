using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using AutoFixture;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public static class ExtensionMethods
	{		
		public static void SetDeficitSurplusStatuses(this ApplyingSchool application, string status)
		{
			application.PreviousFinancialYear.CapitalStatus = status;
			application.PreviousFinancialYear.RevenueStatus = status;
			application.CurrentFinancialYear.CapitalStatus = status;
			application.CurrentFinancialYear.RevenueStatus = status;
			application.NextFinancialYear.CapitalStatus = status;
			application.NextFinancialYear.RevenueStatus = status;
		}
	}

	public class FinanceSectionTests
	{
		private IEnumerable<FormField> ExpectedPreviousFinancialYearFields(FinancialYear financialYear)
		{
			decimal expectedCapital = financialYear.CapitalStatus == "Deficit" ? -1 * financialYear.CapitalCarryForward : financialYear.CapitalCarryForward;
			decimal expectedRevenue = financialYear.RevenueStatus == "Deficit" ? -1 * financialYear.RevenueCarryForward : financialYear.RevenueCarryForward;

			return new[]
			{
				new FormField("End of previous financial year", financialYear.FYEndDate.ToUkDateString()),
				new FormField("Revenue carry forward at the end of the previous financial year (31 March)", expectedRevenue.ToMoneyString(true)),
				new FormField("Surplus or deficit?", financialYear.RevenueStatus),
				new FormField("Capital carry forward at the end of the previous financial year (31 March)", expectedCapital.ToMoneyString(true)),
				new FormField("Surplus or deficit?", financialYear.CapitalStatus)
			};
		}

		private IEnumerable<FormField> ExpectedCurrentFinancialYearFields(FinancialYear financialYear)
		{
			decimal expectedCapital = financialYear.CapitalStatus == "Deficit" ? -1 * financialYear.CapitalCarryForward : financialYear.CapitalCarryForward;
			decimal expectedRevenue = financialYear.RevenueStatus == "Deficit" ? -1 * financialYear.RevenueCarryForward : financialYear.RevenueCarryForward;

			return new[]
			{
				new FormField("End of current financial year", financialYear.FYEndDate.ToUkDateString()),
				new FormField("Forecasted revenue carry forward at the end of the current financial year (31 March)", expectedRevenue.ToMoneyString(true)),
				new FormField("Surplus or deficit?", financialYear.RevenueStatus),
				new FormField("Forecasted capital carry forward at the end of the current financial year (31 March)", expectedCapital.ToMoneyString(true)),
				new FormField("Surplus or deficit?", financialYear.CapitalStatus)
			};
		}

		private IEnumerable<FormField> ExpectedNextFinancialYearFields(FinancialYear financialYear)
		{
			decimal expectedCapital = financialYear.CapitalStatus == "Deficit" ? -1 * financialYear.CapitalCarryForward : financialYear.CapitalCarryForward;
			decimal expectedRevenue = financialYear.RevenueStatus == "Deficit" ? -1 * financialYear.RevenueCarryForward : financialYear.RevenueCarryForward;
			return new[]
			{
				new FormField("End of next financial year", financialYear.FYEndDate.ToUkDateString()),
				new FormField("Forecasted revenue carry forward at the end of the next financial year (31 March)", expectedRevenue.ToMoneyString(true)),
				new FormField("Surplus or deficit?", financialYear.RevenueStatus),
				new FormField("Forecasted capital carry forward at the end of the next financial year (31 March)", expectedCapital.ToMoneyString(true)),
				new FormField("Surplus or deficit?", financialYear.CapitalStatus)
			};
		}

		[Fact(Skip = "complete when missng fields are implemented")]
		public void Constructor_Sets_Conditional_Rows_Following_Yes_Answers()
		{
			throw new NotImplementedException();
		}

		[Fact(Skip = "complete when missng fields are implemented")]
		public void Constructor_Sets_Conditional_Rows_Following_Deficit_Answers()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void Constructor_sets_negative_values_for_deficit_values()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SetDeficitSurplusStatuses("Deficit");
			application.ExistingLoans = null;
			application.ExistingLeases = null;
			application.FinanceOngoingInvestigations = false;
			application.PreviousFinancialYear.CapitalCarryForward = 1000;
			application.PreviousFinancialYear.RevenueCarryForward = 10000;
			application.CurrentFinancialYear.CapitalCarryForward = 2000;
			application.CurrentFinancialYear.RevenueCarryForward = 20000;
			application.NextFinancialYear.CapitalCarryForward = 3000;
			application.NextFinancialYear.RevenueCarryForward = 30000;

			var formSection = new FinanceSection(application);

			var expectedPreviousFinancialYearFields = ExpectedPreviousFinancialYearFields(application.PreviousFinancialYear);
			var expectedCurrentFinancialYearFields = ExpectedCurrentFinancialYearFields(application.CurrentFinancialYear);
			var expectedNextFinancialYearFields = ExpectedNextFinancialYearFields(application.NextFinancialYear);

			formSection.Heading.Should().Be("Finances");
			formSection.SubSections.Should().HaveCount(6);
			var subSections = formSection.SubSections.ToArray();
			subSections[0].Heading.Should().Be("Previous financial year");
			subSections[0].Fields.Should().BeEquivalentTo(expectedPreviousFinancialYearFields);
			subSections[1].Heading.Should().Be("Current financial year");
			subSections[1].Fields.Should().BeEquivalentTo(expectedCurrentFinancialYearFields);
			subSections[2].Heading.Should().Be("Next financial year");
			subSections[2].Fields.Should().BeEquivalentTo(expectedNextFinancialYearFields);

		}

		[Fact]
		public void Constructor_Doesnt_Set_Conditional_Rows_Following_No_Answers_And_Surplus_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SetDeficitSurplusStatuses("Surplus");
			application.ExistingLoans = null;
			application.ExistingLeases = null;
			application.FinanceOngoingInvestigations = false;

			var formSection = new FinanceSection(application);

			var expectedPreviousFinancialYearFields = ExpectedPreviousFinancialYearFields(application.PreviousFinancialYear);
			var expectedCurrentFinancialYearFields = ExpectedCurrentFinancialYearFields(application.CurrentFinancialYear);
			var expectedNextFinancialYearFields = ExpectedNextFinancialYearFields(application.NextFinancialYear);
			var expectedLoansFields = new[]
			{
				new FormField("Are there any existing loans?", "No")
			};
			var expectedFinancialLeasesFields = new[]
			{
				new FormField("Are there any existing leases?", "No")
			};
			var expectedFinancialInvestigationsFields = new[]
			{
				new FormField("Are there any financial investigations ongoing at the school?", "No")
			};

			formSection.Heading.Should().Be("Finances");
			formSection.SubSections.Should().HaveCount(6);
			var subSections = formSection.SubSections.ToArray();
			subSections[0].Heading.Should().Be("Previous financial year");
			subSections[0].Fields.Should().BeEquivalentTo(expectedPreviousFinancialYearFields);
			subSections[1].Heading.Should().Be("Current financial year");
			subSections[1].Fields.Should().BeEquivalentTo(expectedCurrentFinancialYearFields);
			subSections[2].Heading.Should().Be("Next financial year");
			subSections[2].Fields.Should().BeEquivalentTo(expectedNextFinancialYearFields);
			subSections[3].Heading.Should().Be("Loans");
			subSections[3].Fields.Should().BeEquivalentTo(expectedLoansFields);
			subSections[4].Heading.Should().Be("Financial leases");
			subSections[4].Fields.Should().BeEquivalentTo(expectedFinancialLeasesFields);
			subSections[5].Heading.Should().Be("Financial investigations");
			subSections[5].Fields.Should().BeEquivalentTo(expectedFinancialInvestigationsFields);
		}
	}
}