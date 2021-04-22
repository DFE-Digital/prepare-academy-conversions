using System;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DateTimeExtensions
	{
		public static string ToUkDateString(this DateTime dateTime) => dateTime.ToString("dd/MM/yyyy");
	}
}