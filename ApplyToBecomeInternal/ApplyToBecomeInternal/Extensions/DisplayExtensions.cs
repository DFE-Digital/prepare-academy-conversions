using ApplyToBecome.Data.Models.KeyStagePerformance;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Linq;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DisplayExtensions
	{
		private const string NoData = "No data";

		public static string DisplayOfstedRating(this string ofstedRating)
		{
			return ofstedRating switch
			{
				"1" => "Outstanding",
				"2" => "Good",
				"3" => "Requires improvement",
				"4" => "Inadequate",
				_ => NoData
			};
		}

		public static HtmlString HtmlFormatKeyStageDisadvantagedResult(DisadvantagedPupilsResponse disadvantagedPupilResponse)
		{
			if (string.IsNullOrEmpty(disadvantagedPupilResponse?.NotDisadvantaged) &&
			    string.IsNullOrEmpty(disadvantagedPupilResponse?.Disadvantaged))
				return new HtmlString(NoData);

			return new HtmlString(
				$"{disadvantagedPupilResponse.NotDisadvantaged.FormatValue()}<br>(disadvantaged pupils: {disadvantagedPupilResponse.Disadvantaged.FormatValue()})");
		}

		public static string GetErrorStyleClass(this ModelStateDictionary modelState)
		{
			return !modelState.IsValid ? "govuk-form-group--error" : "";
		}

		public static string FormatConfidenceInterval(decimal? lowerBound, decimal? upperBound)
		{
			if (lowerBound == null && upperBound == null) return NoData;
			return $"{lowerBound.FormatValue()} to {upperBound.FormatValue()}";
		}

		public static string FormatValue(this string value)
		{
			return string.IsNullOrEmpty(value) ? NoData : value.FormatAsDouble();
		}

		public static string FormatValue(this decimal? value)
		{
			return value == null ? NoData : value.ToSafeString();
		}

		public static string FormatAsDouble(this string value)
		{
			var isDouble = double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var resultAsDouble);
			return isDouble ? $"{resultAsDouble}" : value;
		}

		public static string FormatKeyStageYear(this string year)
		{
			if (string.IsNullOrEmpty(year)) return year;
			var trimmedYear = string.Concat(year.Where(c => !char.IsWhiteSpace(c)));
			return trimmedYear.Contains("-") ? trimmedYear.Replace("-", " to ") : year;
		}

		public static bool HasData(this string str)
		{
			return str != NoData;
		}
	}
}