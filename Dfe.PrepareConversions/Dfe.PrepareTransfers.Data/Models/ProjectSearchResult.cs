using System.Collections.Generic;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareTransfers.Data.Models.Projects;

namespace Dfe.PrepareTransfers.Data.Models
{
    public class ProjectSearchResult
    {
        public string Reference { get; set; }
        public string OutgoingTrustUkprn { get; set; }
        public string OutgoingTrustName { get; set; }
        public string OutgoingTrustNameInTitleCase => OutgoingTrustName.ToTitleCase();
        public string Status { get; set; }
        public List<TransferringAcademy> TransferringAcademies { get; set; }
        public User AssignedUser { get; set; }
        public string Urn { get; set; }

        public bool? IsFormAMat { get; set; }
    }
}