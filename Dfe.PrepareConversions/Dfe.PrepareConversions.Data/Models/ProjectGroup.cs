using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class ProjectGroup
{
   
      public string TrustReferenceNumber { get; set; }

      public string Urn { get; set; }

      public IEnumerable<ConversionsModel> Conversions { get; set; }
}

public class ConversionsModel(int urn, string? schoolName)
{
   public int Urn { get; private set; } = urn;

   public string? SchoolName { get; private set; } = schoolName;
}

public record CreateProjectGroup(string trustReferenceNumber,string trustUkprn, List<int> conversionProjectIds);