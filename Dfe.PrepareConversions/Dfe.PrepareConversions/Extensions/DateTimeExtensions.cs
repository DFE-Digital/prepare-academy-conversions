using Dfe.PrepareConversions.Data.Models;
using System;
using System.Globalization;

namespace Dfe.PrepareConversions.Extensions;

public static class DateTimeExtensions
{
   public static DateTime ToUkDateTime(this DateTime dateTime)
   {
      // Assuming your datetime is in projectNote.DateTimePropertyName
      // First convert to UTC 
      var utcTime = dateTime.ToUniversalTime();

      // Then convert to UK time
      var ukTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "GMT Standard Time");

      // Replace the time in your object
      dateTime = ukTime;
      return dateTime;
   }

   public static string ToOrdinalDateString(this DateTime date)
   {
      int day = date.Day;
      string suffix = (day % 100) is >= 11 and <= 13
         ? "th"
         : (day % 10) switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" };
      return $"{day}{suffix} {date.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("en-GB"))}";
   }

   public static string ToOrdinalDateString(this DateTime? date) => 
      date.HasValue ? date.Value.ToOrdinalDateString() : string.Empty;
}
