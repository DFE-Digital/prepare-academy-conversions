using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
   public class SchoolResultsModel : PageModel
   {
      public IEnumerable<EstablishmentSearchResponse> Schools { get; private set; }

      [BindProperty(Name = "query", SupportsGet = true)]
      public string SearchQuery { get; set; } = "";

      private readonly IGetEstablishment _getEstablishment;

      public SchoolResultsModel(IGetEstablishment getEstablishment)
      {
         _getEstablishment = getEstablishment;
      }

      public async Task<IActionResult> OnGetAsync()
      {
         Schools = await _getEstablishment.SearchEstablishments(SearchQuery);

         return Page();
      }
   }
}
