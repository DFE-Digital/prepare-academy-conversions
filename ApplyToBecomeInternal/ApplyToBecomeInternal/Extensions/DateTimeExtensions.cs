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
			if (includeDayOfWeek)
			{
				return dateTime.Value.ToString("dddd d MMMM yyyy");
			}
			return dateTime.Value.ToString("d MMMM yyyy");
		}
	}
}