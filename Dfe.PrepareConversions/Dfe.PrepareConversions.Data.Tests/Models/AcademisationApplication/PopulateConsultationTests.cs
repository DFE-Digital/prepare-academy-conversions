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
   public class PopulateConsultationTests
   {
      [Fact]
      public void PopulateConsultation_ShouldCopyValuesCorrectly()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            SchoolHasConsultedStakeholders = true,
            SchoolPlanToConsultStakeholders = "false"
         };

         var academiesApplicationSchool = new ApplyingSchool();

         // Act
         PopulateConsultation(academiesApplicationSchool, academisationApplicationSchool);

         // Assert
         Assert.True(academiesApplicationSchool.SchoolHasConsultedStakeholders);
         Assert.Equal("false", academiesApplicationSchool.SchoolPlanToConsultStakeholders);
      }

   }
}
