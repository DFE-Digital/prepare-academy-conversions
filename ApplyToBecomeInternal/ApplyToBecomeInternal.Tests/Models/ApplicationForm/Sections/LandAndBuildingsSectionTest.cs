
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
	public class LandAndBuildingsSectionTest
	{
		[Fact (Skip = "complete when missng fields are implemented")]
		public void Constructor_WithApplication_SetsFields()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolBuildLandWorksPlanned = true;
			application.SchoolBuildLandSharedFacilities = true;
			application.SchoolBuildLandGrants = true;
			application.SchoolBuildLandPFIScheme = true;
			application.SchoolBuildLandPriorityBuildingProgramme = true;
			application.SchoolBuildLandFutureProgramme = true;

			throw new NotImplementedException();
		}

		[Fact]
		public void Constructor_Doesnt_Set_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			application.SchoolBuildLandWorksPlanned = false;
			application.SchoolBuildLandSharedFacilities = false;
			application.SchoolBuildLandGrants = false;
			application.SchoolBuildLandPFIScheme = false;
			application.SchoolBuildLandPriorityBuildingProgramme = false;
			application.SchoolBuildLandFutureProgramme = false;

			var expectedFields = new[]
			{
				new FormField("As far as you're aware, who owns or holds the school's buildings and land?", application.SchoolBuildLandOwnerExplained),
				new FormField("Are there any current planned building works?", "No"),
				new FormField("Are there any shared facilities on site?", "No"),
				new FormField("Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?", "No"),
				new FormField("Is the school part of a Private Finance Intiative (PFI) scheme?", "No"),
				new FormField("Is the school part of a Priority School Building Programme?", "No"),
				new FormField("Is the school part of a Building Schools for the Future Programme?", "No")
			};
			var formSection = new LandAndBuildingsSection(application);

			formSection.Heading.Should().Be("Land and buildings");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}
