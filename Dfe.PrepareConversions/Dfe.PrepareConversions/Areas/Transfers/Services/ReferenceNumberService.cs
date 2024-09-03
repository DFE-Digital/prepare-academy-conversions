using Dfe.PrepareTransfers.Web.Transfers.Services.Interfaces;
using System;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Transfers.Services.Interfaces;

namespace Dfe.PrepareTransfers.Web.Transfers.Services
{
    public class ReferenceNumberService : IReferenceNumberService
    {
       public string GenerateReferenceNumber(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            string referenceNumber = "SAT";
            if (project.TransferringAcademies.Count > 1)
            {
                referenceNumber = "MAT";
            }

            return $"{referenceNumber}-{project.Urn}";
        }
    }
}