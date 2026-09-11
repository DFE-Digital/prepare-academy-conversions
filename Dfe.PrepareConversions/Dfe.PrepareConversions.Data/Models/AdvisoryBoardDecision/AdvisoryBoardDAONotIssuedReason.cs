using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

public enum AdvisoryBoardDAONotIssuedReason
{
   [Description("The school would not be viable as an academy")]
   SchoolWouldNotBeViableAsAnAcademy = 0,

   [Description("There are no suitable trust options")]
   ThereAreNoSuitableTrustOptions = 1,

   [Description("The school is already sufficiently advanced in the process of converting into an academy")]
   SchoolAlreadyConvertingAndSufficientlyAdvanced = 2,

   [Description("Other")]
   Other = 3
}