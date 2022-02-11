using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class DeclarationSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new ApplyingSchool
			{
				SchoolApplicantDeclarationIsApplicationCorrect = true,
				SchoolDeclarationSignedByName = "Garth Brown"
			};

			var formSection = new DeclarationSection(application);

			var expectedFields = new[]
			{
				new FormField("I agree with all of these statements, and believe that the facts stated in this application are true", "Yes"),
				new FormField("Signed by", "Garth Brown")
			};

			formSection.Heading.Should().Be("Declaration");
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}