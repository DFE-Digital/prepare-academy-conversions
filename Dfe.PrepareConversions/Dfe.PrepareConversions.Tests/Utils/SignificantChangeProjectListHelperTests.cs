using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeProjectListHelperTests
{
   [Theory]
   [InlineData("approved", "Approved")]
   [InlineData("approved with conditions", "Approved with conditions")]
   [InlineData("DAO Revoked", "DAO revoked")]
   [InlineData("something unexpected", "Pre decision")]
   public void MapProjectStatus_Returns_expected_display_value(string inputStatus, string expectedStatus)
   {
      string result = SignificantChangeProjectListHelper.MapProjectStatus(inputStatus);

      Assert.Equal(expectedStatus, result);
   }

   [Fact]
   public void Build_Maps_status_using_shared_status_mapping()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "approved with conditions"
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Equal("Approved with conditions", viewModel.Status);
   }
}
