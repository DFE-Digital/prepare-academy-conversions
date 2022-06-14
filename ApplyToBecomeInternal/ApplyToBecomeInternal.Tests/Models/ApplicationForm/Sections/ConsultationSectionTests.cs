
using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class ConsultationSectionTests
	{
		[Fact]
		public void Constructor_Doesnt_Include_Conditional_Rows_Following_Yes_Answers()
		{
			var application = new ApplyingSchool
			{
				SchoolHasConsultedStakeholders = true
			};

			var formSection = new ConsultationSection(application);

			var expectedFields = new[]
			{
				new FormField("Has the governing body consulted the relevant stakeholders?", "Yes")
			};

			formSection.Heading.Should().Be("Consultation");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}

		[Fact]
		public void Constructor_Includes_Conditional_Rows_Following_No_Answers()
		{
			var application = new ApplyingSchool
			{
				SchoolHasConsultedStakeholders = false
			};

			application.SchoolHasConsultedStakeholders = false;
			var formSection = new ConsultationSection(application);

			var expectedFields = new[]
			{
				new FormField("Has the governing body consulted the relevant stakeholders?", "No"),
				new FormField("When does the governing body plan to consult?", application.SchoolPlanToConsultStakeholders)
			};

			formSection.Heading.Should().Be("Consultation");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}
