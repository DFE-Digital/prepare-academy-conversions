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
		[Fact]
		public void Constructor_Doesnt_Include_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<Application>();
			application.ChangesToLaGovernance = false;
			application.ChangesToTrust = false;
			var formSection = new ApplicationFormSection(application);

			var expectedFields = new[]
			{
				new FormField("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
				new FormField("Application reference", application.ApplicationId),
				new FormField("Lead applicant", application.ApplicationLeadAuthorName),
			};

			var expectedSubSectionFields = new[] {
				new FormField("Trust name", application.TrustName),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", "No"),
				new FormField("Will there be any changes at a local level due to this school joining?", "No"),
			};

			formSection.Heading.Should().Be("Overview");
			formSection.Fields.Should().BeEquivalentTo(expectedFields);
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedSubSectionFields);
		}

		[Fact]
		public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<Application>();
			application.ChangesToLaGovernance = true;
			application.ChangesToTrust = true;
			var formSection = new ApplicationFormSection(application);

			var expectedFields = new[]
			{
				new FormField("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
				new FormField("Application reference", application.ApplicationId),
				new FormField("Lead applicant", application.ApplicationLeadAuthorName),
			};

			var expectedSubSectionFields = new[] {
				new FormField("Trust name", application.TrustName),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", "Yes"),
				new FormField("What are the changes?", application.ChangesToTrustExplained),
				new FormField("Will there be any changes at a local level due to this school joining?", "Yes"),
				new FormField("What are the changes and how do they fit into the current lines of accountability in the trust?", application.ChangesToLaGovernanceExplained)
			};

			formSection.Heading.Should().Be("Overview");
			formSection.Fields.Should().BeEquivalentTo(expectedFields);
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedSubSectionFields);
		}

		[Fact]
		public void Constructor_Shows_Fields_As_Unknown_When_Value_Is_Null()
		{
			var fixture = new Fixture();
			var application = fixture.Create<Application>();
			application.ChangesToTrust = null;
			application.ChangesToTrustExplained = null;
			application.ChangesToLaGovernance = null;
			application.ChangesToLaGovernanceExplained = null;
			var formSection = new ApplicationFormSection(application);

			formSection.SubSections.First().Fields
				.Where(s => s.Title == "Will there be any changes to the governance of the trust due to the school joining?")
				.FirstOrDefault()
				.Content
				.Should().Be("Unknown");

			formSection.SubSections.First().Fields
				.Where(s => s.Title == "Will there be any changes at a local level due to this school joining?")
				.FirstOrDefault()
				.Content
				.Should().Be("Unknown");
		}
	}
}