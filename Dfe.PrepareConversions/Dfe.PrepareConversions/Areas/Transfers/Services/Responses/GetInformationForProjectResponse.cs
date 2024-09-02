using System.Collections.Generic;
using Dfe.PrepareTransfers.Data.Models;

namespace Dfe.PrepareConversions.Areas.Transfers.Services.Responses
{
    public class GetInformationForProjectResponse
    {
        public Project Project { get; set; }
        public List<Academy> OutgoingAcademies { get; set; }
    }
}