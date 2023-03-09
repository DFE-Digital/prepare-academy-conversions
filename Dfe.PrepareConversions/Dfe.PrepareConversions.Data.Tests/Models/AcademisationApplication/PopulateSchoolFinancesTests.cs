using ApplyToBecome.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ApplyToBecome.Data.Models.AcademisationApplication.AcademisationApplication;
using FinancialYear = ApplyToBecome.Data.Models.AcademisationApplication.FinancialYear;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication
{
   public class PopulateSchoolFinancesTests
   {
      [Fact]
      public void PopulateSchoolFinances_MapsValuesCorrectly()
      {
         var academisationApplicationSchool = new School
         {
            PreviousFinancialYear =
               new FinancialYear
               {
                  FinancialYearEndDate = new System.DateTime(2020, 8, 31),
                  Revenue = 100,
                  RevenueStatus = "deficit",
                  RevenueStatusExplained = "Revenue is in deficit",
                  CapitalCarryForward = 200,
                  CapitalCarryForwardExplained = "Capital carry forward explained",
                  CapitalCarryForwardStatus = "deficit"
               },
            CurrentFinancialYear =
               new FinancialYear
               {
                  FinancialYearEndDate = new System.DateTime(2021, 8, 31),
                  Revenue = 300,
                  RevenueStatus = "surplus",
                  RevenueStatusExplained = "Revenue is in surplus",
                  CapitalCarryForward = 400,
                  CapitalCarryForwardExplained = "Capital carry forward explained",
                  CapitalCarryForwardStatus = "surplus"
               },
            NextFinancialYear = new FinancialYear
            {
               FinancialYearEndDate = new System.DateTime(2022, 8, 31),
               Revenue = 500,
               RevenueStatus = "deficit",
               RevenueStatusExplained = "Revenue is in deficit",
               CapitalCarryForward = 600,
               CapitalCarryForwardExplained = "Capital carry forward explained",
               CapitalCarryForwardStatus = "deficit"
            },
         };
         var academiesApplicationSchool = new ApplyingSchool();
         PopulateSchoolFinances(academiesApplicationSchool, academisationApplicationSchool);
         Assert.Equal(new System.DateTime(2020, 8, 31), academiesApplicationSchool.PreviousFinancialYear.FYEndDate);
         Assert.Equal(100, academiesApplicationSchool.PreviousFinancialYear.RevenueCarryForward);
         Assert.True(academiesApplicationSchool.PreviousFinancialYear.RevenueIsDeficit);
         Assert.Equal("Revenue is in deficit", academiesApplicationSchool.PreviousFinancialYear.RevenueStatusExplained);
         Assert.Equal(200, academiesApplicationSchool.PreviousFinancialYear.CapitalCarryForward);
         Assert.True(academiesApplicationSchool.PreviousFinancialYear.CapitalIsDeficit);
         Assert.Equal("Capital carry forward explained", academiesApplicationSchool.PreviousFinancialYear.CapitalStatusExplained);

         Assert.Equal(new System.DateTime(2021, 8, 31), academiesApplicationSchool.CurrentFinancialYear.FYEndDate);
         Assert.Equal(300, academiesApplicationSchool.CurrentFinancialYear.RevenueCarryForward);
         Assert.False(academiesApplicationSchool.CurrentFinancialYear.RevenueIsDeficit);
         Assert.Equal("Revenue is in surplus", academiesApplicationSchool.CurrentFinancialYear.RevenueStatusExplained);
         Assert.Equal(400, academiesApplicationSchool.CurrentFinancialYear.CapitalCarryForward);
         Assert.False(academiesApplicationSchool.CurrentFinancialYear.CapitalIsDeficit);
         Assert.Equal("Capital carry forward explained", academiesApplicationSchool.CurrentFinancialYear.CapitalStatusExplained);

         Assert.Equal(new System.DateTime(2022, 8, 31), academiesApplicationSchool.NextFinancialYear.FYEndDate);
         Assert.Equal(500, academiesApplicationSchool.NextFinancialYear.RevenueCarryForward);
         Assert.True(academiesApplicationSchool.NextFinancialYear.RevenueIsDeficit);
         Assert.Equal("Revenue is in deficit", academiesApplicationSchool.NextFinancialYear.RevenueStatusExplained);
         Assert.Equal(600, academiesApplicationSchool.NextFinancialYear.CapitalCarryForward);
         Assert.True(academiesApplicationSchool.NextFinancialYear.CapitalIsDeficit);
         Assert.Equal("Capital carry forward explained", academiesApplicationSchool.NextFinancialYear.CapitalStatusExplained);
      }
   }
}
