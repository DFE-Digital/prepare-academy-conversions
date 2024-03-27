namespace Dfe.PrepareConversions.Extensions;

using Dfe.Academisation.ExtensionMethods;

public static class StringExtensions
{
   public static string RouteDescription(this string @string, bool? isFormAMat)
   {
      const string converter = nameof(converter);
      const string sponsored = nameof(sponsored);

      if (string.IsNullOrWhiteSpace(@string))
      {
         return string.Empty;
      }
      var stringPrefix = isFormAMat.HasValue && isFormAMat.Value ? "Form a MAT " : string.Empty;

      return @string.SquishToLower() switch
      {
         sponsored => stringPrefix + "Sponsored conversion",
         converter => stringPrefix + "Voluntary conversion",
         _ => @string,
      };
   }

}
