using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class ApplicationFormSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application();
			var formSection = new ApplicationFormSection(application);

			var expectation = new[]
			{
				new FormField("Application to join", "Dynamics Trust with St Wilfrid's Primary School"),
				new FormField("Lead applicant", "Garth Brown"),
			};
			formSection.Fields.Should().BeEquivalentTo(expectation);
		}
	}
}