using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ApplyToBecomeInternal.Extensions
{
	public static class StringExtensions
	{
		public static string SplitPascalCase<T>(this T source)
		{
			return source == null ? string.Empty : Regex.Replace(source.ToString(), "[A-Z]", " $0").Trim().ToString();
		}

		/// <summary>
		/// Converts a string to sentence case.
		/// </summary>
		/// <param name="input">The string to convert.</param>
		/// <returns>A string</returns>
		public static string SentenceCase(this string input)
		{
			if (input.Length < 1)
				return input;

			string sentence = input.ToLower();
			return sentence[0].ToString().ToUpper() +
			       sentence.Substring(1);
		}
	}
}
