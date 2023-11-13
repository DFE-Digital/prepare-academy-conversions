using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.Trust;

public class TrustSummaryResponse
{
   public List<TrustSummary> Data { get; set; }
}

public class TrustSummary
{
   public string Ukprn { get; set; }
   public string Name { get; set; }
   public string CompaniesHouseNumber { get; set; }
   public string TrustType { get; set; }
   public TrustAddress Address { get; set; }  
}

public class TrustAddress
{
   public string Street { get; set; }
   public string Locality { get; set; }
   public string AdditionalLine { get; set; }
   public string Town { get; set; }
   public string County { get; set; }
   public string Postcode { get; set; }
}

