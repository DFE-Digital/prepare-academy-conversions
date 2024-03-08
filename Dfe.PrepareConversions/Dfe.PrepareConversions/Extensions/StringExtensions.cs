namespace Dfe.PrepareConversions.Extensions;

using Dfe.Academisation.ExtensionMethods;
using System.Text.RegularExpressions;

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
   public static string ToSentenceCase(this string str)
   {
      // Check if the string is null or empty to avoid unnecessary processing
      if (string.IsNullOrEmpty(str)) return str;

      // A regex to identify sentence-ending patterns, considering spaces after periods, exclamation marks, or question marks.
      // Adjust this regex if your definition of a sentence differs.
      string pattern = @"(?<=[\.!\?])\s+";
      string[] sentences = Regex.Split(str, pattern);

      for (int i = 0; i < sentences.Length; i++)
      {
         if (!string.IsNullOrWhiteSpace(sentences[i]))
         {
            // Convert the first character of each sentence to uppercase.
            // Trim sentences to remove leading whitespace which might be present after splitting.
            sentences[i] = sentences[i].TrimStart();
            sentences[i] = char.ToUpper(sentences[i][0]) + sentences[i].Substring(1).ToLower();
         }
      }

      // Reassemble the string. This assumes one space after each sentence. Adjust accordingly.
      return string.Join(" ", sentences);
   }

}
