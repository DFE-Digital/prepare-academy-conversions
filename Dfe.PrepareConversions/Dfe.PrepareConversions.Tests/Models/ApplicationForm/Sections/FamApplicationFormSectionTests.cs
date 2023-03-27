using AutoFixture;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class FamApplicationFormSectionTests
{
   [Fact]
   public void Constructor_generates_expected_fields_correctly()
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

      formSection.Heading.Should().Be("Overview");
      formSection.Fields.Should().BeEquivalentTo(expectedFields);
      formSection.SubSections.Should().HaveCount(1);
   }
}
