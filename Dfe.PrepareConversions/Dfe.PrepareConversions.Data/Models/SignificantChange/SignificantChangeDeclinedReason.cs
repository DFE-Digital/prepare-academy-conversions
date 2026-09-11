
using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum SignificantChangeDeclinedReason
{
   [Description("Finance")] Finance = 0,
   [Description("Performance")] Performance = 1,
   [Description("Other")] Other = 2,
   [Description("Governance")] Governance = 3,
   [Description("Choice of trust")] ChoiceOfTrust = 4
}