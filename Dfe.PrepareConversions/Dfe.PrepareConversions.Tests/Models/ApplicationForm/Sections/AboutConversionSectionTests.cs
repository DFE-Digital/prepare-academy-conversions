using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using AutoFixture;
using Dfe.PrepareConversions.Extensions;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections
{
	public class AboutConversionSectionTests
	{
		[Fact]
		public void Constructor_SetsOtherMainContactFields_WhenMainContactIsOther()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolConversionTargetDate = DateTime.Now.AddDays(30);
			application.SchoolConversionTargetDateSpecified = true;
			application.SchoolConversionChangeNamePlanned = true;
			application.SchoolConversionContactRole = "Other";

			var formSection = new AboutConversionSection(application);

			var expectedFields = new[]
			{
				new FormField("The name of the school", application.SchoolName),
			};

			var expectedFieldsContactDetails = new[]
			{
				new FormField("Name of headteacher", application.SchoolConversionContactHeadName),
				new FormField("Headteacher's email address", application.SchoolConversionContactHeadEmail),
				new FormField("Headteacher's phone number", application.SchoolConversionContactHeadTel),
				new FormField("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
				new FormField("Chair's email address", application.SchoolConversionContactChairEmail),
				new FormField("Chair's phone number", application.SchoolConversionContactChairTel),
				new FormField("Who is the main contact for the conversion?", application.SchoolConversionContactRole),
				new FormField("Main contact's name", application.SchoolConversionMainContactOtherName),
				new FormField("Main contact's email address", application.SchoolConversionMainContactOtherEmail),
				new FormField("Main contact's phone number", application.SchoolConversionMainContactOtherTelephone),
				new FormField("Main contact's role", application.SchoolConversionMainContactOtherRole),
				new FormField("Approver's name", application.SchoolConversionApproverContactName),
				new FormField("Approver's email address", application.SchoolConversionApproverContactEmail)							
			};
			
			var expectedFieldsDateForConversion = new[] {
				new FormField("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString()),
				new FormField("Preferred date", application.SchoolConversionTargetDate.Value.ToDateString()),
				new FormField("Explain why you want to convert on this date", application.SchoolConversionTargetDateExplained)
			};

			var expectedFieldsReasonForJoining = new FormField("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);
			var expectedFieldsNameChanges = new[]
			{ 
				new FormField("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString()),
				new FormField("What's the proposed new name?", application.SchoolConversionProposedNewSchoolName)
			};
			
			formSection.Heading.Should().Be("About the conversion");

			var sectionArray = formSection.SubSections.ToArray();

			sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
			sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
			sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
			sectionArray[3].Fields.Should().ContainEquivalentOf(expectedFieldsReasonForJoining);
			sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
		}

		[Fact]
		public void Constructor_Doesnt_Include_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolConversionTargetDateSpecified = false;
			application.SchoolConversionChangeNamePlanned = false;

			var formSection = new AboutConversionSection(application);
			var expectedFields = new[]
			{
				new FormField("The name of the school", application.SchoolName),
			};

			var expectedFieldsContactDetails = new[]
			{
				new FormField("Name of headteacher", application.SchoolConversionContactHeadName),
				new FormField("Headteacher's email address", application.SchoolConversionContactHeadEmail),
				new FormField("Headteacher's phone number", application.SchoolConversionContactHeadTel),
				new FormField("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
				new FormField("Chair's email address", application.SchoolConversionContactChairEmail),
				new FormField("Chair's phone number", application.SchoolConversionContactChairTel),
				new FormField("Who is the main contact for the conversion?", application.SchoolConversionContactRole),
				new FormField("Approver's name", application.SchoolConversionApproverContactName),
				new FormField("Approver's email address", application.SchoolConversionApproverContactEmail)
			};

			var expectedFieldsDateForConversion = new[] {
				new FormField("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString())				
			};

			var expectedFieldsReasonForJoining = new FormField("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);

			var expectedFieldsNameChanges = new[]
			{
				new FormField("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString())				
			};

			formSection.Heading.Should().Be("About the conversion");

			var sectionArray = formSection.SubSections.ToArray();

			sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
			sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
			sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
			sectionArray[3].Fields.Should().ContainEquivalentOf(expectedFieldsReasonForJoining);
			sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
		}

		[Fact]
		public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolConversionTargetDate = DateTime.Now.AddDays(30);
			application.SchoolConversionTargetDateSpecified = true;
			application.SchoolConversionChangeNamePlanned = true;

			var formSection = new AboutConversionSection(application);

			var expectedFields = new[]
			{
				new FormField("The name of the school", application.SchoolName),
			};

			var expectedFieldsContactDetails = new[]
			{
				new FormField("Name of headteacher", application.SchoolConversionContactHeadName),
				new FormField("Headteacher's email address", application.SchoolConversionContactHeadEmail),
				new FormField("Headteacher's phone number", application.SchoolConversionContactHeadTel),
				new FormField("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
				new FormField("Chair's email address", application.SchoolConversionContactChairEmail),
				new FormField("Chair's phone number", application.SchoolConversionContactChairTel),
				new FormField("Who is the main contact for the conversion?", application.SchoolConversionContactRole),
				new FormField("Approver's name", application.SchoolConversionApproverContactName),
				new FormField("Approver's email address", application.SchoolConversionApproverContactEmail)
			};

			var expectedFieldsDateForConversion = new[] {
				new FormField("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString()),
				new FormField("Preferred date", application.SchoolConversionTargetDate.Value.ToDateString()),
				new FormField("Explain why you want to convert on this date", application.SchoolConversionTargetDateExplained)
			};

			var expectedFieldsReasonForJoining = new FormField("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);

			var expectedFieldsNameChanges = new[]
			{
				new FormField("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString()),
				new FormField("What's the proposed new name?", application.SchoolConversionProposedNewSchoolName)
			};

			formSection.Heading.Should().Be("About the conversion");

			var sectionArray = formSection.SubSections.ToArray();

			sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
			sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
			sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
			sectionArray[3].Fields.Should().ContainEquivalentOf(expectedFieldsReasonForJoining);
			sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
		}
	}
}
