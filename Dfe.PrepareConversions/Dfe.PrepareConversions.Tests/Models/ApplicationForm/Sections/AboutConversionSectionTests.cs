using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections;

public class AboutConversionSectionTests
{
   [Fact]
   public void Constructor_SetsOtherMainContactFields_WhenMainContactIsOther()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SchoolConversionTargetDate = DateTime.Now.AddDays(30);
      application.SchoolConversionTargetDateSpecified = true;
      application.SchoolConversionChangeNamePlanned = true;
      application.SchoolConversionContactRole = "Other";

      AboutConversionSection formSection = new(application);

      FormField[] expectedFields = { new("The name of the school", application.SchoolName) };

      FormField[] expectedFieldsContactDetails = {
         new("Name of headteacher", application.SchoolConversionContactHeadName),
         new("Headteacher's email address", application.SchoolConversionContactHeadEmail),
         new("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
         new("Chair's email address", application.SchoolConversionContactChairEmail),
         new("Who is the main contact for the conversion?", application.SchoolConversionContactRole),
         new("Main contact's name", application.SchoolConversionMainContactOtherName),
         new("Main contact's email address", application.SchoolConversionMainContactOtherEmail),
         new("Main contact's role", application.SchoolConversionMainContactOtherRole),
         new("Approver's name", application.SchoolConversionApproverContactName),
         new("Approver's email address", application.SchoolConversionApproverContactEmail)
      };

      FormField[] expectedFieldsDateForConversion = {
         new("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString()),
         new("Preferred date", application.SchoolConversionTargetDate.Value.ToDateString()),
         new("Explain why you want to convert on this date", application.SchoolConversionTargetDateExplained)
      };

      FormField expectedFieldsReasonForJoining = new("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);
      FormField[] expectedFieldsNameChanges = {
         new("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString()),
         new("What's the proposed new name?", application.SchoolConversionProposedNewSchoolName)
      };

      formSection.Heading.Should().Be("About the conversion");

      FormSubSection[] sectionArray = formSection.SubSections.ToArray();

      sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
      sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
      sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
      sectionArray[3].Fields.Should().ContainEquivalentOf(expectedFieldsReasonForJoining);
      sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
   }

   [Fact]
   public void Constructor_Doesnt_Include_Conditional_Rows_Following_No_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SchoolConversionTargetDateSpecified = false;
      application.SchoolConversionChangeNamePlanned = false;

      AboutConversionSection formSection = new(application);
      FormField[] expectedFields = { new("The name of the school", application.SchoolName) };

      FormField[] expectedFieldsContactDetails = {
         new("Name of headteacher", application.SchoolConversionContactHeadName),
         new("Headteacher's email address", application.SchoolConversionContactHeadEmail),
         new("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
         new("Chair's email address", application.SchoolConversionContactChairEmail),
         new("Who is the main contact for the conversion?", application.SchoolConversionContactRole),
         new("Approver's name", application.SchoolConversionApproverContactName),
         new("Approver's email address", application.SchoolConversionApproverContactEmail)
      };

      FormField[] expectedFieldsDateForConversion = {
         new("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString())
      };

      FormField expectedFieldsReasonForJoining = new("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);

      FormField[] expectedFieldsNameChanges = {
         new("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString())
      };

      formSection.Heading.Should().Be("About the conversion");

      FormSubSection[] sectionArray = formSection.SubSections.ToArray();

      sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
      sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
      sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
      sectionArray[3].Fields.Should().ContainEquivalentOf(expectedFieldsReasonForJoining);
      sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
   }

   [Fact]
   public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers()
   {
      Fixture fixture = new();
      ApplyingSchool application = fixture.Create<ApplyingSchool>();
      application.SchoolConversionTargetDate = DateTime.Now.AddDays(30);
      application.SchoolConversionTargetDateSpecified = true;
      application.SchoolConversionChangeNamePlanned = true;

      AboutConversionSection formSection = new(application);

      FormField[] expectedFields = { new("The name of the school", application.SchoolName) };

      FormField[] expectedFieldsContactDetails = {
         new("Name of headteacher", application.SchoolConversionContactHeadName),
         new("Headteacher's email address", application.SchoolConversionContactHeadEmail),
         new("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
         new("Chair's email address", application.SchoolConversionContactChairEmail),
         new("Who is the main contact for the conversion?", application.SchoolConversionContactRole),
         new("Approver's name", application.SchoolConversionApproverContactName),
         new("Approver's email address", application.SchoolConversionApproverContactEmail)
      };

      FormField[] expectedFieldsDateForConversion = {
         new("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString()),
         new("Preferred date", application.SchoolConversionTargetDate.Value.ToDateString()),
         new("Explain why you want to convert on this date", application.SchoolConversionTargetDateExplained)
      };

      FormField expectedFieldsReasonForJoining = new("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);

      FormField[] expectedFieldsNameChanges = {
         new("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString()),
         new("What's the proposed new name?", application.SchoolConversionProposedNewSchoolName)
      };

      formSection.Heading.Should().Be("About the conversion");

      FormSubSection[] sectionArray = formSection.SubSections.ToArray();

      sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
      sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
      sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
      sectionArray[3].Fields.Should().ContainEquivalentOf(expectedFieldsReasonForJoining);
      sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
   }
}
