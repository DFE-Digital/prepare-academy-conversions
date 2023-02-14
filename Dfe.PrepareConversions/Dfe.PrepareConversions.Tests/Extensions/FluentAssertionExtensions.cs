using FluentAssertions.Primitives;

namespace FluentAssertions
{
	public static class FluentAssertionExtensions
	{
		public static void BeUrl(this StringAssertions stringAssertions, string url)
		{
			stringAssertions.Be($"https://localhost{url}");
		}
	}
}
