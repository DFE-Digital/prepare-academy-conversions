
using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using AutoFixture;
using FluentAssertions;
using System.Linq;
using Xunit;
using ApplyToBecomeInternal.Extensions;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class LandAndBuildingsSectionTest
	{
		private static void SetBooleanQuestionsToTrue(ApplyingSchool application, bool value)
		{
			application.SchoolBuildLandWorksPlanned = value;
			application.SchoolBuildLandSharedFacilities = value;
			application.SchoolBuildLandGrants = value;
			application.SchoolBuildLandPFIScheme = value;
			application.SchoolBuildLandPriorityBuildingProgramme = value;
			application.SchoolBuildLandFutureProgramme = value;
			application.SchoolBuildLandSharedFacilities = value;
		}

		[Fact]
		public void Constructor_Doesnt_Include_Conditional_Rows_Following_No_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			SetBooleanQuestionsToTrue(application, false);

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

		[Fact]
		public void Constructor_Includes_Conditional_Rows_Following_Yes_Answers()
		{
			var fixture = new Fixture();
			var application = fixture.Create<ApplyingSchool>();
			SetBooleanQuestionsToTrue(application, true);
			var expectedFields = new[]
			{
				new FormField("As far as you're aware, who owns or holds the school's buildings and land?", application.SchoolBuildLandOwnerExplained),
				new FormField("Are there any current planned building works?", "Yes"),
				new FormField("Provide details of the works, how they'll be funded and whether the funding will be affected by the conversion", application.SchoolBuildLandWorksPlannedExplained),
			    new FormField("When is the scheduled completion date?", application.SchoolBuildLandWorksPlannedCompletionDate.Value.ToDateString()),
				new FormField("Are there any shared facilities on site?", "Yes"),
				new FormField("List the facilities and the school's plan for them after converting", application.SchoolBuildLandSharedFacilitiesExplained),
				new FormField("Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?", "Yes"),
				new FormField("Which bodies awarded the grants and what facilities did they fund?", application.SchoolBuildLandGrantsExplained),
				new FormField("Is the school part of a Private Finance Intiative (PFI) scheme?", "Yes"),
				new FormField("What kind of PFI Scheme is your school part of?", application.SchoolBuildLandPFISchemeType),
			    new FormField("Is the school part of a Priority School Building Programme?", "Yes"),
				new FormField("Is the school part of a Building Schools for the Future Programme?", "Yes"),
			};
			var formSection = new LandAndBuildingsSection(application);

			formSection.Heading.Should().Be("Land and buildings");
			formSection.SubSections.Should().HaveCount(1);
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}
