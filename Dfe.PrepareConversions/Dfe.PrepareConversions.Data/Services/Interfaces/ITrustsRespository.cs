using Dfe.PrepareConversions.Data.Models;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
	public interface ITrustsRespository
	{
		Task<TrustsResponse> SearchTrusts(string searchQuery);
	}
}
