using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
	public class SearchTrustModel : PageModel
	{
		private readonly ITrustsRespository _trustsRepository;
		private readonly ErrorService _errorService;
		private const string SEARCH_LABEL = "Search by name, UKPRN or Companies House number. Entering more characters will give quicker results. You should write UKPRN or Companies House number in full.";
		private const string SEARCH_ENDPOINT = "/start-new-project/trust-name?handler=Search&searchQuery=";


		public SearchTrustModel(ITrustsRespository trustsRepository, ErrorService errorService)
		{
			_trustsRepository = trustsRepository;
			_errorService = errorService;
			AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, string.Empty, SEARCH_ENDPOINT);
		}

		[BindProperty] public string SearchQuery { get; set; } = "";
		public string Urn { get; set; }
		public string Ukprn { get; set; }
		public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

		public async Task<IActionResult> OnGet(string ukprn, string urn)
		{
			Urn = urn;
			Ukprn = ukprn;

			if (string.IsNullOrWhiteSpace(ukprn)) return Page();

			var trusts = await _trustsRepository.SearchTrusts(ukprn);
			if (trusts.Data.Any())
			{
				var trust = trusts.Data.First();
				SearchQuery = $"{trust.GroupName} ({trust.Ukprn})";
			}

			AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

			return Page();
		}

		public async Task<IActionResult> OnGetSearch(string searchQuery)
		{
			var searchSplit = SplitOnBrackets(searchQuery);

			var trusts = await _trustsRepository.SearchTrusts(searchSplit[0].Trim());

			return new JsonResult(trusts.Data.Select(t =>
			{
				var suggestion = $@"{t.GroupName.ToTitleCase()} ({t.Ukprn})
									<br />
									Companies House number: {t.CompaniesHouseNumber}";
				return new
				{
					suggestion = HighlightSearchMatch(suggestion, searchSplit[0].Trim(), t),
					value = $"{t.GroupName.ToTitleCase()} ({t.Ukprn})"
				};
			}));
		}

		public async Task<IActionResult> OnPost(string urn)
		{
			AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);
			if (string.IsNullOrWhiteSpace(SearchQuery))
			{
				ModelState.AddModelError(nameof(SearchQuery), "Enter the Trust name, UKPRN or Companies House number");
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return Page();
			}

			var searchSplit = SplitOnBrackets(SearchQuery);
			if (searchSplit.Length < 2) return Page();

			var ukprn = searchSplit[1];

			var trust = await _trustsRepository.SearchTrusts(ukprn);
			if (trust != null) return RedirectToPage(Links.InvoluntaryProject.Summary.Page, new { ukprn, urn });

			return Page();
		}

		private static string HighlightSearchMatch(string input, string toReplace, TrustSummary trust)
		{
			if (trust == null ||
				string.IsNullOrWhiteSpace(trust.Ukprn) ||
				string.IsNullOrWhiteSpace(trust.GroupName) ||
				string.IsNullOrWhiteSpace(trust.CompaniesHouseNumber))
			{
				return string.Empty;
			}

			var index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
			var correctCaseSearchString = input.Substring(index, toReplace.Length);

			return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
		}

		private static string[] SplitOnBrackets(string input)
		{
			return input.Split(new[] { '(', ')' }, 3, StringSplitOptions.None);
		}
	}
}
