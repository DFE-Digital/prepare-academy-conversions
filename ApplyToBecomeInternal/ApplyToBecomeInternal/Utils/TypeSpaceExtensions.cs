using Microsoft.AspNetCore.Html;

namespace ApplyToBecomeInternal.Utils
{
	public static class TypespaceExtensions
	{
		public static HtmlString ToData(this string name, string dataName)
		{
			return string.IsNullOrWhiteSpace(name)
				? HtmlString.Empty
				: new HtmlString($"data-{dataName}=\"{name}\"");
		}
	}
}
