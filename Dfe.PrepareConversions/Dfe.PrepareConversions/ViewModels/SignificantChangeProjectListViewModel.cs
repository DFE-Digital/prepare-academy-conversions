using Dfe.PrepareConversions.Data.Models;

namespace Dfe.PrepareConversions.ViewModels;

public class SignificantChangeProjectViewBaseModel
{
   public int Id { get; set; }
   public int Urn { get; set; }
   public required string SchoolName { get; set; }
   public byte Tier { get; set; }
   public required string TrustName { get; set; }
   public required string TrustUkprn { get; set; }
   public User AssignedUser { get; set; }
   public required string TypeOfSignificantChange { get; set; }
   public required string Status { get; set; }
   public required string StatusColour { get; set; }
}