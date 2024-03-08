using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;

public class UpdateTrustModel : BaseAcademyConversionProjectPageModel
{
   private const string SEARCH_LABEL =
      "Enter the name, UKPRN (UK Provider Reference Number) or Companies House number.";

   private const string SEARCH_ENDPOINT = "/start-new-project/trust-name?handler=Search&searchQuery=";
   private readonly ErrorService _errorService;
   private readonly ITrustsRepository _trustsRepository;

   public UpdateTrustModel(ITrustsRepository trustsRepository, IAcademyConversionProjectRepository academyConversionProjectRepository, ErrorService errorService)
      :base(academyConversionProjectRepository)
   {
      _trustsRepository = trustsRepository;
      _errorService = errorService;
      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, string.Empty, SEARCH_ENDPOINT);
   }

   [BindProperty]
   public string SearchQuery { get; set; } = "";
   public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      
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

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);
      if (string.IsNullOrWhiteSpace(SearchQuery))
      {
         ModelState.AddModelError(nameof(SearchQuery), "Enter the Trust name, UKPRN or Companies House number");
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

      string ukprn = searchSplit[searchSplit.Length - 1];

      TrustDtoResponse trusts = await _trustsRepository.SearchTrusts(ukprn);

      if (trusts.Data.Count != 1)
      {
         ModelState.AddModelError(nameof(SearchQuery), "We could not find a trust matching your search criteria");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      if (ModelState.IsValid)
      {
         try
         {
            var trust = trusts.Data.First();

            await _repository.SetIncomingTrust(id, new SetIncomingTrustDataModel(id,
                                                                              trust.ReferenceNumber,
                                                                              trust.Name));

            (string returnPage, string fragment) = GetReturnPageAndFragment();

            if (!string.IsNullOrWhiteSpace(returnPage))
            {
               return RedirectToPage(returnPage, null, new { id }, fragment);
            }

            return RedirectToPage(SuccessPage, new { id });

         }
         catch (ApiResponseException ex)
         {

            _errorService.AddApiError();
            return Page();
         }

      }
      return Page();
   }

   private static string HighlightSearchMatch(string input, string toReplace, TrustDto trust)
   {
      if (trust == null ||
          string.IsNullOrWhiteSpace(trust.Name))
         return string.Empty;

      int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
      string correctCaseSearchString = input.Substring(index, toReplace.Length);

      return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
   }

   private static string[] SplitOnBrackets(string input)
   {
      // return array containing one empty string if input string is null or empty
      if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

      return input.Split(new[] { '(', ')' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
   }

   private (string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      return (returnQuery, fragmentQuery);
   }
   public string SuccessPage
   {
      get => TempData["SuccessPage"].ToString();
      set => TempData["SuccessPage"] = value;
   }

}
