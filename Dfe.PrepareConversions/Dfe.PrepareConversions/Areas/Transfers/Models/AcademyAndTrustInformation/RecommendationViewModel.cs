using Dfe.PrepareConversions.Areas.Transfers.Models.Forms;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Web.Models.Forms;

namespace Dfe.PrepareConversions.Areas.Transfers.Models.AcademyAndTrustInformation
{
    public class RecommendationViewModel : CommonViewModel
    {
        public TransferAcademyAndTrustInformation.RecommendationResult Recommendation { get; set; }
        public string Author { get; set; }
    }
}