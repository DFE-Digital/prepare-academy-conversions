using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class SearchTrustModel : PageModel
{
   private const string SEARCH_LABEL =
      "Enter the name, UKPRN (UK Provider Reference Number) or Companies House number.";

   private const string SEARCH_ENDPOINT = "/start-new-project/trust-name?handler=Search&searchQuery=";
   private readonly ErrorService _errorService;
   private readonly ITrustsRepository _trustsRepository;

   public SearchTrustModel(ITrustsRepository trustsRepository, ErrorService errorService)
   {
      _trustsRepository = trustsRepository;
      _errorService = errorService;
      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, string.Empty, SEARCH_ENDPOINT);
   }

   [BindProperty]
   public string SearchQuery { get; set; } = "";
   [BindProperty]
   public string HasSchoolApplied { get; set; }

   public string Urn { get; set; }
   public string Ukprn { get; set; }
   public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

   public async Task<IActionResult> OnGet(string ukprn, string urn, string hasSchoolApplied)
   {
      Urn = urn;
      Ukprn = ukprn;
      HasSchoolApplied = string.IsNullOrEmpty(hasSchoolApplied) ? "yes" : hasSchoolApplied;

      if (string.IsNullOrWhiteSpace(ukprn)) return Page();

      TrustDtoResponse trusts = await _trustsRepository.SearchTrusts(ukprn);
      if (trusts.Data.Any())
      {
         TrustDto trust = trusts.Data[0];
         SearchQuery = $"{trust.Name} ({trust.Ukprn})";
      }

      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

      return Page();
   }

   public async Task<IActionResult> OnGetSearch(string searchQuery)
   {
      string[] searchSplit = SplitOnBrackets(searchQuery);

      TrustDtoResponse trusts = await _trustsRepository.SearchTrusts(searchSplit[0].Trim());

      return new JsonResult(trusts.Data.Select(t =>
      {
         string displayUkprn = string.IsNullOrWhiteSpace(t.Ukprn) ? string.Empty : $"({t.Ukprn})";
         string suggestion = $@"{t.Name?.ToTitleCase() ?? ""} {displayUkprn}
									<br />
									Companies House number: {t.CompaniesHouseNumber ?? ""}";
         return new { suggestion = HighlightSearchMatch(suggestion, searchSplit[0].Trim(), t), value = $"{t.Name?.ToTitleCase() ?? ""} ({t.Ukprn})" };
      }));
   }

   public async Task<IActionResult> OnPost(string urn)
   {
      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);
      if (string.IsNullOrWhiteSpace(SearchQuery))
      {
         ModelState.AddModelError(nameof(SearchQuery), "Enter a trust's name, UKPRN or Companies House number to continue");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      string[] searchSplit = SplitOnBrackets(SearchQuery);
      if (searchSplit.Length < 2)
      {
         ModelState.AddModelError(nameof(SearchQuery), "We could not find any trusts matching your search criteria");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      string ukprn = searchSplit[1];

      TrustDtoResponse trusts = await _trustsRepository.SearchTrusts(ukprn);

      if (trusts.Data.Count != 1)
      {
         ModelState.AddModelError(nameof(SearchQuery), "We could not find a trust matching your search criteria");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      return RedirectToPage(Links.NewProject.Summary.Page, new { ukprn, urn, HasSchoolApplied });
   }

   private static string HighlightSearchMatch(string input, string toReplace, TrustDto trust)
   {
      if (trust == null ||
          string.IsNullOrWhiteSpace(trust.Name))
      {
         return string.Empty;
      }

      int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
      string correctCaseSearchString = input.Substring(index, toReplace.Length);

      return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
   }

   private static string[] SplitOnBrackets(string input)
   {
      return input.Split(new[] { '(', ')' }, 3, StringSplitOptions.None);
   }
}
