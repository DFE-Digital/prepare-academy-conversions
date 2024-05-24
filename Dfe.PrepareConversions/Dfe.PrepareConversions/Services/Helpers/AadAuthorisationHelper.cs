using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Services.Helpers;

public interface IAadAuthorisationHelper
{
   Task<string> GetAccessToken();
}

public class AadAuthorisationHelper : IAadAuthorisationHelper
{
   private readonly IConfiguration _configuration;

   public AadAuthorisationHelper(IConfiguration configuration)
   {
      _configuration = configuration;
   }
   public async Task<string> GetAccessToken()
   {
      var app = ConfidentialClientApplicationBuilder
          .Create(_configuration["Sharepoint:ClientId"])
          .WithClientSecret(_configuration["Sharepoint:Secret"])
          .WithAuthority(AzureCloudInstance.AzurePublic, _configuration["Sharepoint:TenantId"])
          .Build();

      var result = app.AcquireTokenForClient(new string[] { $"{_configuration["Sharepoint:Authority"]}/.default" });

      return (await result.ExecuteAsync()).AccessToken;
   }
}
