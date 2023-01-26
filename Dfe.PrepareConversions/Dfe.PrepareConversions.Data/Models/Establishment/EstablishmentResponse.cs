namespace Dfe.PrepareConversions.Data.Models.Establishment
{
	/// <remarks>
	/// Copy of TramsDataApi.ResponseModels.EstablishmentResponse with most properties omitted
	/// </remarks>
	public class EstablishmentResponse
	{
		public string Urn { get; set; }
		public string LocalAuthorityCode { get; set; }
		public string LocalAuthorityName { get; set; }
		public string EstablishmentNumber { get; set; }
		public string EstablishmentName { get; set; }
		public NameAndCodeResponse EstablishmentType { get; set; }
		public NameAndCodeResponse PhaseOfEducation { get; set; }
		public string StatutoryLowAge { get; set; }
		public string StatutoryHighAge { get; set; }
		public NameAndCodeResponse Diocese { get; set; }
		public string SchoolCapacity { get; set; }
		public CensusResponse Census { get; set; }
		public NameAndCodeResponse ParliamentaryConstituency { get; set; }
		public MisEstablishmentResponse MISEstablishment { get; set; }
		public AddressResponse Address { get; set; }
	}
}
