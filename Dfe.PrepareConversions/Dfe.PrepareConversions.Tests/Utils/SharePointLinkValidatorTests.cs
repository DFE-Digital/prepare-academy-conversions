using Dfe.PrepareConversions.Utils;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SharePointLinkValidatorTests
{
   [Theory]
   [InlineData(null, true)]
   [InlineData("", true)]
   [InlineData("   ", true)]
   [InlineData("https://educationgovuk.sharepoint.com/sites/team/evidence", true)]
   [InlineData("https://educationgovuk-my.sharepoint.com/personal/user/evidence", true)]
   [InlineData("http://educationgovuk.sharepoint.com/sites/team/evidence", false)]
   [InlineData("https://other.sharepoint.com/sites/team/evidence", false)]
   [InlineData("https://educationgovuk.sharepoint.com.attacker.example/sites/team", false)]
   [InlineData("https://educationgovuk.sharepoint.com", false)]
   [InlineData("https://educationgovuk.sharepoint.com/", false)]
   [InlineData("https://educationgovuk.sharepoint.com:444/sites/team", false)]
   [InlineData("/sites/team/evidence", false)]
   public void IsValid_ReturnsExpectedResult(string link, bool expected)
   {
      Assert.Equal(expected, SharePointLinkValidator.IsValid(link));
   }
}