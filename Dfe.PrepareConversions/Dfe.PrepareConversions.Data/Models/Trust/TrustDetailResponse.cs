using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.Trust;

public class TrustDetailResponse
{
   public List<TrustDetail> Data { get; set; }
}

public class TrustDetail
{
   public GiasData GiasData { get; set; }
}

public class GiasData
{
   public string GroupId { get; set; }
   public string GroupName { get; set; }
   public string GroupType { get; set; }
   public int CompaniesHouseNumber { get; set; }
   public string Ukprn { get; set; }
}
