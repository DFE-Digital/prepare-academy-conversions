using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeOfstedInspectionCommand(
   DateTime? InspectionDate,
   bool? MeetsOfstedStandards,
   string AdditionalInformation,
   string SupportingEvidence);
