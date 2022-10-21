namespace ApplyToBecomeInternal.Tests.Extensions
{
	public static class QuerySelectorExtensions
	{
		public static string ToSelector(this string input, string attribute = "data-cy", string comparator = "=")
		{
			return $"[{attribute}{comparator}'{input}']";
		}
	}
}
