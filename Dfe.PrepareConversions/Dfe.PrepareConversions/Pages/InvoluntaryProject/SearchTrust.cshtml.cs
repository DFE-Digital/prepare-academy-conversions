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

		public SearchTrustModel(ITrustsRespository trustsRepository, ErrorService errorService)
		{
			_trustsRepository = trustsRepository;
			_errorService = errorService;
		}

		[BindProperty] public string SearchQuery { get; set; } = "";
		public string Urn { get; set; }
		public string Ukprn { get; set; }

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

			return Page();
		}

		public async Task<IActionResult> OnGetSearch(string searchQuery)
		{
			var searchSplit = searchQuery.Split('(', ')');

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
			if (string.IsNullOrWhiteSpace(SearchQuery))
			{
				ModelState.AddModelError(nameof(SearchQuery), "Enter the trust, UKPRN or Companies House number");
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return Page();
			}

			var searchSplit = SearchQuery.Split('(', ')');
			if (searchSplit.Count() < 1) return Page();

			var ukprn = SearchQuery.Split('(', ')')[1];
			var trust = await _trustsRepository.SearchTrusts(ukprn);
			if (trust != null)
			{
				return RedirectToPage(Links.InvoluntaryProject.SearchTrusts.Page, new { ukprn, urn });
			}

			return Page();
		}

		private static string HighlightSearchMatch(string input, string toReplace, Trust trust)
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
	}
}
