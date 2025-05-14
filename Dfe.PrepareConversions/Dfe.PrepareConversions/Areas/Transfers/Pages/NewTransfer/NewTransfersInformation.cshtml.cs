using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.Transfers
{
    public class NewTransfersInformation : TransfersPageModel
    {

        public IActionResult OnGet()
        {
            return Page();
        }


      public IActionResult OnPost()
      {
         HttpContext.Session.Remove(OutgoingTrustIdSessionKey);
         HttpContext.Session.Remove(IncomingTrustIdSessionKey);
         HttpContext.Session.Remove(OutgoingAcademyIdSessionKey);
         HttpContext.Session.Remove(ProposedTrustNameSessionKey);

         return RedirectToPage("/NewTransfer/TrustName");
      }
   }
}
