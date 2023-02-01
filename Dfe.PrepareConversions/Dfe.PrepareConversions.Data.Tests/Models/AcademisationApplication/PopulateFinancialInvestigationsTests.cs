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
   public class PopulateFinancialInvestigationsTests
   {
      [Fact]
      public void PopulateFinancialInvestigations_ShouldPopulateApplyingSchoolWithFinancialInvestigationsDetails()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            FinanceOngoingInvestigations = true,
            FinancialInvestigationsExplain = "Test Explanation",
            FinancialInvestigationsTrustAware = false
         };
         var academiesApplicationSchool = new ApplyingSchool();
         // Act
         PopulateFinancialInvestigations(academiesApplicationSchool, academisationApplicationSchool);
         // Assert
         Assert.True(academiesApplicationSchool.FinanceOngoingInvestigations);
         Assert.Equal("Test Explanation", academiesApplicationSchool.SchoolFinancialInvestigationsExplain);
         Assert.False(academiesApplicationSchool.SchoolFinancialInvestigationsTrustAware);
      }
   }
}
