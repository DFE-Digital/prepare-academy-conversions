using Dfe.PrepareConversions.Data.Models.Establishment;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services
{
	public interface IGetEstablishment
	{
		Task<EstablishmentResponse> GetEstablishmentByUrn(string urn);
	}
}