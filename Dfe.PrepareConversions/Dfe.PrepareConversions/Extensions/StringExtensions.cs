namespace Dfe.PrepareConversions.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Dfe.Academisation.ExtensionMethods;

public static class StringExtensions
{
   public static string SplitPascalCase<T>(this T @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.SplitPascalCase(@string);

   /// <summary>
   ///    Converts a string to sentence case, ignoring acronyms.
   /// </summary>
   /// <param name="string">The string to convert.</param>
   /// <returns>A string</returns>
   public static string ToSentenceCase(this string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.ToSentenceCase(@string);

   public static bool IsAcronym(string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.IsAcronym(@string);


   /// <summary>
   ///    Checks a string to see if it contains exclusively capital letters
   /// </summary>
   /// <param name="string">The string to check.</param>
   /// <returns>A string</returns>
   public static bool IsAllCaps(string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.IsAllCaps(@string);

   /// <summary>
   ///    Extension method that converts "Yes" and "No" strings to bool values.
   ///    "Yes" is converted to true and "No" is converted to false.
   ///    The comparison is case-insensitive.
   ///    If the @string string does not match "Yes" or "No", an ArgumentException will be thrown.
   /// </summary>
   public static bool ToBool(this string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.ToBool(@string);

   public static string ToTitleCase(this string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.ToTitleCase(@string);

   public static bool IsEmpty(this string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.IsEmpty(@string);

   public static bool IsPresent(this string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.IsPresent(@string);

   private static string Squish(this string @string) => Dfe.Academisation.ExtensionMethods.StringExtensions.Squish(@string);

   public static string RouteDescription(this string @string)
   {
      const string converter = nameof(converter);
      const string sponsored = nameof(sponsored);
      const string formamat = nameof(formamat);

      if (string.IsNullOrWhiteSpace(@string))
      {
         return string.Empty;
      }

      return Dfe.Academisation.ExtensionMethods.StringExtensions.Squish(@string) switch
      {
         sponsored => "Sponsored conversion",
         converter => "Voluntary conversion",
         formamat => "Form a MAT",
         _ => @string,
      };
   }
}
