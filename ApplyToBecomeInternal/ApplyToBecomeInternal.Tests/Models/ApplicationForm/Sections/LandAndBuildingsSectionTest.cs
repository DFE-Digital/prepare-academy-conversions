
using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class LandAndBuildingsSectionTest
	{
		[Fact]
		public void Constructor_WithApplication_SetsFields()
		{
			var application = new Application
			{
				LandAndBuildings = new LandAndBuildings
				{
					BuildingAndLandOwner = "The Diocese of Warrington owns the building and the land. The LA owns the playing fields.",
					HasCurrentPlannedBuildingWorks = true,
					HasSharedFacilitiesOnSite = false,
					HasSchoolGrants = false,
					HasPrivateFinanceInitiativeScheme = false,
					IsInPrioritySchoolBuildingProgramme = false,
					IsInBuildingSchoolsForFutureProgramme = false
				}
			};


			var expectedFields = new[]
			{
				new FormField("As far as you're aware, who owns or holds the school's buildings and land?", "The Diocese of Warrington owns the building and the land. The LA owns the playing fields."),
				new FormField("Are there any current planned building works?", "Yes"),
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
