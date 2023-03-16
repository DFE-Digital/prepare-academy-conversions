using System;

namespace Dfe.PrepareConversions.Extensions;

public static class DateTimeExtensions
{
   public static string ToUkDateString(this DateTime dateTime)
   {
      return dateTime.ToString("dd/MM/yyyy");
   }

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
      int month = (thisMonth.Month + monthsToAdd) % 12;
      if (month == 0) month = 12;
      int yearsToAdd = (thisMonth.Month + monthsToAdd - 1) / 12;
      return new DateTime(thisMonth.Year + yearsToAdd, month, 1);
   }
}
