using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using AutoFixture;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class FinanceSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();

			var formSection = new FinanceSection(application);

			var expectedPreviousFinancialYearFields = new[]
			{
				new FormField("End of previous financial year", application.PreviousFinancialYear.FYEndDate.ToShortDateString()),
				new FormField("Revenue carry forward at the end of the previous financial year (31 March)", application.PreviousFinancialYear.RevenueCarryForward.ToString()), // CML to money string
				new FormField("Surplus or deficit?", "Surplus")
			};
			var expectedCurrentFinancialYearFields = new[]
			{
				new FormField("End of current financial year", "31/03/2021"),
				new FormField("Forecasted revenue carry forward at the end of the current financial year (31 March)", "£143,931.00"),
				new FormField("Surplus or deficit?", "Surplus"),
				new FormField("Forecasted capital carry forward at the end of the current financial year (31 March)", "£0.00"),
				new FormField("Surplus or deficit?", "Surplus")
			};
			var expectedNextFinancialYearFields = new[]
			{
				new FormField("End of next financial year", "31/03/2022"),
				new FormField("Forecasted revenue carry forward at the end of the next financial year (31 March)", "£169,093.00"),
				new FormField("Surplus or deficit?", "Surplus"),
				new FormField("Forecasted capital carry forward at the end of the next financial year (31 March)", "£0.00"),
				new FormField("Surplus or deficit?", "Surplus")
			};
			var expectedLoansFields = new[]
			{
				new FormField("Are there any existing loans?", "No")
			};
			var expectedFinancialLeasesFields = new[]
			{
				new FormField("Are there any existing leases?", "Yes")
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