using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class FurtherInformationSectionTests
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application
			{
				FurtherInformation = new FurtherInformation
				{
					WhatWillSchoolBringToTrust = "the school will bring these things to the trust",
					HasUnpublishedOfstedInspection = false,
					HasSafeguardingInvestigations = false,
					IsPartOfLocalAuthorityReorganisation = false,
					IsPartOfLocalAuthorityClosurePlans = false,
					IsLinkedToDiocese = true,
					NameOfDiocese = "Diocese of Warrington",
					DioceseLetterOfConsent = new Link("consent-from-diocese.docx", "#"),
					IsPartOfFederation = false,
					IsSupportedByFoundationTrustOrOtherBody = true,
					HasSACREChristianWorshipExcemption = false,
					MainFeederSchools = "n/a as we are a primary school",
					SchoolConsent = new Link("consent.docx", "#"),
					EqualitiesImpactAssessmentResult = "Considered unlikely",
					AdditionalInformation = null
				}
			};

			var exectedFields = new[] {
				new FormField("What will the school bring to the trust they are joining?", "the school will bring these things to the trust"),
				new FormField("Have Ofsted inspected the school but not published the report yet?", "No"),
				new FormField("Are there any safeguarding investigations ongoing at the school?", "No"),
				new FormField("Is the school part of a local authority reorganisation?", "No"),
				new FormField("Is the school part of any local authority closure plans?", "No"),
				new FormField("Is your school linked to a diocese?", "Yes"),
				new FormField("Name of diocese?", "Diocese of Warrington"),
				new LinkFormField("Upload a letter of consent from the diocese", "consent-from-diocese.docx", "#"),
				new FormField("Is your school part of a federation?", "No"),
				new FormField("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", "Yes"),
				new FormField("Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?", "No"),
				new FormField("Please provide a list of your main feeder schools", "n/a as we are a primary school"),
				new LinkFormField("The school's Governing Body must have passed a resolution to apply to convert to academy status. Upload a copy of the school’s consent to converting and joining the trust.", "consent.docx", "#"),
				new FormField("Has an equalities impact assessment been carried out and considered by the governing body?", "Considered unlikely"),
				new FormField("Do you want to add any further information?", "No")
			};
			
			var formSection = new FurtherInformationSection(application);
			formSection.Heading.Should().Be("Further information");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Heading.Should().Be("Additional details");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(exectedFields);
		}
	}
}