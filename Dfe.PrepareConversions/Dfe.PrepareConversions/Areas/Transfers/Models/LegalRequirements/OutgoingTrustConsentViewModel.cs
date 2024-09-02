using Dfe.PrepareTransfers.Data.Models;

namespace Dfe.PrepareConversions.Areas.Transfers.Models.LegalRequirements
{
    public class OutgoingTrustConsentViewModel : CommonLegalViewModel
    {
        public ThreeOptions? OutgoingTrustConsent { get; set; }
    }
}
