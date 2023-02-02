using Dfe.PrepareConversions.Data.Models.Establishment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services
{
	public interface IGetEstablishment
	{
		Task<EstablishmentResponse> GetEstablishmentByUrn(string urn);
		Task<IEnumerable<EstablishmentResponse>> SearchEstablishments(string searchQuery);
	}
}