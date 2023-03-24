using AutoFixture;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class ApplicationFormSectionTests
{
   [Fact]
   public void Constructor_Does_not_Include_Conditional_Rows_Following_No_Answers()
   {
      Fixture fixture = new();
      Application application = fixture.Create<Application>();
      application.ChangesToLaGovernance = false;
      application.ChangesToTrust = false;
      ApplicationFormSection formSection = new(application);

      FormField[] expectedFields = {
         new("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
         new("Application reference", application.ApplicationId),
         new("Lead applicant", application.ApplicationLeadAuthorName)
      };

      FormField[] expectedSubSectionFields = {
         new("Trust name", application.TrustName),
         new("Will there be any changes to the governance of the trust due to the school joining?", "No"),
         new("Will there be any changes at a local level due to this school joining?", "No")
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
      Fixture fixture = new();
      Application application = fixture.Create<Application>();
      application.ChangesToLaGovernance = true;
      application.ChangesToTrust = true;
      ApplicationFormSection formSection = new(application);

      FormField[] expectedFields = {
         new("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
         new("Application reference", application.ApplicationId),
         new("Lead applicant", application.ApplicationLeadAuthorName)
      };

      FormField[] expectedSubSectionFields = {
         new("Trust name", application.TrustName),
         new("Will there be any changes to the governance of the trust due to the school joining?", "Yes"),
         new("What are the changes?", application.ChangesToTrustExplained),
         new("Will there be any changes at a local level due to this school joining?", "Yes"),
         new("What are the changes and how do they fit into the current lines of accountability in the trust?", application.ChangesToLaGovernanceExplained)
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
      Fixture fixture = new();
      Application application = fixture.Create<Application>();
      application.ChangesToTrust = null;
      application.ChangesToTrustExplained = null;
      application.ChangesToLaGovernance = null;
      application.ChangesToLaGovernanceExplained = null;
      ApplicationFormSection formSection = new(application);

      formSection.SubSections.First().Fields
         .FirstOrDefault(s => s.Title == "Will there be any changes to the governance of the trust due to the school joining?")
         ?.Content
         .Should().Be("Unknown");

      formSection.SubSections.First().Fields
         .FirstOrDefault(s => s.Title == "Will there be any changes at a local level due to this school joining?")
         ?.Content
         .Should().Be("Unknown");
   }
}
