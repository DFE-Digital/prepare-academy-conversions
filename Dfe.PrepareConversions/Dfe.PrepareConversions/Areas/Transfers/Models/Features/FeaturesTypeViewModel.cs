using Dfe.PrepareTransfers.Data.Models.Projects;

namespace Dfe.PrepareConversions.Areas.Transfers.Models.Features
{
    public class FeaturesTypeViewModel
    {
        public TransferFeatures.TransferTypes TypeOfTransfer { get; set; }
        public string OtherType { get; set; }
    }
}