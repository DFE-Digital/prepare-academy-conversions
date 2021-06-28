using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class SchoolPerformanceViewComponent : ViewComponent
	{
		private readonly SchoolPerformanceService _schoolPerformanceService;
		private readonly IAcademyConversionProjectRepository _repository;

		public SchoolPerformanceViewComponent(SchoolPerformanceService schoolPerformanceService, IAcademyConversionProjectRepository repository)
		{
			_schoolPerformanceService = schoolPerformanceService;
			_repository = repository;
		}

		public async Task<IViewComponentResult> InvokeAsync(bool showAdditionalInformation)
		{
			var id = int.Parse(ViewContext.RouteData.Values["id"].ToString());

			var response = await _repository.GetProjectById(id);
			if (!response.Success)
			{
				throw new InvalidOperationException();
			}

			var project = response.Body;
			var schoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());

			var viewModel = new SchoolPerformanceViewModel
			{
				Id = project.Id.ToString(),
				OfstedLastInspection = schoolPerformance.OfstedLastInspection?.ToString("d MMMM yyyy"),
				PersonalDevelopment = schoolPerformance.PersonalDevelopment.DisplayOfstedRating(),
				BehaviourAndAttitudes = schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating(),
				EarlyYearsProvision = schoolPerformance.EarlyYearsProvision.DisplayOfstedRating(),
				EffectivenessOfLeadershipAndManagement = schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating(),
				OverallEffectiveness = schoolPerformance.OverallEffectiveness.DisplayOfstedRating(),
				QualityOfEducation = schoolPerformance.QualityOfEducation.DisplayOfstedRating(),
				ShowAdditionalInformation = showAdditionalInformation,
				AdditionalInformation = project.SchoolPerformanceAdditionalInformation
			};

			return View(viewModel);
		}
	}
}
