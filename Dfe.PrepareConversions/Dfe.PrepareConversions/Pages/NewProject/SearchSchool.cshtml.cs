using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstablishmentDto = Dfe.Academies.Contracts.V4.Establishments.EstablishmentDto;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class SearchSchoolModel : PageModel
{
   private const string SEARCH_LABEL = "Enter school name or URN (Unique Reference Number).";
   private const string SEARCH_ENDPOINT = "/start-new-project/school-name?handler=Search&searchQuery=";
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;

   public SearchSchoolModel(IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }

   [BindProperty]

   public string SearchQuery { get; set; } = "";

   public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

   public async Task<IActionResult> OnGet(string urn)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      if (!string.IsNullOrWhiteSpace(establishment.Urn))
      {
         SearchQuery = $"{establishment.Name} ({establishment.Urn})";
      }

      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

      return Page();
   }

   public async Task<IActionResult> OnGetSearch(string searchQuery)
   {
      string[] searchSplit = SplitOnBrackets(searchQuery);

      IEnumerable<EstablishmentSearchResponse> schools = await _getEstablishment.SearchEstablishments(searchSplit[0].Trim());

      return new JsonResult(schools.Select(s => new { suggestion = HighlightSearchMatch($"{s.Name} ({s.Urn})", searchSplit[0].Trim(), s), value = $"{s.Name} ({s.Urn})" }));
   }

   public async Task<IActionResult> OnPost(string ukprn, string redirect)
   {
      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

      if (string.IsNullOrWhiteSpace(SearchQuery))
      {
         ModelState.AddModelError(nameof(SearchQuery), "Enter a school name or URN to continue");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      string[] splitSearch = SplitOnBrackets(SearchQuery);
      if (splitSearch.Length < 2)
      {
         ModelState.AddModelError(nameof(SearchQuery), "We could not find any schools matching your search criteria");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      string expectedUkprn = splitSearch[1];

      var expectedEstablishment = await _getEstablishment.GetEstablishmentByUrn(expectedUkprn);

      if (expectedEstablishment.Name == null)
      {
         ModelState.AddModelError(nameof(SearchQuery), "We could not find a school matching your search criteria");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      redirect = string.IsNullOrEmpty(redirect) ? Links.NewProject.SchoolApply.Page : redirect;

      return RedirectToPage(redirect, new { urn = splitSearch[1], ukprn });
   }

   private static string HighlightSearchMatch(string input, string toReplace, EstablishmentSearchResponse school)
   {
      if (school == null || string.IsNullOrWhiteSpace(school.Urn) || string.IsNullOrWhiteSpace(school.Name)) return string.Empty;

      int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
      string correctCaseSearchString = input.Substring(index, toReplace.Length);

      return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
   }

   private static string[] SplitOnBrackets(string input)
   {
      return input.Split(new[] { '(', ')' }, 3, StringSplitOptions.None);
   }
}
