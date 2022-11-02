using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;

namespace ApplyToBecomeInternal.Utils
{
	public static class TypespaceExtensions
	{
		private static readonly Regex NotAlphaNumeric = new Regex("[^[a-z0-9-_]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		public static HtmlString ToData(this string name, string dataName)
		{
			return string.IsNullOrWhiteSpace(name)
				? HtmlString.Empty
				: new HtmlString($"data-{dataName}=\"{name}\"");
		}

		public static HtmlString Of(this string stub, string extension)
		{
			if (string.IsNullOrWhiteSpace(stub)) return HtmlString.Empty;

			return string.IsNullOrWhiteSpace(extension)
				? new HtmlString(stub)
				: new HtmlString($"{stub}-{extension.Stub()}");
		}

		public static HtmlString Stub(this string input)
		{
			return new HtmlString(NotAlphaNumeric.Replace(input.ToLowerInvariant(), "-"));
		}
	}
}
