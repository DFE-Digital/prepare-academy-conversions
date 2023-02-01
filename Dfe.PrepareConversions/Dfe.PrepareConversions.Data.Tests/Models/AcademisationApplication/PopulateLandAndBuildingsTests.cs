using ApplyToBecome.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ApplyToBecome.Data.Models.AcademisationApplication.AcademisationApplication;
using LandAndBuildings = ApplyToBecome.Data.Models.AcademisationApplication.LandAndBuildings;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication
{
   public class PopulateLandAndBuildingsTests
   {
      [Fact]
      public void PopulateLandAndBuildings_Populates_Properties_Correctly()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            LandAndBuildings = new LandAndBuildings
            {
               OwnerExplained = "Owner",
               WorksPlanned = true,
               WorksPlannedExplained = "Works planned explanation",
               WorksPlannedDate = DateTime.Today,
               FacilitiesShared = true,
               FacilitiesSharedExplained = "Facilities shared explanation",
               Grants = true,
               GrantsAwardingBodies = "Grants awarding bodies",
               PartOfPfiScheme = true,
               PartOfPfiSchemeType = "PFI scheme type",
               PartOfPrioritySchoolsBuildingProgramme = true,
               PartOfBuildingSchoolsForFutureProgramme = true
            },
            SchoolSupportGrantFundsPaidTo = "School support grant funds paid to"
         };
         var academiesApplicationSchool = new ApplyingSchool();

         // Act
         PopulateLandAndBuildings(academiesApplicationSchool, academisationApplicationSchool);

         // Assert
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.OwnerExplained, academiesApplicationSchool.SchoolBuildLandOwnerExplained);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.WorksPlanned, academiesApplicationSchool.SchoolBuildLandWorksPlanned);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.WorksPlannedExplained, academiesApplicationSchool.SchoolBuildLandWorksPlannedExplained);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.WorksPlannedDate, academiesApplicationSchool.SchoolBuildLandWorksPlannedCompletionDate);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.FacilitiesShared, academiesApplicationSchool.SchoolBuildLandSharedFacilities);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.FacilitiesSharedExplained, academiesApplicationSchool.SchoolBuildLandSharedFacilitiesExplained);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.Grants, academiesApplicationSchool.SchoolBuildLandGrants);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.GrantsAwardingBodies, academiesApplicationSchool.SchoolBuildLandGrantsExplained);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.PartOfPfiScheme, academiesApplicationSchool.SchoolBuildLandPFIScheme);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.PartOfPfiSchemeType, academiesApplicationSchool.SchoolBuildLandPFISchemeType);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.PartOfPrioritySchoolsBuildingProgramme, academiesApplicationSchool.SchoolBuildLandPriorityBuildingProgramme);
         Assert.Equal(academisationApplicationSchool.LandAndBuildings.PartOfBuildingSchoolsForFutureProgramme, academiesApplicationSchool.SchoolBuildLandFutureProgramme);
         Assert.Equal(academisationApplicationSchool.SchoolSupportGrantFundsPaidTo, academiesApplicationSchool.SchoolSupportGrantFundsPaidTo);
      }
   }
}
