using System.Threading.Tasks;
using Dfe.PrepareConversions.Areas.Transfers.Services.Responses;

namespace Dfe.PrepareConversions.Areas.Transfers.Services.Interfaces
{
    public interface IGetInformationForProject
    {
        public Task<GetInformationForProjectResponse> Execute(string projectUrn);
    }
}