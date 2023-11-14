namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication;

public class JoinTrustDtos
{
   public int UKPRN { get; set; }

   public string TrustName { get; set; }

   public string ChangesToTrust { get; set; }

   public string ChangesToTrustExplained { get; set; }

   public bool? ChangesToLaGovernance { get; set; }

   public string ChangesToLaGovernanceExplained { get; set; }
}
