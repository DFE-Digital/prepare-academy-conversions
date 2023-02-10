using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
	public class SummaryModel : PageModel
	{
		private readonly IGetEstablishment _getEstablishment;
		private readonly ITrustsRespository _trustRepository;

		public SummaryModel(IGetEstablishment getEstablishment, ITrustsRespository trustRepository)
		{
			_getEstablishment = getEstablishment;
			_trustRepository = trustRepository;
		}

		public EstablishmentResponse Establishment { get; set; }
		public Trust Trust { get; set; }


		public async Task OnGetAsync(string urn, string ukprn)
		{
			Establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
			Trust = (await _trustRepository.SearchTrusts(ukprn)).Data.First();
		}

		public async Task OnPostAsync(string urn, string ukprn)
		{

		}
	}
}
