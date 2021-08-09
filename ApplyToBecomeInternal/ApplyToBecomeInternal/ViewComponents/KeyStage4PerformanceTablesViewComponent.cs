using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class KeyStage4PerformanceTablesViewComponent : ViewComponent
	{
		private readonly KeyStagePerformanceService _keyStagePerformanceService;
		private readonly IAcademyConversionProjectRepository _repository;

		public KeyStage4PerformanceTablesViewComponent(
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

			var viewModel = Build(keyStagePerformance.KeyStage4.ToList());

			return View(viewModel);
		}

		private KeyStage4PerformanceTableViewModel Build(IReadOnlyCollection<KeyStage4PerformanceResponse> keyStage4Performance)
		{
			var keyStage4PerformanceResults = keyStage4Performance.Take(3).OrderByDescending(ks4 => ks4.Year)
				.Concat(Enumerable.Range(0, 3).Select(_ => new KeyStage4PerformanceResponse())).Take(3).ToList();

			return new KeyStage4PerformanceTableViewModel
			{
				Year = keyStage4PerformanceResults.ElementAt(0)?.Year,
				PreviousYear = keyStage4PerformanceResults.ElementAt(1)?.Year,
				TwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.Year,

				SipAttainment8Score = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8score),
				SipAttainment8ScorePreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8score),
				SipAttainment8ScoreTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8score),
				LaAverageAttainment8Score = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageA8Score),
				LaAverageAttainment8ScorePreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageA8Score),
				LaAverageAttainment8ScoreTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageA8Score),
				NationalAverageAttainment8Score = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8Score),
				NationalAverageAttainment8ScorePreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8Score),
				NationalAverageAttainment8ScoreTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8Score),

				SipAttainment8ScoreEnglish = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoreenglish),
				SipAttainment8ScoreEnglishPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoreenglish),
				SipAttainment8ScoreEnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoreenglish),
				LaAverageAttainment8ScoreEnglish = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageA8English),
				LaAverageAttainment8ScoreEnglishPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageA8English),
				LaAverageAttainment8ScoreEnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageA8English),
				NationalAverageAttainment8English = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8English),
				NationalAverageAttainment8EnglishPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8English),
				NationalAverageAttainment8EnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8English),

				SipAttainment8ScoreMaths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoremaths),
				SipAttainment8ScoreMathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoremaths),
				SipAttainment8ScoreMathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoremaths),
				LaAverageAttainment8ScoreMaths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageA8Maths),
				LaAverageAttainment8ScoreMathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageA8Maths),
				LaAverageAttainment8ScoreMathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageA8Maths),
				NationalAverageAttainment8Maths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8Maths),
				NationalAverageAttainment8MathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8Maths),
				NationalAverageAttainment8MathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8Maths),

				SipAttainment8ScoreEbacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoreebacc),
				SipAttainment8ScoreEbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoreebacc),
				SipAttainment8ScoreEbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoreebacc),
				LaAverageAttainment8ScoreEbacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageA8EBacc),
				LaAverageAttainment8ScoreEbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageA8EBacc),
				LaAverageAttainment8ScoreEbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageA8EBacc),
				NationalAverageAttainment8Ebacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8EBacc),
				NationalAverageAttainment8EbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8EBacc),
				NationalAverageAttainment8EbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8EBacc),

				SipNumberOfPupilsProgress8 = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipNumberofpupilsprogress8),
				SipNumberOfPupilsProgress8PreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipNumberofpupilsprogress8),
				SipNumberOfPupilsProgress8TwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipNumberofpupilsprogress8),
				LaAveragePupilsIncludedProgress8 = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8PupilsIncluded),
				LaAveragePupilsIncludedProgress8PreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8PupilsIncluded),
				LaAveragePupilsIncludedProgress8TwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8PupilsIncluded),
				NationalAveragePupilsIncludedProgress8 = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8PupilsIncluded),
				NationalAveragePupilsIncludedProgress8PreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8PupilsIncluded),
				NationalAveragePupilsIncludedProgress8TwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8PupilsIncluded),

				SipProgress8Score = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8Score),
				SipProgress8ScorePreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8Score),
				SipProgress8ScoreTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8Score),
				LaAverageProgress8Score = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8Score),
				LaAverageProgress8ScorePreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8Score),
				LaAverageProgress8ScoreTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8Score),
				NationalAverageProgress8Score = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Score),
				NationalAverageProgress8ScorePreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Score),
				NationalAverageProgress8ScoreTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Score),
				SipProgress8ConfidenceInterval = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8lowerconfidence, keyStage4PerformanceResults.ElementAt(0)?.SipProgress8upperconfidence),
				SipProgress8ConfidenceIntervalPreviousYear = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8lowerconfidence, keyStage4PerformanceResults.ElementAt(1)?.SipProgress8upperconfidence),
				SipProgress8ConfidenceIntervalTwoYearsAgo = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8lowerconfidence, keyStage4PerformanceResults.ElementAt(2)?.SipProgress8upperconfidence),
				LaAverageProgress8ConfidenceInterval = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8LowerConfidence, keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8UpperConfidence),
				LaAverageProgress8ConfidenceIntervalPreviousYear = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8LowerConfidence, keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8UpperConfidence),
				LaAverageProgress8ConfidenceIntervalTwoYearsAgo = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8LowerConfidence, keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8UpperConfidence),
				NationalAverageProgress8ConfidenceInterval = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8LowerConfidence, keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8UpperConfidence),
				NationalAverageProgress8ConfidenceIntervalPreviousYear = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8LowerConfidence, keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8UpperConfidence),
				NationalAverageProgress8ConfidenceIntervalTwoYearsAgo = DisplayExtensions.DisplayConfidenceInterval(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8LowerConfidence, keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8UpperConfidence),

				SipProgress8English = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8english),
				SipProgress8EnglishPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8english),
				SipProgress8EnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8english),
				LaAverageProgress8ScoreEnglish = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8English),
				LaAverageProgress8ScoreEnglishPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8English),
				LaAverageProgress8ScoreEnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8English),
				NationalAverageProgress8English = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8English),
				NationalAverageProgress8EnglishPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8English),
				NationalAverageProgress8EnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8English),

				SipProgress8Maths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8maths),
				SipProgress8MathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8maths),
				SipProgress8MathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8maths),
				LaAverageProgress8ScoreMaths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8Maths),
				LaAverageProgress8ScoreMathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8Maths),
				LaAverageProgress8ScoreMathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8Maths),
				NationalAverageProgress8Maths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Maths),
				NationalAverageProgress8MathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Maths),
				NationalAverageProgress8MathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Maths),

				SipProgress8Ebacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8ebacc),
				SipProgress8EbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8ebacc),
				SipProgress8EbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8ebacc),
				LaAverageProgress8ScoreEbacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.LAAverageP8Ebacc),
				LaAverageProgress8ScoreEbaccPreviousYear= DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.LAAverageP8Ebacc),
				LaAverageProgress8ScoreEbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.LAAverageP8Ebacc),
				NationalAverageProgress8Ebacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Ebacc),
				NationalAverageProgress8EbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Ebacc),
				NationalAverageProgress8EbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Ebacc),

				// LA fields
			};
		}
	}
}
