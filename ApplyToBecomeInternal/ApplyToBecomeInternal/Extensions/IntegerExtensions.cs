using System.Globalization;

namespace ApplyToBecomeInternal.Extensions
{
	public static class IntegerExtensions
	{
		public static string ToMoneyString(this int value, bool includePoundSign = false)
		{
			return string.Format(CultureInfo.CreateSpecificCulture("en-GB"), includePoundSign ? "{0:C0}" : "{0:F0}", value);
		}
		
		public static string AsPercentageOf(this int? part, int? whole)
		{
			if (!whole.HasValue || !part.HasValue)
			{
				return "";
			}
			return string.Format("{0:F0}%", (100d / whole) * part);
		}

		public static int? ToInt(string value)
		{
			if (int.TryParse(value, out var result))
			{
				return result;
			}
			return null;
		}
	}
}
