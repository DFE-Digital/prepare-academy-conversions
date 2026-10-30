namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangePlanningPermissionCommand(
   PlanningPermissionAnswer? PlanningPermissionAnswer,
   string AdditionalInformation,
   string SupportingEvidence);