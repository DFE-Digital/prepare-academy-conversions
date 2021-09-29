using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ApplyToBecomeInternal.Extensions.DisplayExtensions;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class KeyStage2PerformanceTablesViewComponent : ViewComponent
	{
		private readonly KeyStagePerformanceService _keyStagePerformanceService;
		private readonly IAcademyConversionProjectRepository _repository;

		public KeyStage2PerformanceTablesViewComponent(
			KeyStagePerformanceService keyStagePerformanceService,
			IAcademyConversionProjectRepository repository)
		{
			_keyStagePerformanceService = keyStagePerformanceService;
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
			ViewData["SchoolName"] = project.SchoolName;
			ViewData["LocalAuthority"] = project.LocalAuthority;
			var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());

			var viewModel = keyStagePerformance.KeyStage2.Select(KeyStage2PerformanceTableViewModel.Build).OrderByDescending(ks => ks.Year);

			return View(viewModel);
		}
	}
}
