using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services.Interfaces
{
	public interface IAcademyConversionAdvisoryBoardDecisionRepository
	{
		Task Create(AdvisoryBoardDecision decision);
		Task<ApiResponse<AdvisoryBoardDecision>> Get(int id);
	}
}