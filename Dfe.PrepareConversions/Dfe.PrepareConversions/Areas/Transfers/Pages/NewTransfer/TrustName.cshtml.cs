using Dfe.PrepareTransfers.Web.Pages.Transfers;
using Dfe.PrepareTransfers.Web.Validators.Transfers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

         public async Task<IActionResult> OnPostAsync(bool change = false)
         {
            var validator = new OutgoingTrustNameValidator();
            var validationResults = await validator.ValidateAsync(this);

            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
               return OnGet(change);
            }

            return change ? RedirectToPage("/NewTransfer/CheckYourAnswers") : RedirectToPage("/NewTransfer/TrustSearch", new { query = SearchQuery });
         }
   }
}
