using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Extensions;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
	public class SearchTrustModel : PageModel
	{
		private readonly ITrustsRespository _trustsRepository;
		private readonly ISession _session;
		private readonly ErrorService _errorService;
		private const string INVOLUNTARY_PROJECT_TRUST_KEY = "InvoluntaryProjectTrust";

		public SearchTrustModel(ITrustsRespository trustsRepository, ISession session, ErrorService errorService)
		{
			_trustsRepository = trustsRepository;
			_session = session;
			_errorService = errorService;
		}

		[BindProperty] public string SearchQuery { get; set; } = "";

		public IActionResult OnGet()
		{
			var trust = _session.Get<Trust>(INVOLUNTARY_PROJECT_TRUST_KEY);
			if (trust != null)
			{
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

		public async Task<IActionResult> OnPost()
		{
			if (string.IsNullOrWhiteSpace(SearchQuery))
			{
				ModelState.AddModelError(nameof(SearchQuery), "Enter the trust, UKPRN or Companies House number");
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return Page();
			}

			var searchSplit = SearchQuery.Split('(', ')');
			if (searchSplit.Count() < 1) return Page();

			var trust = await _trustsRepository.SearchTrusts(SearchQuery.Split('(', ')')[1]);
			if (trust != null)
			{
				_session.Set(INVOLUNTARY_PROJECT_TRUST_KEY, trust.Data?.First());
				return Page();
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

			var trustName = input.Split('(', ')')[0].Trim();

			return input.Replace(toReplace, $"<strong>{trustName}</strong>", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
