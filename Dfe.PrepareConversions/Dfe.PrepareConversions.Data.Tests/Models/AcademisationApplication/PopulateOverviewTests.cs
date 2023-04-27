using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Dfe.PrepareConversions.Data.Models.AcademisationApplication.AcademisationApplication;

namespace Dfe.PrepareConversions.Data.Tests.Models.AcademisationApplication;

public class PopulateOverviewTests
{
   [Fact]
   public void PopulateOverview_PopulatesApplicationWithExpectedValues()
   {
      // Arrange
      Data.Models.AcademisationApplication.AcademisationApplication academisationApplication =
         new()
         {
            ApplicationReference = "A12345",
            ApplicationType = GlobalStrings.JoinAMat,
            JoinTrustDetails = new JoinTrustDetails
            {
               TrustName = "Trust1",
               ChangesToTrust = "yes",
               ChangesToTrustExplained = "Changes explained",
               ChangesToLaGovernance = true,
               ChangesToLaGovernanceExplained = "Governance explained"
            },
            Schools = new List<School> { new() { SchoolName = "School1" } }
         };
      // Act
      Application application = PopulateOverview(academisationApplication, out School academisationApplicationSchool, out ApplyingSchool academiesApplicationSchool);
      // Assert
      Assert.Equal("A12345", application.ApplicationId);
      Assert.Equal(GlobalStrings.JoinAMat, application.ApplicationType);
      Assert.Equal("Trust1", application.TrustName);
      Assert.True(application.ChangesToTrust);
      Assert.Equal("Changes explained", application.ChangesToTrustExplained);
      Assert.Equal(true, application.ChangesToLaGovernance);
      Assert.Equal("Governance explained", application.ChangesToLaGovernanceExplained);
      Assert.Single(application.ApplyingSchools);
      Assert.Same(academisationApplicationSchool, academisationApplication.Schools.First());
      Assert.Same(academiesApplicationSchool, application.ApplyingSchools.First());
   }
}
