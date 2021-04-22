
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
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application
			{
				HasGovernmentConsultedStakeholders = true
			};

			var formSection = new ConsultationSection(application);

			var expectedFields = new[]
			{
				new FormField("Has the governing body consulted the relevant stakeholders?", "Yes")
			};

			formSection.Heading.Should().Be("Consultation");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}
