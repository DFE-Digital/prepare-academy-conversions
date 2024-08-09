using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class ProjectGroup
{
   public int Id { get; set; }
   public string ReferenceNumber { get; set; }
   public string TrustReferenceNumber { get; set; }
   public string TrustName { get; set; }
   public string TrustUkprn { get; set; }
   public User AssignedUser { get; set; }
   public ICollection<AcademyConversionProject> Projects { get; set; } = new List<AcademyConversionProject>();
}

public record CreateProjectGroup(string trustReferenceNumber, string trustUkprn, string trustName, List<int> conversionProjectIds);

public record SetProjectGroup(List<int> conversionProjectIds);