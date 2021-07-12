using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static ApplyToBecomeInternal.Extensions.IntegerExtensions;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class SchoolPupilForecastsCurrentYearViewComponent : ViewComponent
	{
		private readonly IGetEstablishment _establishmentService;
		private readonly IAcademyConversionProjectRepository _repository;

		public SchoolPupilForecastsCurrentYearViewComponent(IGetEstablishment establishmentService, IAcademyConversionProjectRepository repository)
		{
			_establishmentService = establishmentService;
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
			var establishment = await _establishmentService.GetEstablishmentByUrn(project.Urn.ToString());

			var viewModel = new SchoolPupilForecastsCurrentYearViewModel
			{
				Id = project.Id.ToString(),
				CurrentYearCapacity = establishment.SchoolCapacity,
				CurrentYearPupilNumbers = establishment.Census?.NumberOfPupils,
				PercentageSchoolFull = ToInt(establishment.Census?.NumberOfPupils).AsPercentageOf(ToInt(establishment.SchoolCapacity))
			};

			return View(viewModel);
		}
	}
}
