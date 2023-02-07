using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

			var trusts = await _trustsRepository.SearchTrusts(searchQuery.Trim());

			return new JsonResult(trusts.Select(t =>
			{
				var suggestion = $@"{t.GroupName} ({t.Urn})
									<br />
									Companies House number: {t.CompaniesHouseNumber}";
				return new
				{
					suggestion = HighlightSearchMatch(suggestion, searchQuery, t),
					value = $"{t.GroupName} ({t.Urn})"
				};
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

		private static string HighlightSearchMatch(string input, string toReplace, Trust school)
		{
			//if (school == null || string.IsNullOrWhiteSpace(school.Urn) || string.IsNullOrWhiteSpace(school.Name)) return string.Empty;

			var index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
			var correctCaseSearchString = input.Substring(index, toReplace.Length);

			return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
