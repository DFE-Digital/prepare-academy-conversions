using Dfe.PrepareConversions.Configuration;
using System;
using System.Globalization;
using System.Linq;
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
   public static string ToSentenceCase(this string input)
   {
      if (string.IsNullOrEmpty(input)) return input;

      var acronyms = Constants.Acronyms.Select(acronym => acronym.ToUpperInvariant())
         .ToHashSet();

      string[] words = input.Split(' ');

      bool firstNonAcronymCapitalized = false;

      for (int i = 0; i < words.Length; i++)
      {
         if (acronyms.Contains(words[i].ToUpperInvariant())) // It's an acronym
         {
            words[i] = words[i].ToUpperInvariant();
         }
         else // Not an acronym
         {
            words[i] = words[i].ToLowerInvariant();

            if (firstNonAcronymCapitalized is false)
            {
               words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1);
               firstNonAcronymCapitalized = true;
            }
         }
      }

      return string.Join(' ', words);
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
