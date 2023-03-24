using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Models;

public class LegalRequirementsTests
{
   [Fact]
   public void From_ShouldCreateLegalRequirementsWithCorrectValues()
   {
      // Arrange
      AcademyConversionProject project = new()
      {
         LegalRequirementsSectionComplete = true,
         GoverningBodyResolution = "yes",
         Consultation = "no",
         FoundationConsent = "notApplicable",
         DiocesanConsent = "notApplicable"
      };

      // Act
      LegalRequirements result = LegalRequirements.From(project);

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
      string result = LegalRequirements.HandleLegalRequirementString(input);

      // Assert
      Assert.Equal(expectedOutput, result);
   }
}
