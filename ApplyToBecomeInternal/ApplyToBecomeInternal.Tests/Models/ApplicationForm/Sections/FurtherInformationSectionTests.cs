using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using AutoFixture;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using ApplyToBecomeInternal.Extensions;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class FurtherInformationSectionTests
	{
		[Fact]
		public void Constructor_Doesnt_Set_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();			
			application.SetDeficitStatus(false);
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
				new FormField("Has an equalities impact assessment been carried out and considered by the governing body?", "No"),
				new FormField("Do you want to add any further information?", "No")
			};
			
			var formSection = new FurtherInformationSection(application);
			formSection.Heading.Should().Be("Further information");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Additional details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
		[Fact]
		public void Constructor_Set_Conditional_Rows_Following_Yes_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SetDeficitStatus(false);
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

			var expectedFields = new[] {
				new FormField("What will the school bring to the trust they are joining?", application.SchoolAdSchoolContributionToTrust),
				new FormField("Have Ofsted inspected the school but not published the report yet?", "Yes"),
				new FormField("Provide the inspection date and a short summary of the outcome?", application.SchoolAdInspectedButReportNotPublishedExplain),
				new FormField("Are there any safeguarding investigations ongoing at the school?", "Yes"),
				new FormField("Details of the investigation", application.SchoolOngoingSafeguardingDetails),
				new FormField("Is the school part of a local authority reorganisation?", "Yes"),
				new FormField("Details of the reorganisation", application.SchoolLaReorganizationDetails),
				new FormField("Is the school part of any local authority closure plans?", "Yes"),
				new FormField("Details of the closure plan", application.SchoolLaClosurePlanDetails),
				new FormField("Is your school linked to a diocese?", "Yes"),
				new FormField("Name of diocese", application.SchoolFaithSchoolDioceseName),
				new FormField("Is your school part of a federation?", "Yes"),
				new FormField("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", "Yes"),
				new FormField("Name of this body", application.SchoolSupportedFoundationBodyName),
				new FormField("Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?", "Yes"),
				new FormField("When does the exemption end?", application.SchoolSACREExemptionEndDate.Value.ToDateString()),
				new FormField("Please provide a list of your main feeder schools", application.SchoolAdFeederSchools),
				new FormField("Has an equalities impact assessment been carried out and considered by the governing body?", "Yes"),
				new FormField("When the governing body considered the equality duty what did they decide?", application.SchoolAdEqualitiesImpactAssessmentDetails),
				new FormField("Do you want to add any further information?", "Yes"),
				new FormField("Add any further information", application.SchoolAdditionalInformation)
			};

			var formSection = new FurtherInformationSection(application);
			formSection.Heading.Should().Be("Further information");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Additional details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}