using Dfe.PrepareConversions.Data.Models;
using System;

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
}
