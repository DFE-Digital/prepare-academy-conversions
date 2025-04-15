using System.ComponentModel;

namespace Dfe.PrepareConversions.Models
{
   public enum PublicSectorEqualityDutyImpact
   {
      [Description("Unlikely")]
      Unlikely = 1,
      [Description("Some impact")]
      SomeImpact = 2,
      [Description("Unlikely")]
      Likely = 3
   }
}
