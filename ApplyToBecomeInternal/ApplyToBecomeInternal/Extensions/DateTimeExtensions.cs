using System;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToUkDateString(this DateTime dateTime) => dateTime.ToString("dd/MM/yyyy");

		public static string ToDateString(this DateTime? dateTime, bool includeDayOfWeek = false)
		{
			if (!dateTime.HasValue)
			{
				return string.Empty;
			}
			return ToDateString(dateTime.Value, includeDayOfWeek);
		}

		public static string ToDateString(this DateTime dateTime, bool includeDayOfWeek = false)
		{
			if (includeDayOfWeek)
			{
				return dateTime.ToString("dddd d MMMM yyyy");
			}
			return dateTime.ToString("d MMMM yyyy");
		}

		public static DateTime FirstOfMonth(this DateTime thisMonth, int monthsToAdd)
		{
			var month = thisMonth.Month + monthsToAdd == 12 ? 12 : (thisMonth.Month + monthsToAdd) % 12;
			var yearsToAdd = (thisMonth.Month + monthsToAdd - 1) / 12;
			return new DateTime(thisMonth.Year + yearsToAdd, month, 1);
		}
	}
}