using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class PreOpeningSupportGrantSectionTests
{
   [Fact]
   public static void Constructor_WithApplication_SetFields()
   {
      ApplyingSchool application = new() { SchoolSupportGrantFundsPaidTo = "School" };

      PreOpeningSupportGrantSection formSection = new(application);

      FormField[] expectedFields = { new("Do you want these funds paid to the school or the trust?", application.SchoolSupportGrantFundsPaidTo) };

      formSection.Heading.Should().Be("Conversion support grant");
      formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
   }
}
