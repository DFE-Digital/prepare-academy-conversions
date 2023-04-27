using System.Globalization;

namespace Dfe.PrepareConversions.Extensions;

public static class DecimalExtensions
{
   public static string ToMoneyString(this decimal value, bool includePoundSign = false)
   {
      return string.Format(CultureInfo.CreateSpecificCulture("en-GB"), includePoundSign ? "{0:C2}" : "{0:F2}", value);
   }

   public static string ToMoneyString(this decimal? value, bool includePoundSign = false)
   {
      string format = includePoundSign ? "{0:C2}" : "{0:F2}";
      return value.HasValue ? string.Format(CultureInfo.CreateSpecificCulture("en-GB"), format, value) : string.Empty;
   }

   public static string ToPercentage(this decimal value)
   {
      return $"{value:G0}%";
   }

   public static string ToSafeString(this decimal? value)
   {
      return value.HasValue ? value.Value.ToString("G0") : string.Empty;
   }
}
