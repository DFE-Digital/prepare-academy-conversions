using Dfe.PrepareConversions.Data.Models.UserRole;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text.Json;

namespace Dfe.PrepareConversions.Extensions;

public static class SessionExtensions
{
   public static void Set<T>(this ISession session, string key, T value)
   {
      session.SetString(key, JsonSerializer.Serialize(value));
   }

   public static T Get<T>(this ISession session, string key)
   {
      string value = session.GetString(key);
      return value == null ? default : JsonSerializer.Deserialize<T>(value);
   }

   public static bool HasPermission(this ISession session, string key, RoleCapability roleCapability)
   {
      var sessionData = session.Get<string>(key) ?? string.Empty;
      return !sessionData.IsNullOrEmpty() && sessionData.Split(",").Any(x =>
      {
         return x.Contains(roleCapability.ToString(), StringComparison.OrdinalIgnoreCase);
      });
   }
}
