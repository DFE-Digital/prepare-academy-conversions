using ApplyToBecome.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ApplyToBecome.Data.Models.AcademisationApplication.AcademisationApplication;
using Loan = Dfe.PrepareConversions.Data.Models.AcademisationApplication.Loan;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication
{
   public class PopulateSchoolLoansTests
   {
      [Fact]
      public void PopulateSchoolLoans_Adds_School_Loans()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            Loans = new List<Loan>
            {
               new() { Amount = 1000, Purpose = "Education", Provider = "Provider1", InterestRate = 2.5, Schedule = "Monthly" },
               new() { Amount = 2000, Purpose = "Training", Provider = "Provider2", InterestRate = 3.5, Schedule = "Yearly" },
            }
         };
         var academiesApplicationSchool = new ApplyingSchool { SchoolLoans = new List<Dfe.PrepareConversions.Data.Models.Application.Loan>() };
         // Act
         PopulateSchoolLoans(academiesApplicationSchool, academisationApplicationSchool);
         // Assert
         Assert.Equal(2, academiesApplicationSchool.SchoolLoans.Count);
         Assert.Equal(1000, academiesApplicationSchool.SchoolLoans.ElementAt(0).SchoolLoanAmount);
         Assert.Equal("Education", academiesApplicationSchool.SchoolLoans.ElementAt(0).SchoolLoanPurpose);
         Assert.Equal("Provider1", academiesApplicationSchool.SchoolLoans.ElementAt(0).SchoolLoanProvider);
         Assert.Equal("2.5", academiesApplicationSchool.SchoolLoans.ElementAt(0).SchoolLoanInterestRate);
         Assert.Equal("Monthly", academiesApplicationSchool.SchoolLoans.ElementAt(0).SchoolLoanSchedule);
         Assert.Equal(2000, academiesApplicationSchool.SchoolLoans.ElementAt(1).SchoolLoanAmount);
         Assert.Equal("Training", academiesApplicationSchool.SchoolLoans.ElementAt(1).SchoolLoanPurpose);
         Assert.Equal("Provider2", academiesApplicationSchool.SchoolLoans.ElementAt(1).SchoolLoanProvider);
         Assert.Equal("3.5", academiesApplicationSchool.SchoolLoans.ElementAt(1).SchoolLoanInterestRate);
         Assert.Equal("Yearly", academiesApplicationSchool.SchoolLoans.ElementAt(1).SchoolLoanSchedule);
      }
   }
}
