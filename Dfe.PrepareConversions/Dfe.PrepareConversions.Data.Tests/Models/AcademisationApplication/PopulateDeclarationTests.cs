using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using Xunit;
using static Dfe.PrepareConversions.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication;

public class PopulateDeclarationTests
{
   [Fact]
   public void PopulateDeclaration_ShouldCopyValuesCorrectly()
   {
      // Arrange
      School academisationApplicationSchool = new() { DeclarationBodyAgree = true, DeclarationSignedByName = "John Doe" };

      ApplyingSchool academiesApplicationSchool = new();

      // Act
      PopulateDeclaration(academiesApplicationSchool, academisationApplicationSchool);

      // Assert
      Assert.True(academiesApplicationSchool.DeclarationBodyAgree);
      Assert.Equal("John Doe", academiesApplicationSchool.DeclarationSignedByName);
   }
}
