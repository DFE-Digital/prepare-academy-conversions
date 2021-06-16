using System;
using System.Globalization;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DecimalExtensions
	{
		public static string ToMoneyString(this decimal value, bool includePoundSign = false)
		{
			return string.Format(CultureInfo.CreateSpecificCulture("en-GB"), includePoundSign ? "{0:C2}" : "{0:F2}", value);
		}

		public static string ToPercentage(this decimal? value)
		{
			if (!value.HasValue)
			{
				return "";
			}
			return string.Format("{0}%", value);
		}

		public static string ToSafeString<T>(this Nullable<T> value) where T : struct
		{
			if (!value.HasValue)
			{
				return "";
			}
			return value.Value.ToString();
		}
	}
}
