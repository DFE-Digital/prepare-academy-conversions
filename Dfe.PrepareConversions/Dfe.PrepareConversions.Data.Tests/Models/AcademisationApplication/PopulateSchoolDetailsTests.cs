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
   public class PopulateSchoolDetailsTests
   {
      [Fact]
      public void PopulateSchoolDetails_Should_Populate_Fields_Correctly()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            SchoolName = "Old School Name",
            SchoolConversionContactHeadName = "Head Name",
            SchoolConversionContactHeadEmail = "head@email.com",
            SchoolConversionContactHeadTel = "1234567890",
            SchoolConversionContactRole = "Role",
            SchoolConversionContactChairName = "Chair Name",
            SchoolConversionContactChairEmail = "chair@email.com",
            SchoolConversionContactChairTel = "0987654321",
            SchoolConversionMainContactOtherName = "Other Name",
            SchoolConversionMainContactOtherEmail = "other@email.com",
            SchoolConversionMainContactOtherTelephone = "1111111111",
            SchoolConversionApproverContactName = "Approver Name",
            SchoolConversionApproverContactEmail = "approver@email.com",
            SchoolConversionTargetDateSpecified = true,
            SchoolConversionTargetDate = new DateTime(2023, 1, 1),
            SchoolConversionTargetDateExplained = "Target Date Explained",
            ApplicationJoinTrustReason = "Reasons",
            ConversionChangeNamePlanned = true,
            ProposedNewSchoolName = "New School Name"
         };

         var academiesApplicationSchool = new ApplyingSchool();

         // Act
         PopulateSchoolDetails(academiesApplicationSchool, academisationApplicationSchool);

         // Assert
         Assert.Equal("Old School Name", academiesApplicationSchool.SchoolName);
         Assert.Equal("Head Name", academiesApplicationSchool.SchoolConversionContactHeadName);
         Assert.Equal("head@email.com", academiesApplicationSchool.SchoolConversionContactHeadEmail);
         Assert.Equal("1234567890", academiesApplicationSchool.SchoolConversionContactHeadTel);
         Assert.Equal("Role", academiesApplicationSchool.SchoolConversionContactRole);
         Assert.Equal("Chair Name", academiesApplicationSchool.SchoolConversionContactChairName);
         Assert.Equal("chair@email.com", academiesApplicationSchool.SchoolConversionContactChairEmail);
         Assert.Equal("0987654321", academiesApplicationSchool.SchoolConversionContactChairTel);
         Assert.Equal("Other Name", academiesApplicationSchool.SchoolConversionMainContactOtherName);
         Assert.Equal("other@email.com", academiesApplicationSchool.SchoolConversionMainContactOtherEmail);
         Assert.Equal("1111111111", academiesApplicationSchool.SchoolConversionMainContactOtherTelephone);
         Assert.Equal("Approver Name", academiesApplicationSchool.SchoolConversionApproverContactName);
         Assert.Equal("approver@email.com", academiesApplicationSchool.SchoolConversionApproverContactEmail);
         Assert.True(academiesApplicationSchool.SchoolConversionTargetDateSpecified);
         Assert.Equal(new DateTime(2023, 1, 1), academiesApplicationSchool.SchoolConversionTargetDate);
         Assert.Equal("Target Date Explained", academisationApplicationSchool.SchoolConversionTargetDateExplained);
         Assert.Equal("Reasons", academisationApplicationSchool.ApplicationJoinTrustReason);
         Assert.True(academisationApplicationSchool.ConversionChangeNamePlanned);
         Assert.Equal("New School Name", academisationApplicationSchool.ProposedNewSchoolName);
      }
   }
}
