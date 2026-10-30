namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeFundingCommand(
   FundingAnswer? fundingAnswer,
   string additionalInformation,
   string supportingEvidence)
{
   public FundingAnswer? FundingAnswer { get; set; } = fundingAnswer;
   public string AdditionalInformation { get; set; } = additionalInformation;
   public string SupportingEvidence { get; set; } = supportingEvidence;
}
