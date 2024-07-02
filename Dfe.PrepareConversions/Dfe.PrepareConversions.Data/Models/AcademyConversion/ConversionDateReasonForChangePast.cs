using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.AcademyConversion;

public enum ConversionDateReasonForChangePast
{
   [Description("Conversion is progressing faster than expected")] ProgressingFaster = 1,
   [Description("Correcting an error")] CorrectingAnError = 2
}
