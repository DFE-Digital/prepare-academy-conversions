using Dfe.PrepareTransfers.Web.Pages.Transfers;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.NewTransfer
{
    public class TrustNameModel : TransfersPageModel
    {
        [BindProperty(Name = "query", SupportsGet = true)]
        public string SearchQuery { get; set; } = "";

        public IActionResult OnGet(bool change = false)
        {
            ViewData["ChangeLink"] = change;

            // We redirect here with any error messages from the subsequent
            // search step.
            if (TempData.Peek("ErrorMessage") != null)
            {
                ModelState.AddModelError(nameof(SearchQuery), (string)TempData["ErrorMessage"]);
            }

            return Page();
        }
    }
}
