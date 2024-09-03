using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Transfers.Services.Responses;

namespace Dfe.PrepareTransfers.Web.Transfers.Services.Interfaces
{
    public interface IGetInformationForProject
    {
        public Task<GetInformationForProjectResponse> Execute(string projectUrn);
    }
}