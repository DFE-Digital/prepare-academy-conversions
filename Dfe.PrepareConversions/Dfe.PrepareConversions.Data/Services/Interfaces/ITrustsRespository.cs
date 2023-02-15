using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Trust;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
	public interface ITrustsRespository
	{
		Task<TrustSummaryResponse> SearchTrusts(string searchQuery);

		Task<TrustDetail> GetTrustByUkprn(string ukprn);
	}
}
