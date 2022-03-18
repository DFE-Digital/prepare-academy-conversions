using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using AutoFixture;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class FurtherInformationSectionTests
	{
		[Fact(Skip = "complete when missng fields are implemented")]
		public void Constructor_SetsFields()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolAdInspectedButReportNotPublished = true;
			application.SchoolOngoingSafeguardingInvestigations = true;
			application.SchoolPartOfLaReorganizationPlan = true;
			application.SchoolPartOfLaClosurePlan = true;
			application.SchoolFaithSchool = true;
			application.SchoolIsPartOfFederation = true;
			application.SchoolIsSupportedByFoundation = true;
			application.SchoolHasSACREException = true;

			var expectedFields = new[]
			{
				new FormField("Is your school linked to a diocese?", "No"),
				new FormField("Name of diocese?", application.SchoolFaithSchoolDioceseName),
				new LinkFormField("Upload a letter of consent from the diocese",
					"DiocesePermissionEvidenceDocument",
					application.DiocesePermissionEvidenceDocumentLink)
			};

			throw new NotImplementedException();
		}

		[Fact]
		public void Constructor_Doesnt_Set_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolAdInspectedButReportNotPublished = false;
			application.SchoolOngoingSafeguardingInvestigations = false;
			application.SchoolPartOfLaReorganizationPlan = false;
			application.SchoolPartOfLaClosurePlan = false;
			application.SchoolFaithSchool = false;
			application.SchoolIsPartOfFederation = false;
			application.SchoolIsSupportedByFoundation = false;
			application.SchoolHasSACREException = false;
			application.SchoolAdEqualitiesImpactAssessmentCompleted = false;
			application.SchoolAdditionalInformationAdded = false;

			var expectedFields = new[] {
				new FormField("What will the school bring to the trust they are joining?", application.SchoolAdSchoolContributionToTrust),
				new FormField("Have Ofsted inspected the school but not published the report yet?", "No"),
				new FormField("Are there any safeguarding investigations ongoing at the school?", "No"),
				new FormField("Is the school part of a local authority reorganisation?", "No"),
				new FormField("Is the school part of any local authority closure plans?", "No"),
				new FormField("Is your school linked to a diocese?", "No"),
				new FormField("Is your school part of a federation?", "No"),
				new FormField("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", "No"),
				new FormField("Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?", "No"),
				new FormField("Please provide a list of your main feeder schools", application.SchoolAdFeederSchools),
				new LinkFormField("The school's Governing Body must have passed a resolution to apply to convert to academy status. Upload a copy of the school’s consent to converting and joining the trust.",
					"Governing body resolution document", 
					application.GoverningBodyConsentEvidenceDocumentLink),
				new FormField("Has an equalities impact assessment been carried out and considered by the governing body?", "No"),
				new FormField("Do you want to add any further information?", "No")
			};
			
			var formSection = new FurtherInformationSection(application);
			formSection.Heading.Should().Be("Further information");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Additional details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}