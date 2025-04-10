using Dfe.PrepareConversions.Data.Models.Trust;
using DfE.CoreLibs.Contracts.Academies.V4.Trusts;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface ITrustsRepository
{
   Task<TrustDtoResponse> SearchTrusts(string searchQuery);

   Task<TrustDto> GetTrustByUkprn(string ukprn);
}
