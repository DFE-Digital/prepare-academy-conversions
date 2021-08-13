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

			var viewModel = keyStagePerformance.KeyStage2.Select(Build).OrderByDescending(ks => ks.Year);

			return View(viewModel);
		}

		private KeyStage2PerformanceTableViewModel Build(KeyStage2PerformanceResponse keyStage2Performance)
		{
			return new KeyStage2PerformanceTableViewModel
			{
				Year = keyStage2Performance.Year.FormatKeyStageYear(),
				PercentageMeetingExpectedStdInRWM = keyStage2Performance.PercentageMeetingExpectedStdInRWM.NotDisadvantaged.FormatValue(),
				PercentageAchievingHigherStdInRWM = keyStage2Performance.PercentageAchievingHigherStdInRWM.NotDisadvantaged.FormatValue(),
				ReadingProgressScore = keyStage2Performance.ReadingProgressScore.NotDisadvantaged.FormatValue(),
				WritingProgressScore = keyStage2Performance.WritingProgressScore.NotDisadvantaged.FormatValue(),
				MathsProgressScore = keyStage2Performance.MathsProgressScore.NotDisadvantaged.FormatValue(),
				NationalAveragePercentageMeetingExpectedStdInRWM = FormatKeyStageDisadvantagedResult(keyStage2Performance.NationalAveragePercentageMeetingExpectedStdInRWM),
				NationalAveragePercentageAchievingHigherStdInRWM = FormatKeyStageDisadvantagedResult(keyStage2Performance.NationalAveragePercentageAchievingHigherStdInRWM),
				NationalAverageReadingProgressScore = keyStage2Performance.NationalAverageReadingProgressScore.NotDisadvantaged.FormatValue(),
				NationalAverageWritingProgressScore = keyStage2Performance.NationalAverageWritingProgressScore.NotDisadvantaged.FormatValue(),
				NationalAverageMathsProgressScore = keyStage2Performance.NationalAverageMathsProgressScore.NotDisadvantaged.FormatValue(),
				LAAveragePercentageMeetingExpectedStdInRWM = keyStage2Performance.LAAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged.FormatValue(),
				LAAveragePercentageAchievingHigherStdInRWM = keyStage2Performance.LAAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged.FormatValue(),
				LAAverageMathsProgressScore = keyStage2Performance.LAAverageMathsProgressScore.NotDisadvantaged.FormatValue(),
				LAAverageReadingProgressScore = keyStage2Performance.LAAverageReadingProgressScore.NotDisadvantaged.FormatValue(),
				LAAverageWritingProgressScore = keyStage2Performance.LAAverageWritingProgressScore.NotDisadvantaged.FormatValue()
			};
		}
	}
}
