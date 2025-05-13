using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Pages.Transfers;
using Dfe.PrepareTransfers.Web.Validators.Transfers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.NewTransfer
{
    public class TrustNameModel : TransfersPageModel
    {
         protected readonly ITrusts TrustsRepository;

         public TrustNameModel(ITrusts trustsRepository)
         {
            TrustsRepository = trustsRepository;
         }

         [BindProperty(Name = "query", SupportsGet = true)]
        public string SearchQuery { get; set; } = "";

        public IActionResult OnGet(bool change = false)
        {
            ViewData["ChangeLink"] = change;

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

            var result = await TrustsRepository.SearchTrusts(SearchQuery);
            if (result.Count == 0)
            {
               ModelState.AddModelError(nameof(SearchQuery), "We could not find any trusts matching your search criteria");
            }

            if (!ModelState.IsValid)
            {
               return OnGet(change);
            }

            return RedirectToPage("/NewTransfer/TrustSearch", new { query = SearchQuery, change });
         }
   }
}
