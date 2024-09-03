using Dfe.PrepareTransfers.Data.Models;

namespace Dfe.PrepareTransfers.Web.Transfers.Services.Interfaces
{
    public interface IReferenceNumberService
    {
        public string GenerateReferenceNumber(Project project);
    }
}