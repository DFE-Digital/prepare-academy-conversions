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
   public class PopulateSchoolLeasesTests
   {
      [Fact]
      public void PopulateSchoolLeases_Adds_School_Leases()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            Leases = new List<Lease>
            {
               new() { SchoolLeasePurpose = "Equipment", SchoolLeaseRepaymentValue = 1000, SchoolLeaseInterestRate = (decimal)2.5, SchoolLeasePaymentToDate = (decimal)2.5, SchoolLeaseResponsibleForAssets = "School", SchoolLeaseValueOfAssets = "5000", SchoolLeaseTerm = "36" },
               new() { SchoolLeasePurpose = "Facility", SchoolLeaseRepaymentValue = 2000, SchoolLeaseInterestRate = (decimal)3.5, SchoolLeasePaymentToDate = (decimal)2.5, SchoolLeaseResponsibleForAssets = "Landlord", SchoolLeaseValueOfAssets = "10000", SchoolLeaseTerm = "48" },
            }
         };
         var academiesApplicationSchool = new ApplyingSchool { SchoolLeases = new List<Lease>() };

         // Act
         PopulateSchoolLeases(academiesApplicationSchool, academisationApplicationSchool);

         // Assert
         Assert.Equal(2, academiesApplicationSchool.SchoolLeases.Count);
         Assert.Equal("Equipment", academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeasePurpose);
         Assert.Equal(1000, academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeaseRepaymentValue);
         Assert.Equal((decimal)2.5, academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeaseInterestRate);
         Assert.Equal((decimal)2.5, academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeasePaymentToDate);
         Assert.Equal("School", academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeaseResponsibleForAssets);
         Assert.Equal("5000", academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeaseValueOfAssets);
         Assert.Equal("36", academiesApplicationSchool.SchoolLeases.ElementAt(0).SchoolLeaseTerm);
         Assert.Equal("Facility", academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeasePurpose);
         Assert.Equal(2000, academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeaseRepaymentValue);
         Assert.Equal((decimal)3.5, academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeaseInterestRate);
         Assert.Equal((decimal)2.5, academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeasePaymentToDate);
         Assert.Equal("Landlord", academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeaseResponsibleForAssets);
         Assert.Equal("10000", academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeaseValueOfAssets);
         Assert.Equal("48", academiesApplicationSchool.SchoolLeases.ElementAt(1).SchoolLeaseTerm);
      }
   }
}
