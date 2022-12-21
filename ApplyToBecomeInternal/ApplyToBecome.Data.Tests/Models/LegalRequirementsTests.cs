using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecome.Data.Tests.Models
{
   public class LegalRequirementsTests
   {
      [Fact]
      public void From_ShouldCreateLegalRequirementsWithCorrectValues()
      {
         // Arrange
         var project = new AcademyConversionProject
         {
            LegalRequirementsSectionComplete = true,
            GoverningBodyResolution = "yes",
            Consultation = "no",
            FoundationConsent = "notApplicable",
            DiocesanConsent = "notApplicable"
         };

         // Act
         var result = LegalRequirements.From(project);

         // Assert
         Assert.True(result.IsComplete);
         Assert.Equal(ThreeOptions.Yes, result.GoverningBodyApproved);
         Assert.Equal(ThreeOptions.No, result.ConsultationDone);
         Assert.Equal(ThreeOptions.NotApplicable, result.FoundationConsent);
         Assert.Equal(ThreeOptions.NotApplicable, result.DiocesanConsent);
      }


      [Theory]
      [InlineData("yes", "Yes")]
      [InlineData("no", "No")]
      [InlineData("notApplicable", "NotApplicable")]
      [InlineData("Other value", "Other value")]
      public void HandleLegalRequirementString_ShouldReturnCorrectValue(string input, string expectedOutput)
      {
         // Act
         var result = LegalRequirements.HandleLegalRequirementString(input);

         // Assert
         Assert.Equal(expectedOutput, result);
      }
   }
}


