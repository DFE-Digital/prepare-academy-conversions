using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Validators.Transfers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Helpers;
using FluentValidation.AspNetCore;

namespace Dfe.PrepareTransfers.Web.Pages.Transfers
{
    public class TrustSearchModel : TransfersPageModel, ISetTrusts
    {
        public List<Trust> Trusts { get; private set; }

        [BindProperty(Name = "query", SupportsGet = true)]
        public string SearchQuery { get; set; } = "";

         [BindProperty(Name = "trustId", SupportsGet = true)]
         public string TrustId { get; set; }

         protected readonly ITrusts TrustsRepository;

        public TrustSearchModel(ITrusts trustsRepository)
        {
            TrustsRepository = trustsRepository;
        }

        public async Task<IActionResult> OnGetAsync(string trustId, bool change = false)
        {
            ViewData["ChangeLink"] = change;

            Trusts = await TrustsRepository.SearchTrusts(SearchQuery);
            TrustId = trustId;

            return Page();
         }

         public async Task<IActionResult> OnPostAsync(string query, string trustId, bool change = false)
         {
            Trusts = await TrustsRepository.SearchTrusts(query);
            TrustId = trustId;

            var searchValidator = new OutgoingTrustSearchValidator();
            var searchValidationResult = await searchValidator.ValidateAsync(this);

            searchValidationResult.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
               return await OnGetAsync(trustId, change);
            }

            var trust = Trusts.First(x => x.Ukprn == trustId);

            return RedirectToPage("/NewTransfer/OutgoingTrustDetails", new { query = SearchQuery, TrustId = trust.Ukprn, change });
         }
        void ISetTrusts.SetTrusts(IEnumerable<Trust> trusts)
        {
           Trusts = trusts.ToList();
        }
    }
}
