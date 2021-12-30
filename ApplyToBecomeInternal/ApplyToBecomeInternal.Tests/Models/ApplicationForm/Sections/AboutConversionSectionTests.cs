using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using AutoFixture;
using ApplyToBecomeInternal.Extensions;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class AboutConversionSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var fixture = new Fixture();
			var application = fixture.Create<Application>();
			application.SchoolConversionTargetDateDate = new DateTime(2021, 04, 20);
			//var application = new Application
			//{
			//	SchoolName = "St Wilfrid's Primary School" ,
			//	ConversionInformation = new ConversionInformation
			//	{
			//		HeadTeacher = new ContactDetails
			//		{
			//			Name = "Garth Brown",
			//			EmailAddress = "garth.brown@stwilfridsprimary.edu.uk",
			//			TelephoneNumber = "09876 64547563"
			//		},
			//		GoverningBodyChair = new ContactDetails
			//		{
			//			Name = "Arna Siggurdottier",
			//			EmailAddress = "arna.siggurdottier@dynamicstrust.co.uk",
			//			TelephoneNumber = "0972 345 119"
			//		},
			//		Approver = new ContactDetails
			//		{
			//			Name = "Garth Brown",
			//			EmailAddress = "garth.brown@stwilfridsprimary.edu.uk",
			//		},
			//		DateForConversion = new DateForConversion
			//		{
			//			HasPreferredDate = true,
			//			PreferredDate = new DateTime(2021,04,20)
			//		},
			//		SchoolToTrustRationale = "This is a rationale",
			//		WillSchoolChangeName = true
			//	}
			//};

			var formSection = new AboutConversionSection(application);

			var expectedFields = new[]
			{
				new FormField("The name of the school", application.Name),
			};

			var expectedFieldsContactDetails = new[]
			{
				new FormField("Name of headteacher", application.SchoolConversionContactHeadName),
				new FormField("Headteacher's email address", application.SchoolConversionContactHeadEmail),
				new FormField("Headteacher's telephone number", application.SchoolConversionContactHeadTel),
				new FormField("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
				new FormField("Chair's email address", application.SchoolConversionContactChairEmail),
				new FormField("Chair's phone number", application.SchoolConversionContactChairTel),
				new FormField("Approver's name", application.SchoolConversionApproverContactName),
				new FormField("Approver's email address", application.SchoolConversionApproverContactEmail)
			};

			var expectedFieldsDateForConversion = new[] {
				new FormField("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateDifferent.ToYesNoString()),
				new FormField("Preferred date", "20/04/2021")
			};

			var expectedFieldsReasonForJoining = new FormField("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining);
			var expectedFieldsNameChanges = new FormField("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeName.ToYesNoString());

			formSection.Heading.Should().Be("About the conversion");

			var sectionArray = formSection.SubSections.ToArray();

			sectionArray[0].Fields.Should().BeEquivalentTo(expectedFields);
			sectionArray[1].Fields.Should().BeEquivalentTo(expectedFieldsContactDetails);
			sectionArray[2].Fields.Should().BeEquivalentTo(expectedFieldsDateForConversion);
			sectionArray[3].Fields.Should().BeEquivalentTo(expectedFieldsReasonForJoining);
			sectionArray[4].Fields.Should().BeEquivalentTo(expectedFieldsNameChanges);
		}
	}
}
