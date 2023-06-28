using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using Dfe.PrepareConversions.Models.ApplicationForm;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Dfe.PrepareConversions.Data.Models.AcademisationApplication;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections
{
   public class KeyPeopleSectionTests
   {
      [Fact]
      public void Constructor_generates_expected_fields_correctly()
      {
         Fixture fixture = new();
         List<TrustKeyPerson> keyPeople = new()
         {
            new(1, "John Smith", DateTime.Today, "John is the smithiest", new List<TrustKeyPersonRole>{ new(1, "Member", "1 month"), new(1, "Chair", "1 year")}),
            new(1, "Jane Doe", DateTime.Today, "Jane is the doe", new List<TrustKeyPersonRole>{ new(1, "Member", "1 month")})
         };
         Application application = fixture.Create<Application>();
         application.KeyPeople = keyPeople;

         KeyPeopleSection formSection = new(application);

         FormField[] expectedFirstKeyPerson = {
            new("Position(s) within the trust", "Member, Chair"),
            new("Date of birth", $"{application.KeyPeople.ElementAt(0).DateOfBirth.ToDateString()}"),
            new("Biography", $"{application.KeyPeople.ElementAt(0).Biography}")
         };
         FormField[] expectedSecondKeyPerson = {
            new("Position(s) within the trust", "Member"),
            new("Date of birth", $"{application.KeyPeople.ElementAt(1).DateOfBirth.ToDateString()}"),
            new("Biography", $"{application.KeyPeople.ElementAt(1).Biography}")
         };

         formSection.Heading.Should().Be("Key people within the trust");
         formSection.SubSections.Should().HaveCount(2);
         FormSubSection[] subSections = formSection.SubSections.ToArray();
         subSections[0].Heading.Should().Be(application.KeyPeople.ElementAt(0).Name);
         subSections[0].Fields.Should().BeEquivalentTo(expectedFirstKeyPerson);
         subSections[1].Heading.Should().Be(application.KeyPeople.ElementAt(1).Name);
         subSections[1].Fields.Should().BeEquivalentTo(expectedSecondKeyPerson);
      }
   }
}