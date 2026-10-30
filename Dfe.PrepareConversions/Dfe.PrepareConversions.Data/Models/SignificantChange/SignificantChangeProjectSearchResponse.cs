using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeProjectSearchResponse
{
   public int Id { get; set; }
   public int Urn { get; set; }
   public string SchoolName { get; set; }
   public byte Tier { get; set; }
   public required string TrustName { get; set; }
   public required string TrustUkprn { get; set; }
   public User AssignedUser { get; set; }
   public required string TypeOfSignificantChange { get; set; }
   public required string ApplicationId { get; set; }
   public required string ApplicationReference { get; set; }
   public required string Status { get; set; }
   public required string LocalAuthorityName { get; set; }
   public string RegionName { get; set; } = string.Empty;
}
