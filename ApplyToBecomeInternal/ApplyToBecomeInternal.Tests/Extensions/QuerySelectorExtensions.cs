namespace ApplyToBecomeInternal.Tests.Extensions
{
	public static class QuerySelectorExtensions
	{
		public static string CypressSelector(this string input, string comparator = "=")
		{
			return $"[data-cy{comparator}'{input}']";
		}
	}
}
