using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class SchoolPostcodeViewComponent : ViewComponent
	{
		private readonly GeneralInformationService _generalInformationService;
		private readonly IAcademyConversionProjectRepository _repository;

		public SchoolPostcodeViewComponent(GeneralInformationService generalInformationService, IAcademyConversionProjectRepository repository)
		{
			_generalInformationService = generalInformationService;
			_repository = repository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var id = int.Parse(ViewContext.RouteData.Values["id"].ToString());

			var response = await _repository.GetProjectById(id);
			if (!response.Success)
			{
				throw new InvalidOperationException();
			}

			var project = response.Body;
			var generalInformation = await _generalInformationService.GetGeneralInformationByUrn(project.Urn.ToString());

			var viewModel = new SchoolPostcodeViewModel
			{
				SchoolPostcode = generalInformation.SchoolPostcode
			};

			return View(viewModel);
		}
	}
}
