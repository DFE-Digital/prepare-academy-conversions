using System.Collections.Generic;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareTransfers.Data.TRAMS.Models.AcademyTransferProject;

namespace Dfe.PrepareTransfers.Data.TRAMS.Models
{
    public class TramsProjectSummary
    {
        public string ProjectUrn { get; set; }
        public string ProjectReference { get; set; }
        public string OutgoingTrustUkprn { get; set; }
        public string OutgoingTrustName { get; set; }
        public string Status { get; set; }
        public List<TransferringAcademy> TransferringAcademies { get; set; }
        public User AssignedUser { get; set; }
        public bool? IsFormAMat { get; set; }
    }
}