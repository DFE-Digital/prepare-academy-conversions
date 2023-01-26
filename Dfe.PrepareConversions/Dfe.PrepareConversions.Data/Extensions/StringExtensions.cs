namespace Dfe.PrepareConversions.Data.Extensions
{
	public static class StringExtensions
	{
		public static string ToFirstUpper(this string input)
		{
			string lowered = input.ToLower();
			return $"{char.ToUpper(lowered[0])}{lowered[1..]}";
		}
   }
}
