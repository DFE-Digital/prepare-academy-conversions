using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models
{
	public class Trust
	{
		public string Ukprn { get; set; }
		public string Urn { get; set; }
		public string GroupName { get; set; }
		public string CompaniesHouseNumber { get; set; }
		public string TrustType { get; set; }
		public TrustAddress TrustAddress { get; set; }
		public List<EstablishmentDetail> Establishments { get; set; }
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

	public class EstablishmentDetail
	{
		public string Urn { get; set; }
		public string Name { get; set; }
		public string Ukprn { get; set; }
	}

}
