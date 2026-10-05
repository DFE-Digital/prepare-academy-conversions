using System;

namespace Dfe.PrepareConversions.Utils;

public static class SharePointLinkValidator
{
   public const string ErrorMessage = "Entry must be a valid https gov uk SharePoint link and include a file path";

   public static bool IsValid(string link)
   {
      if (string.IsNullOrWhiteSpace(link))
      {
         return true;
      }

      if (!Uri.TryCreate(link.Trim(), UriKind.Absolute, out Uri uri)
          || !uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
          || !uri.IsDefaultPort
          || !string.IsNullOrEmpty(uri.UserInfo))
      {
         return false;
      }

      bool validHost = uri.Host.Equals("educationgovuk.sharepoint.com", StringComparison.OrdinalIgnoreCase)
                       || uri.Host.Equals("educationgovuk-my.sharepoint.com", StringComparison.OrdinalIgnoreCase);

      return validHost && uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries).Length > 0;
   }
}