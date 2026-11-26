namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public sealed record SetSignificantChangeImpactOnSenProvisionCommand(
   YesNoNotApplicableAnswer? Answer,
   string AdditionalInformation,
   string SupportingEvidenceSharepointLink);
