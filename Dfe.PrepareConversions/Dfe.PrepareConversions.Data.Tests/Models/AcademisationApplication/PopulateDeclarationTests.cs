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
   public class PopulateDeclarationTests
   {
      [Fact]
      public void PopulateDeclaration_ShouldCopyValuesCorrectly()
      {
         // Arrange
         var academisationApplicationSchool = new School
         {
            DeclarationBodyAgree = true,
            DeclarationSignedByName = "John Doe"
         };

         var academiesApplicationSchool = new ApplyingSchool();

         // Act
         PopulateDeclaration(academiesApplicationSchool, academisationApplicationSchool);

         // Assert
         Assert.True(academiesApplicationSchool.DeclarationBodyAgree);
         Assert.Equal("John Doe", academiesApplicationSchool.DeclarationSignedByName);
      }
   }
}
