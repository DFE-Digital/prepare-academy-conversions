using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.InvoluntaryProject;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
	public class SummaryModel : PageModel
	{
		private readonly IGetEstablishment _getEstablishment;
		private readonly ITrustsRespository _trustRepository;
		private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;

		public SummaryModel(IGetEstablishment getEstablishment, ITrustsRespository trustRepository,
			 IAcademyConversionProjectRepository academyConversionProjectRepository)
		{
			_getEstablishment = getEstablishment;
			_trustRepository = trustRepository;
			_academyConversionProjectRepository = academyConversionProjectRepository;
		}

		public EstablishmentResponse Establishment { get; set; }
		public TrustSummary Trust { get; set; }


		public async Task<IActionResult> OnGetAsync(string urn, string ukprn)
		{
			Establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
			Trust = (await _trustRepository.SearchTrusts(ukprn)).Data.FirstOrDefault();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string urn, string ukprn)
		{
			var establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
			var trust = await _trustRepository.GetTrustByUkprn(ukprn);

			await _academyConversionProjectRepository.CreateInvoluntaryProject(MapToDto(establishment, trust));

			return RedirectToPage(Links.ProjectList.Index.Page);
		}

		private static CreateInvoluntaryProject MapToDto(EstablishmentResponse establishment, TrustDetail trust)
		{
			var createSchool = new InvoluntaryProjectSchool(
								establishment.EstablishmentName,
								establishment.Urn,
								string.IsNullOrEmpty(establishment?.OpenDate)
									? null
									: DateTime.Parse(establishment?.OpenDate),
								establishment.ViewAcademyConversion?.Pfi != null && establishment.ViewAcademyConversion?.Pfi != "No");

			var createTrust = new InvoluntaryProjectTrust(
					   trust.GiasData.GroupName,
					   trust.GiasData.GroupId);

			return new CreateInvoluntaryProject(createSchool, createTrust);
	  }
	}
}
