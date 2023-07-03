using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class FurtherInformationSectionTests
{
   [Fact]
   public void Constructor_Does_not_Include_Conditional_Rows_Following_No_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SchoolAdInspectedButReportNotPublished = false;
      application.SchoolOngoingSafeguardingInvestigations = false;
      application.SchoolPartOfLaReorganizationPlan = false;
      application.SchoolPartOfLaClosurePlan = false;
      application.SchoolFaithSchool = false;
      application.SchoolIsPartOfFederation = false;
      application.SchoolIsSupportedByFoundation = false;
      application.SchoolHasSACREException = false;
      application.SchoolAdEqualitiesImpactAssessmentCompleted = false;
      application.SchoolAdEqualitiesImpactAssessmentCompleted = false;
      application.SchoolAdditionalInformationAdded = false;

      FormField[] expectedFields = {
         new("What will the school bring to the trust they are joining?", application.SchoolAdSchoolContributionToTrust),
         new("Have Ofsted inspected the school but not published the report yet?", "No"),
         new("Are there any safeguarding investigations ongoing at the school?", "No"),
         new("Is the school part of a local authority reorganisation?", "No"),
         new("Is the school part of any local authority closure plans?", "No"),
         new("Is your school linked to a diocese?", "No"),
         new("Is your school part of a federation?", "No"),
         new("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", "No"),
         new(
            "Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?",
            "No"),
         new("Please provide a list of your main feeder schools", application.SchoolAdFeederSchools),
         new("Has an equalities impact assessment been carried out and considered by the governing body?", "No"),
         new("Do you want to add any further information?", "No")
      };

      FurtherInformationSection formSection = new(application);
      formSection.Heading.Should().Be("Further information");
      formSection.SubSections.Should().HaveCount(1);
      formSection.SubSections.First().Heading.Should().Be("Additional details");
      formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
   }

   [Fact]
   public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SchoolAdInspectedButReportNotPublished = true;
      application.SchoolOngoingSafeguardingInvestigations = true;
      application.SchoolPartOfLaReorganizationPlan = true;
      application.SchoolPartOfLaClosurePlan = true;
      application.SchoolFaithSchool = true;
      application.SchoolIsPartOfFederation = true;
      application.SchoolIsSupportedByFoundation = true;
      application.SchoolHasSACREException = true;
      application.SchoolAdEqualitiesImpactAssessmentCompleted = true;
      application.SchoolAdEqualitiesImpactAssessmentCompleted = true;
      application.SchoolAdditionalInformationAdded = true;

      FormField[] expectedFields = {
         new("What will the school bring to the trust they are joining?", application.SchoolAdSchoolContributionToTrust),
         new("Have Ofsted inspected the school but not published the report yet?", "Yes"),
         new("Provide the inspection date and a short summary of the outcome?", application.SchoolAdInspectedButReportNotPublishedExplain),
         new("Are there any safeguarding investigations ongoing at the school?", "Yes"),
         new("Details of the investigation", application.SchoolOngoingSafeguardingDetails),
         new("Is the school part of a local authority reorganisation?", "Yes"),
         new("Details of the reorganisation", application.SchoolLaReorganizationDetails),
         new("Is the school part of any local authority closure plans?", "Yes"),
         new("Details of the closure plan", application.SchoolLaClosurePlanDetails),
         new("Is your school linked to a diocese?", "Yes"),
         new("Name of diocese?", application.SchoolFaithSchoolDioceseName),
         new("Is your school part of a federation?", "Yes"),
         new("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", "Yes"),
         new("Name of this body?", application.SchoolSupportedFoundationBodyName),
         new(
            "Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?",
            "Yes"),
         new("When does the exemption end?", application.SchoolSACREExemptionEndDate?.ToDateString()),
         new("Please provide a list of your main feeder schools", application.SchoolAdFeederSchools),
         new("Has an equalities impact assessment been carried out and considered by the governing body?", "Yes"),
         new("When the governing body considered the equality duty what did they decide?", application.SchoolAdEqualitiesImpactAssessmentDetails),
         new("Do you want to add any further information?", "Yes"),
         new("Add any further information", application.SchoolAdditionalInformation)
      };

      FurtherInformationSection formSection = new(application);
      formSection.Heading.Should().Be("Further information");
      formSection.SubSections.Should().HaveCount(1);
      formSection.SubSections.First().Heading.Should().Be("Additional details");
      formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
   }
}
