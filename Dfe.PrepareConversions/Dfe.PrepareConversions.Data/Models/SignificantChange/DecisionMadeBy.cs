using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum DecisionMadeBy
{
   [Description("Regional Director")] RegionalDirector = 0,
   [Description("Minister")] Minister = 1,
   [Description("Director General")] DirectorGeneral = 2,
   [Description("Deputy Director")] DeputyDirector = 3,
   [Description("Grade 6")] Grade6 = 4,
}