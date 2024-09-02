using Dfe.PrepareTransfers.Data.Models;

namespace Dfe.PrepareConversions.Areas.Transfers.Services.Interfaces
{
    public interface IReferenceNumberService
    {
        public string GenerateReferenceNumber(Project project);
    }
}