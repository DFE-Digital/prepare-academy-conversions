using System.ComponentModel;
using System.Reflection;

namespace Dfe.PrepareConversions.Extensions;

public static class EnumExtensions
{
   public static string ToDescription<T>(this T source)
   {
      if (source == null) return string.Empty;

      string description = source.ToString();

      if (string.IsNullOrWhiteSpace(description)) return string.Empty;

      FieldInfo fi = source.GetType().GetField(description);

      DescriptionAttribute[] attributes = (DescriptionAttribute[])fi?.GetCustomAttributes(
         typeof(DescriptionAttribute), false);

      return attributes is { Length: > 0 }
         ? attributes[0].Description
         : description;
   }
}
