using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeOfstedInspectionResponse
{
   public DateTime? InspectionDate { get; set; }
   public bool? MeetsOfstedStandards { get; set; }
   public string AdditionalInformation { get; set; } = string.Empty;
   public string SupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
