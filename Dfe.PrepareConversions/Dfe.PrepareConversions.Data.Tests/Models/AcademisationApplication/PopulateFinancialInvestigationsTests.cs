using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using Xunit;
using static Dfe.PrepareConversions.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication;

public class PopulateFinancialInvestigationsTests
{
   [Fact]
   public void PopulateFinancialInvestigations_ShouldPopulateApplyingSchoolWithFinancialInvestigationsDetails()
   {
      // Arrange
      School academisationApplicationSchool = new()
      {
         FinanceOngoingInvestigations = true, FinancialInvestigationsExplain = "Test Explanation", FinancialInvestigationsTrustAware = false
      };
      ApplyingSchool academiesApplicationSchool = new();
      // Act
      PopulateFinancialInvestigations(academiesApplicationSchool, academisationApplicationSchool);
      // Assert
      Assert.True(academiesApplicationSchool.FinanceOngoingInvestigations);
      Assert.Equal("Test Explanation", academiesApplicationSchool.SchoolFinancialInvestigationsExplain);
      Assert.False(academiesApplicationSchool.SchoolFinancialInvestigationsTrustAware);
   }
}
