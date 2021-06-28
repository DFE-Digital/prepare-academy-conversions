using ApplyToBecome.Data.Models.Establishment;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public interface IGetEstablishment
	{
		Task<EstablishmentResponse> GetEstablishmentByUrn(string urn);
	}
}