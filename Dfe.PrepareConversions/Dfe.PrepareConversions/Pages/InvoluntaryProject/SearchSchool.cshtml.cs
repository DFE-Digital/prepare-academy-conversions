using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
   public class SearchSchoolModel : PageModel
   {
      private readonly IGetEstablishment _getEstablishment;
      private readonly ErrorService _errorService;

      public SearchSchoolModel(IGetEstablishment getEstablishment, ErrorService errorService)
      {
         _getEstablishment = getEstablishment;
         _errorService = errorService;
      }

      [BindProperty] public string SearchQuery { get; set; } = "";

      public IActionResult OnGet()
      {
         return Page();
      }

      public async Task<IActionResult> OnGetSearch(string searchQuery)
      {
         if (searchQuery.Contains('('))
         {
            // if the school name contains the URN as well remove from the search
            var startIndex = searchQuery.IndexOf('(');
            searchQuery = searchQuery.Replace(searchQuery.Substring(startIndex), "");
          }

         var schools = await _getEstablishment.SearchEstablishments(searchQuery.Trim());

         return new JsonResult(schools.Select(s => new
         {
            suggestion = HighlightSearchMatch($"{s.Name} ({s.Urn})", searchQuery, s),
            value = $"{s.Name} ({s.Urn})"
         }));
      }

      public IActionResult OnPost()
      {
         if (string.IsNullOrWhiteSpace(SearchQuery))
         {
            ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN");
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
         }

         return Page();
      }

      private static string HighlightSearchMatch(string input, string toReplace, EstablishmentSearchResponse school)
      {
         if (school == null || string.IsNullOrWhiteSpace(school.Urn) || string.IsNullOrWhiteSpace(school.Name)) return string.Empty;

         var index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
         var correctCaseSearchString = input.Substring(index, toReplace.Length);

         return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
      }
   }
}