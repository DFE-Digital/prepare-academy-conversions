using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class FormAMatProject
{
   public int Id { get; set; }

   public string ProposedTrustName { get; set; }
   public string ApplicationReference { get; set; }
   public User AssignedUser { get; set; }
   public ICollection<AcademyConversionProject> Projects { get; set; } = new List<AcademyConversionProject>();

}
