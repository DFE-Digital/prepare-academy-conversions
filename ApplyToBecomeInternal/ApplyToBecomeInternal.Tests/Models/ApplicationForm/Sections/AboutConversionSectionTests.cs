
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class AboutConversionSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application
			{
				School = new School { Name = "St Wilfrid's Primary School" },
				HeadTeacher = new ContactDetails
				{
					Name = "Garth Brown",
					EmailAddress = "garth.brown@stwilfridsprimary.edu.uk",
					TelephoneNumber = "09876 64547563"
				},
				GoverningBodyChair = new ContactDetails
				{
					Name = "Arna Siggurdottier",
					EmailAddress = "arna.siggurdottier@dynamicstrust.co.uk",
					TelephoneNumber = "0972 345 119"
				},
				Approver = new ContactDetails
				{
					Name = "Garth Brown",
					EmailAddress = "garth.brown@stwilfridsprimary.edu.uk",
				},
				DateForConversion = new DateForConversion
				{
					HasPreferredDate = true,
					PreferredDate = DateTime.UtcNow
				},
				SchoolToTrustRationale = "This is a rationale",
				WillSchoolChangeName = true
			};

			var formSection = new AboutConversionSection(application);

			var expectedFields = new[]
			{
				new FormField("The name of the school", "St Wilfrid's Primary School"),
			};

			var expectedFieldsContactDetails = new[]
			{
				new FormField("Name of headteacher", "Garth Brown"),
				new FormField("Headteacher's email address", "garth.brown@stwilfridsprimary.edu.uk"),
				new FormField("Headteacher's telephone number", "09876 64547563"),
				new FormField("Name of the chair of the Governing Body", "Arna Siggurdottier"),
				new FormField("Chair's email address", "arna.siggurdottier@dynamicstrust.co.uk"),
				new FormField("Chair's phone number", "0972 345 119"),
				new FormField("Approver's name", "Garth Brown"),
				new FormField("Approver's email address", "garth.brown@stwilfridsprimary.edu.uk"),
			};

			var expectedFieldsDateForConversion = new[] {
				new FormField("Do you want the conversion to happen on a particular date", "Yes"),
				new FormField("Preferred date", "21/4/2021")
			};

			var expectedFieldsReasonForJoining = new FormField("Why does the school want to join this trust in particular?", "This is a rationale");
			var expectedFieldsNameChanges = new FormField("Is the school planning to change its name when it becomes an academy?", "Yes");

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
