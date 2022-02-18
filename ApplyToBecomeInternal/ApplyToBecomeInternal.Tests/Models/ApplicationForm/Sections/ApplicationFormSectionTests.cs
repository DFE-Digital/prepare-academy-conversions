using ApplyToBecome.Data.Models;
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
	public class ApplicationFormSectionTests
	{
		[Fact(Skip = "complete when missng fields are implemented")]
		public void Constructor_WithApplication_SetsFields()
		{
			var fixture = new Fixture();
			var application = fixture.Create<Application>();
			application.ChangesToLaGovernance = true;
			application.ChangesToLaGovernance = true;

			throw new NotImplementedException();
		}

		[Fact]
		public void Constructor_Doesnt_Set_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<Application>();
			application.ChangesToLaGovernance = false;
			application.ChangesToTrust = false;
			var formSection = new ApplicationFormSection(application);

			var expectedFields = new[]
			{
				new FormField("Application to join", $"{application.TrustName} with {application.SchoolApplication.SchoolName}"),
				new FormField("Application reference", application.ApplicationId),
				new FormField("Lead applicant", application.ApplicationLeadAuthorName),
			};

			var expectedSubSectionFields = new[] {
				new FormField("Trust name", application.TrustName),
				new LinkFormField("Upload evidence that the trust consents to the school joining", "MadeUpDocName", application.TrustConsentEvidenceDocumentLink),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", "No"),
				new FormField("Will there be any changes at a local level due to this school joining?", "No"),
			};

			formSection.Heading.Should().Be("School application form");
			formSection.Fields.Should().BeEquivalentTo(expectedFields);
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedSubSectionFields);
		}
	}
}