using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class FuturePupilNumberSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application
			{
				FuturePupilNumbers = new FuturePupilNumbers
				{
					Year1 = 189,
					Year2 = 189,
					Year3 = 189,
					ProjectionReasoning = "",
					PublishedAdmissionsNumber = 210
				}
			};
			var formSection = new FuturePupilNumberSection(application);

			var expectedFormFields = new[] {
				new FormField("Projected pupil numbers on roll in the year the academy opens (year 1)", "189"),
				new FormField("Projected pupil numbers on roll in the following year after the academy has opened (year 2)", "189"),
				new FormField("Projected pupil numbers on roll in the following year (year 3) ", "189"),
				new FormField("What do you base these projected numbers on? ", ""),
				new FormField("What is the school's published admissions number (PAN)?", "210")
			};
			formSection.Heading.Should().Be("Future pupil numbers");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFormFields);
		}
	}
}
