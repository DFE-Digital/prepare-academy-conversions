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
	}
}
