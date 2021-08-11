using ApplyToBecome.Data.Models.KeyStagePerformance;
using System.Linq;

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

			return $"{disadvantagedPupilResponse.NotDisadvantaged.FormatValue()}\n(disadvantaged {disadvantagedPupilResponse.Disadvantaged.FormatValue()})";
		}

		public static string DisplayConfidenceInterval(decimal? lowerBound, decimal? upperBound)
		{
			if (lowerBound == null && upperBound == null) return NoData;
			return $"{lowerBound.FormatValue()} to {upperBound.FormatValue()}";
		}

		private static string FormatValue(this string value)
		{
			return string.IsNullOrEmpty(value) ? NoData : value.ToDouble();
		}

		public static string FormatValue(this decimal? value)
		{
			return value == null ? NoData : value.ToString();
		}

		public static string FormatKeyStageYear(this string year)
		{
			if (string.IsNullOrEmpty(year)) return year;
			var trimmedYear = string.Concat(year.Where(c => !char.IsWhiteSpace(c)));
			return trimmedYear.Contains("-") ? trimmedYear.Replace("-", " to ") : year;
		}
	}
}
