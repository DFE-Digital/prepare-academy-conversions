using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public enum ConsultationDurationAnswer
{
   [Description("Yes")]Yes = 1,
   [Description("No - satisfactory consultation carried out")] NoSatisfactoryConsultationCarriedOut = 2,
   [Description("No")] No = 3
}