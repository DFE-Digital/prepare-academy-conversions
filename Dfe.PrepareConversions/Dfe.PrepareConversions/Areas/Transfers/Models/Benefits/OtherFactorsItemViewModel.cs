using Dfe.PrepareTransfers.Data.Models.Projects;

namespace Dfe.PrepareConversions.Areas.Transfers.Models.Benefits
{
    public class OtherFactorsItemViewModel
    {
        public TransferBenefits.OtherFactor OtherFactor { get; set; }
        public bool Checked { get; set; }
        public string Description { get; set; }
    }
}