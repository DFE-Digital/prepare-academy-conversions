using System;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToUkDateString(this DateTime dateTime) => dateTime.ToString("dd/MM/yyyy");

		public static string ToDateString(this DateTime? dateTime)
		{
			if (!dateTime.HasValue)
			{
				return string.Empty;
			}
			return dateTime.Value.ToString("dd MMM yyyy");
		}
	}
}