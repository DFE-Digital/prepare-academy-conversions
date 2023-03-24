using AutoFixture;
using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections
{
   public class TrustInformationSectionTests
   {
      [Fact]
      public void Constructor_generates_expected_fields_correctly()
      {
         Fixture fixture = new();
         Application application = fixture.Create<Application>();
         TrustInformationSection formSection = new(application);

         FormField[] expectedProposedNameFields = {
            new("Proposed name of the trust", $"{application.TrustName}")
         };
         FormField[] expectedOpeningDateFields = {
            new("When do you plan to establish the new multi-academy trust?", $"{application.FormTrustOpeningDate.ToDateString()}"),
            new("Approver full name", $"{application.TrustApproverName}"),
            new("Approver email address", $"{application.TrustApproverEmail}")
         };
         FormField[] expectedReasonsForJoiningFields = {
            new("Why are the schools forming the trust?", $"{application.FormTrustReasonForming}"),
            new("What vision and aspirations have the schools agreed to for the trust?", $"{application.FormTrustReasonVision}"),
            new("What geographical areas and communities will the trust serve?", $"{application.FormTrustReasonGeoAreas}"),
            new("How will the schools make the most of the freedom that academies have?", $"{application.FormTrustReasonFreedom}"),
            new("How will the schools work together to improve teaching and learning?", $"{application.FormTrustReasonImproveTeaching}")
         };
         FormField[] expectedPlansForGrowthFields = {
            new("Do you plan to grow the trust over the next 5 years?", $"{application.FormTrustGrowthPlansYesNo.ToYesNoString()}"),
            new("What are your plans?", $"{application.FormTrustPlansForNoGrowth ?? application.FormTrustPlanForGrowth}")
         };
         FormField[] expectedSchoolImprovementStrategy = {
            new("How will the trust support and improve the academies in the trust?", application.FormTrustImprovementSupport),
            new("How will the trust make sure that the school improvement strategy is fit for purpose and improve standards?", application.FormTrustImprovementStrategy),
            new("If the trust wants to become an approved sponsor, when would it plan to apply and begin sponsoring schools?", application.FormTrustImprovementApprovedSponsor)
         };

         formSection.Heading.Should().Be("Trust information");
         formSection.SubSections.Should().HaveCount(5);
         FormSubSection[] subSections = formSection.SubSections.ToArray();
         subSections[0].Heading.Should().Be(string.Empty);
         subSections[0].Fields.Should().BeEquivalentTo(expectedProposedNameFields);
         subSections[1].Heading.Should().Be("Opening date");
         subSections[1].Fields.Should().BeEquivalentTo(expectedOpeningDateFields);
         subSections[2].Heading.Should().Be("Reasons for forming the trust");
         subSections[2].Fields.Should().BeEquivalentTo(expectedReasonsForJoiningFields);
         subSections[3].Heading.Should().Be("Plans for growth");
         subSections[3].Fields.Should().BeEquivalentTo(expectedPlansForGrowthFields);
         subSections[4].Heading.Should().Be("School improvement strategy");
         subSections[4].Fields.Should().BeEquivalentTo(expectedSchoolImprovementStrategy);
      }
   }
}
