namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public sealed record SetSignificantChangeTrustExperienceAndGovernanceCommand(
   YesNoNotApplicableAnswer? Answer,
   string AdditionalInformation,
   string SupportingEvidenceSharepointLink);
