namespace ApplyToBecomeInternal.Extensions
{
	public static class DisplayExtensions
	{
		public static string DisplayOfstedRating(this string ofstedRating)
		{
			return ofstedRating switch
			{
				"1" => "Outstanding",
				"2" => "Good",
				"3" => "Requires improvement",
				"4" => "Inadequate",
				_ => "No data"
			};
		}
	}
}
