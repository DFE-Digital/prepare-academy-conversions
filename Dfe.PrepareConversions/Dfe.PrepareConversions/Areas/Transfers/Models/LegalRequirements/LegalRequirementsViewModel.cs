using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;

namespace Dfe.PrepareTransfers.Web.Models.LegalRequirements
{
    public class LegalRequirementsViewModel : CommonViewModel
    {
        public readonly ThreeOptions? IncomingTrustAgreement;
        public readonly ThreeOptions? DiocesanConsent;
        public readonly ThreeOptions? OutgoingTrustConsent;
        public readonly bool? InternalIsReadOnly;

        public LegalRequirementsViewModel(ThreeOptions? incomingTrustAgreement,
            ThreeOptions? diocesanConsent,
            ThreeOptions? outgoingTrustConsent,
            string projectUrn,
            bool? IsReadOnly)
        {
            IncomingTrustAgreement = incomingTrustAgreement;
            DiocesanConsent = diocesanConsent;
            OutgoingTrustConsent = outgoingTrustConsent;
            Urn = projectUrn;
            InternalIsReadOnly = IsReadOnly;

        }
    }
}