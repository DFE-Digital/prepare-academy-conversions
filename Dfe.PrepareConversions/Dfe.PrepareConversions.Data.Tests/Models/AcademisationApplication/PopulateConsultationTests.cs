using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using Xunit;
using static Dfe.PrepareConversions.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication;

public class PopulateConsultationTests
{
   [Fact]
   public void PopulateConsultation_ShouldCopyValuesCorrectly()
   {
      // Arrange
      School academisationApplicationSchool = new() { SchoolHasConsultedStakeholders = true, SchoolPlanToConsultStakeholders = "false" };

      ApplyingSchool academiesApplicationSchool = new();

      // Act
      PopulateConsultation(academiesApplicationSchool, academisationApplicationSchool);

      // Assert
      Assert.True(academiesApplicationSchool.SchoolHasConsultedStakeholders);
      Assert.Equal("false", academiesApplicationSchool.SchoolPlanToConsultStakeholders);
   }
}
