using AutoFixture;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class LandAndBuildingsSectionTest
{
   private static void SetBooleanQuestionsToTrue(ApplyingSchool application, bool value)
   {
      application.SchoolBuildLandWorksPlanned = value;
      application.SchoolBuildLandSharedFacilities = value;
      application.SchoolBuildLandGrants = value;
      application.SchoolBuildLandPFIScheme = value;
      application.SchoolBuildLandPriorityBuildingProgramme = value;
      application.SchoolBuildLandFutureProgramme = value;
      application.SchoolBuildLandSharedFacilities = value;
   }

   [Fact]
   public void Constructor_Does_not_Include_Conditional_Rows_Following_No_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      SetBooleanQuestionsToTrue(application, false);

      FormField[] expectedFields = {
         new("As far as you're aware, who owns or holds the school's buildings and land?", application.SchoolBuildLandOwnerExplained),
         new("Are there any current planned building works?", "No"),
         new("Are there any shared facilities on site?", "No"),
         new("Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?", "No"),
         new("Is the school part of a Private Finance Intiative (PFI) scheme?", "No"),
         new("Is the school part of a Priority School Building Programme?", "No"),
         new("Is the school part of a Building Schools for the Future Programme?", "No")
      };
      LandAndBuildingsSection formSection = new(application);

      formSection.Heading.Should().Be("Land and buildings");
      formSection.SubSections.Should().HaveCount(1);
      formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
   }

   [Fact]
   public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      SetBooleanQuestionsToTrue(application, true);
      FormField[] expectedFields = {
         new("As far as you're aware, who owns or holds the school's buildings and land?", application.SchoolBuildLandOwnerExplained),
         new("Are there any current planned building works?", "Yes"),
         new("Provide details of the works, how they'll be funded and whether the funding will be affected by the conversion",
            application.SchoolBuildLandWorksPlannedExplained),
         new("When is the scheduled completion date?", application.SchoolBuildLandWorksPlannedCompletionDate?.ToDateString()),
         new("Are there any shared facilities on site?", "Yes"),
         new("List the facilities and the school's plan for them after converting", application.SchoolBuildLandSharedFacilitiesExplained),
         new("Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?", "Yes"),
         new("Which bodies awarded the grants and what facilities did they fund?", application.SchoolBuildLandGrantsExplained),
         new("Is the school part of a Private Finance Intiative (PFI) scheme?", "Yes"),
         new("What kind of PFI Scheme is your school part of?", application.SchoolBuildLandPFISchemeType),
         new("Is the school part of a Priority School Building Programme?", "Yes"),
         new("Is the school part of a Building Schools for the Future Programme?", "Yes")
      };
      LandAndBuildingsSection formSection = new(application);

      formSection.Heading.Should().Be("Land and buildings");
      formSection.SubSections.Should().HaveCount(1);
      formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
   }
}
