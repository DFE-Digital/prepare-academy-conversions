using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum EqualitiesImpact
{
   [Description("Unlikely")]
   Unlikely,
   [Description("Some impact")]
   SomeImpact,
   [Description("Likely")]
   Likely
}
