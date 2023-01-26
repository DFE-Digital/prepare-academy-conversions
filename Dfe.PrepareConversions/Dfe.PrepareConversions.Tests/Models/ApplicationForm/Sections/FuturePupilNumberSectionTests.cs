using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections
{
	public class FuturePupilNumberSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new ApplyingSchool
			{
				ProjectedPupilNumbersYear1 = 150,
				ProjectedPupilNumbersYear2 = 151,
				ProjectedPupilNumbersYear3 = 152,
				SchoolCapacityAssumptions = "spreadsheets",
				SchoolCapacityPublishedAdmissionsNumber = 210
			};
			var formSection = new FuturePupilNumberSection(application);

			var expectedFormFields = new[] {
				new FormField("Projected pupil numbers on roll in the year the academy opens (year 1)", "150"),
				new FormField("Projected pupil numbers on roll in the following year after the academy has opened (year 2)", "151"),
				new FormField("Projected pupil numbers on roll in the following year (year 3) ", "152"),
				new FormField("What do you base these projected numbers on? ", application.SchoolCapacityAssumptions),
				new FormField("What is the school's published admissions number (PAN)?", "210")
			};
			formSection.Heading.Should().Be("Future pupil numbers");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFormFields);
		}
	}
}
