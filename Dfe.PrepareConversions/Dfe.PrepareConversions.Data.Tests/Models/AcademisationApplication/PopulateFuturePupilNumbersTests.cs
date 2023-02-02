using ApplyToBecome.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ApplyToBecome.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication
{
   public  class PopulateFuturePupilNumbersTests
   {
      [Fact]
      public void PopulateFuturePupilNumbers_ShouldPopulateApplyingSchoolWithProjectedPupilNumbersAndSchoolCapacityDetails()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            ProjectedPupilNumbersYear1 = 10,
            ProjectedPupilNumbersYear2 = 20,
            ProjectedPupilNumbersYear3 = 30,
            SchoolCapacityAssumptions = "Test Assumptions",
            SchoolCapacityPublishedAdmissionNumber = "40"
         };
         var academiesApplicationSchool = new ApplyingSchool();

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
}
