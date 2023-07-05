namespace Dfe.PrepareConversions.Extensions;

public static class IntegerExtensions
{
   public static int? ToInt(string value)
   {
      if (int.TryParse(value, out int result))
      {
         return result;
      }

      return null;
   }
}
