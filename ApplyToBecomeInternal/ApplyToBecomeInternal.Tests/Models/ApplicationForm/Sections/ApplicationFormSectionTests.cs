using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class ApplicationFormSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application
			{
				School = new School {Name = "St Wilfrid's Primary School"},
				Trust = new Trust {Name = "Dynamics Trust",},
				LeadApplicant = "Garth Brown",
				Details = new ApplicationDetails
				{
					EvidenceDocument = new Link {Name = "consent_dynamics.docx", Url = "#"}, ChangesToGovernance = false, ChangesAtLocalLevel = true
				}
			};
			var formSection = new ApplicationFormSection(application);

			var expectedFields = new[]
			{
				new FormField("Application to join", "Dynamics Trust with St Wilfrid's Primary School"),
				new FormField("Lead applicant", "Garth Brown"),
			};

			var expectedSubSectionFields = new[] {
				new LinkFormField("Upload evidence that the trust consents to the school joining", "consent_dynamics.docx", "#"),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", "No"),
				new FormField("Will there be any changes at a local level due to this school joining?", "Yes"),
			};

			formSection.Heading.Should().Be("Application Form");
			formSection.Fields.Should().BeEquivalentTo(expectedFields);
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedSubSectionFields);
		}
	}
}