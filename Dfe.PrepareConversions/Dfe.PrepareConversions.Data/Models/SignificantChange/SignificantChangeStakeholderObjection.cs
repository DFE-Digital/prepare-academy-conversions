using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum SignificantChangeStakeholderObjection
{
   [Description("No")]
   No,
   [Description("Yes - all objections have been addressed")]
   YesAllObjectionsAddressed,
   [Description("Yes - no further information provided")]
   YesNoFurtherInformationProvided
}