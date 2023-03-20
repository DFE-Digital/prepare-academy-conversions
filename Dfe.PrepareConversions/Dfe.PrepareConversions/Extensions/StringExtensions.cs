using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dfe.PrepareConversions.Extensions;

public static class StringExtensions
{
   public static string SplitPascalCase<T>(this T source)
   {
      return source == null ? string.Empty : Regex.Replace(source.ToString() ?? string.Empty, "[A-Z]", " $0", RegexOptions.None, TimeSpan.FromSeconds(1)).Trim();
   }

   /// <summary>
   ///    Converts a string to sentence case.
   /// </summary>
   /// <param name="input">The string to convert.</param>
   /// <returns>A string</returns>
   public static string SentenceCase(this string input)
   {
      if (input.Length < 2)
         return input.ToUpper();

      string sentence = input.ToLower();
      return char.ToUpper(sentence[0]) + sentence[1..];
   }

   public static string ToTitleCase(this string str)
   {
      TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
      return textInfo.ToTitleCase(str);
   }

   public static bool IsEmpty(this string input)
   {
      return string.IsNullOrWhiteSpace(input);
   }

   public static bool IsPresent(this string input)
   {
      return input.IsEmpty() is false;
   }

   private static string Squish(this string input)
   {
      return input.Replace(" ", "").ToLowerInvariant();
   }

   public static string RouteDescription(this string input)
   {
      const string converter = nameof(converter);
      const string sponsored = nameof(sponsored);
      const string formamat = nameof(formamat);

      if (string.IsNullOrWhiteSpace(input)) return string.Empty;

      return input.Squish() switch
      {
         sponsored => "Involuntary conversion",
         converter => "Voluntary conversion",
         formamat => "Form a MAT",
         _ => input
      };
   }
}
