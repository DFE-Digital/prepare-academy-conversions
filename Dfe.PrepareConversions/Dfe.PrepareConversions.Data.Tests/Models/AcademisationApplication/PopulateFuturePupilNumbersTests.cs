using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using Xunit;
using static Dfe.PrepareConversions.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication;

public class PopulateFuturePupilNumbersTests
{
   [Fact]
   public void PopulateFuturePupilNumbers_ShouldPopulateApplyingSchoolWithProjectedPupilNumbersAndSchoolCapacityDetails()
   {
      // Arrange
      School academisationApplicationSchool = new()
      {
         ProjectedPupilNumbersYear1 = 10,
         ProjectedPupilNumbersYear2 = 20,
         ProjectedPupilNumbersYear3 = 30,
         SchoolCapacityAssumptions = "Test Assumptions",
         SchoolCapacityPublishedAdmissionsNumber = 40
      };
      ApplyingSchool academiesApplicationSchool = new();

      // Act
      PopulateFuturePupilNumbers(academiesApplicationSchool, academisationApplicationSchool);

      // Assert
      Assert.Equal(10, academiesApplicationSchool.ProjectedPupilNumbersYear1);
      Assert.Equal(20, academiesApplicationSchool.ProjectedPupilNumbersYear2);
      Assert.Equal(30, academiesApplicationSchool.ProjectedPupilNumbersYear3);
      Assert.Equal("Test Assumptions", academiesApplicationSchool.SchoolCapacityAssumptions);
      Assert.Equal(40, academiesApplicationSchool.SchoolCapacityPublishedAdmissionsNumber);
   }
}
