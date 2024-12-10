using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.IO;

namespace Dfe.PrepareConversions.Utils
{
   internal static class DataProtectionService
   {
      public static void AddDataProtectionService(this IServiceCollection services, IConfiguration configuration)
      {
         var dp = services.AddDataProtection();
         var dpTargetPath = "@/srv/app/storage";

         if (Directory.Exists(dpTargetPath)) {
            dp.PersistKeysToFileSystem(new DirectoryInfo(dpTargetPath));

            // If a Key Vault Key URI is defined, expect to encrypt the keys.xml
            string kvProtectionKeyUri = configuration.GetValue<string>("DataProtection:KeyVaultKey");

            if (!string.IsNullOrWhiteSpace(kvProtectionKeyUri))
            {
               dp.ProtectKeysWithAzureKeyVault(new Uri(kvProtectionKeyUri), new DefaultAzureCredential());
            }
         }
      }
   }
}
