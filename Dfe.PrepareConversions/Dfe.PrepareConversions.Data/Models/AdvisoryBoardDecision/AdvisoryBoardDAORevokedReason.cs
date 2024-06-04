using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

public enum AdvisoryBoardDAORevokedReason
{
   [Description("School closed or closing")]
   SchoolClosedOrClosing = 0,
   [Description("School rated good or outstanding")]
   SchoolRatedGoodOrOutstanding = 1,
   [Description("Safeguarding concerns addressed")]
   SafeguardingConcernsAddressed = 2,
}