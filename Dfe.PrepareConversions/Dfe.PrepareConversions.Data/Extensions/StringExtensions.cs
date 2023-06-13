namespace Dfe.PrepareConversions.Data.Extensions;
using Dfe.Academisation.ExtensionMethods;
public static class StringExtensions
{
   public static string ToFirstUpper(this string input)
   {
      // Intentionally just passing on the method call so that connectivity to the nuget server can be proven

      return Dfe.Academisation.ExtensionMethods.StringExtensions.ToFirstUpper(input);
   }
}
