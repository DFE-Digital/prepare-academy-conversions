namespace Dfe.PrepareConversions.Extensions;

using Dfe.Academisation.ExtensionMethods;

public static class StringExtensions
{
   public static string RouteDescription(this string @string)
   {
      const string converter = nameof(converter);
      const string sponsored = nameof(sponsored);
      const string formamat = nameof(formamat);

      if (string.IsNullOrWhiteSpace(@string))
      {
         return string.Empty;
      }

      return @string.SquishToLower() switch
      {
         sponsored => "Sponsored conversion",
         converter => "Voluntary conversion",
         formamat => "Form a MAT",
         _ => @string,
      };
   }
}
