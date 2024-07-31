using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class ProjectGroup
{
   public int Id { get; set; }
   public string ReferenceNumber { get; set; }
   public User AssignedUser { get; set; }
   public ICollection<AcademyConversionProject> Projects { get; set; } = new List<AcademyConversionProject>();
}

public record CreateProjectGroup(string trustReferenceNumber,string trustUkprn, List<int> conversionProjectIds);