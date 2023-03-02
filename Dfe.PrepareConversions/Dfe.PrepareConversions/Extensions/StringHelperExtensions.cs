namespace Dfe.PrepareConversions.Extensions;

public static class StringHelperExtensions
{
   public static bool IsEmpty(this string input)
   {
      return string.IsNullOrWhiteSpace(input);
   }

   public static bool IsPresent(this string input)
   {
      return input.IsEmpty() is false;
   }
}
