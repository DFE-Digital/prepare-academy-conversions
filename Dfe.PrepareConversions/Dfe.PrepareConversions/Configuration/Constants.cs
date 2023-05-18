using System.Collections.Generic;

namespace Dfe.PrepareConversions.Configuration;

public static class Constants
{
   public const string DateTimeFormat = "dd MMMM yyyy";
   public static readonly HashSet<string> Acronyms = new()
   {
      "DAO",
      "MAT"
   };
}
