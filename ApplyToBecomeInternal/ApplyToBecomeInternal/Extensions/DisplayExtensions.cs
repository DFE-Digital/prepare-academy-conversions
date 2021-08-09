using ApplyToBecome.Data.Models.KeyStagePerformance;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DisplayExtensions
	{
		private const string NoData = "no data";
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

		public static string DisplayKeyStageDisadvantagedResult(DisadvantagedPupilsResponse disadvantagedPupilResponse)
		{
			if (string.IsNullOrEmpty(disadvantagedPupilResponse?.NotDisadvantaged) &&
			    string.IsNullOrEmpty(disadvantagedPupilResponse?.Disadvantaged))
				return NoData;

			return $"{GetFormattedResult(disadvantagedPupilResponse.NotDisadvantaged)}\n(disadvantaged {GetFormattedResult(disadvantagedPupilResponse.Disadvantaged)})";
		}

		public static string GetFormattedResult(string result)
		{
			return string.IsNullOrEmpty(result) ? NoData : result.ToDouble();
		}
	}
}
