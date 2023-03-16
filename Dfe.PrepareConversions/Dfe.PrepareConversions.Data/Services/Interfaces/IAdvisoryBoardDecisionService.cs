using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface IAcademyConversionAdvisoryBoardDecisionRepository
{
   Task Create(AdvisoryBoardDecision decision);
   Task Update(AdvisoryBoardDecision decision);
   Task<ApiResponse<AdvisoryBoardDecision>> Get(int id);
}
