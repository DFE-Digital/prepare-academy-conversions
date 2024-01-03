using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class AcademyConversionSearchModelV2
{
   public int Page { get; set; }
   public int Count { get; set; }
   public string TitleFilter { get; set; }
   public IEnumerable<string> DeliveryOfficerQueryString { get; set; }
   public IEnumerable<string> RegionQueryString { get; set; }
   public IEnumerable<string> StatusQueryString { get; set; }
   public IEnumerable<string> LocalAuthoritiesQueryString { get; set; }
   public IEnumerable<string> AdvisoryBoardDatesQueryString { get; set; }
}
