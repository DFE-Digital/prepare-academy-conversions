using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class DeclarationSectionTests
{
   [Fact]
   public void Constructor_WithApplication_SetsFields()
   {
      ApplyingSchool application = new() { DeclarationBodyAgree = true};

      DeclarationSection formSection = new(application);

      FormField[] expectedFields = {
         new("I agree with all of these statements, and believe that the facts stated in this application are true", "Yes")
      };

      formSection.Heading.Should().Be("Declaration");
      formSection.SubSections.First().Heading.Should().Be("Details");
      formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
   }
}
