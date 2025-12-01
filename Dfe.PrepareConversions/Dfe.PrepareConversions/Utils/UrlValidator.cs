using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dfe.PrepareConversions.Utils
{
   /// <summary>
   /// Provides URL validation to prevent XSS and Open Redirect attacks.
   /// Uses ASP.NET Core's IUrlHelper.IsLocalUrl for proper local URL validation.
   /// Reference: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iurlhelper.islocalurl
   /// </summary>
   public static class UrlValidator
   {
      // Dangerous URL protocols that can execute JavaScript or cause other security issues
      private static readonly string[] DangerousProtocols = new[]
      {
            "javascript:",
            "data:",
            "vbscript:",
            "file:",
            "about:",
            "blob:"
        };

      /// <summary>
      /// Validates that a URL is safe for use in redirects and href attributes.
      /// Blocks dangerous protocols (javascript:, data:, etc.) and external URLs.
      /// </summary>
      public static bool IsValidReturnUrl(string url, IUrlHelper urlHelper)
      {
         if (string.IsNullOrWhiteSpace(url))
         {
            return false;
         }

         ArgumentNullException.ThrowIfNull(urlHelper);

         // Decode the URL to catch encoded attacks like javascript%3Aalert(1)
         var decodedUrl = System.Web.HttpUtility.UrlDecode(url);

         // Check for dangerous protocols first (case-insensitive)
         if (ContainsDangerousProtocol(decodedUrl))
         {
            return false;
         }

         // Use ASP.NET Core's built-in IsLocalUrl method
         // MS docs: "A URL is considered local if it does not have a host/authority part and it has an absolute path"
         return urlHelper.IsLocalUrl(url);
      }

      private static bool ContainsDangerousProtocol(string url)
      {
         if (string.IsNullOrWhiteSpace(url))
         {
            return false;
         }

         var lowerUrl = url.ToLowerInvariant().Trim();
         return DangerousProtocols.Any(protocol => lowerUrl.StartsWith(protocol));
      }

      public static string SanitizeReturnUrl(string url, string defaultUrl, IUrlHelper urlHelper)
      {
         return IsValidReturnUrl(url, urlHelper) ? url : defaultUrl;
      }
   }
}
