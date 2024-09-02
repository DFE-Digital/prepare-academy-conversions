using Dfe.PrepareConversions.Areas.Transfers.Services.Interfaces;
using System;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareConversions.Areas.Transfers.Services.Interfaces;

namespace Dfe.PrepareConversions.Areas.Transfers.Services
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