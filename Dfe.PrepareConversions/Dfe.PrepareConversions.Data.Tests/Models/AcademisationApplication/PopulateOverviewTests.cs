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
   public class PopulateOverviewTests
   {
      [Fact]
      public void PopulateOverview_PopulatesApplicationWithExpectedValues()
      {
         // Arrange
         var academisationApplication = new ApplyToBecome.Data.Models.AcademisationApplication.AcademisationApplication
         {
            ApplicationReference = "A12345",
            ApplicationType = "Type1",
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
         var application = PopulateOverview(academisationApplication, out School academisationApplicationSchool, out ApplyingSchool academiesApplicationSchool);
         // Assert
         Assert.Equal("A12345", application.ApplicationId);
         Assert.Equal("Type1", application.ApplicationType);
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
}
