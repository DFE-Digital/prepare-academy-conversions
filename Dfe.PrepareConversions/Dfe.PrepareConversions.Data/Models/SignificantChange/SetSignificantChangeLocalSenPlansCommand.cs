namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public sealed record SetSignificantChangeLocalSenPlansCommand(
   YesNoNotApplicableAnswer? Answer,
   string AdditionalInformation,
   string SupportingEvidenceSharepointLink);
