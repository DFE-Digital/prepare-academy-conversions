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
				SipAttainment8ScorePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8score?.NotDisadvantaged,
				SipAttainment8ScoreTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8score?.NotDisadvantaged,
				SipAttainment8ScoreDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8score?.Disadvantaged,
				SipAttainment8ScoreDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8score?.Disadvantaged,
				NationalAverageAttainment8Score = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8Score?.NotDisadvantaged,
				NationalAverageAttainment8ScorePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8Score?.NotDisadvantaged,
				NationalAverageAttainment8ScoreTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8Score?.NotDisadvantaged,
				NationalAverageAttainment8ScoreDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8Score?.Disadvantaged,
				NationalAverageAttainment8ScoreDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8Score?.Disadvantaged,
				NationalAverageAttainment8ScoreDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8Score?.Disadvantaged,

				SipAttainment8ScoreEnglish = keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoreenglish?.NotDisadvantaged,
				SipAttainment8ScoreEnglishPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoreenglish?.NotDisadvantaged,
				SipAttainment8ScoreEnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoreenglish),
				SipAttainment8ScoreEnglishDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoreenglish?.Disadvantaged,
				SipAttainment8ScoreEnglishDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoreenglish?.Disadvantaged,
				NationalAverageAttainment8English = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8English?.NotDisadvantaged,
				NationalAverageAttainment8EnglishPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8English?.NotDisadvantaged,
				NationalAverageAttainment8EnglishTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8English),
				NationalAverageAttainment8EnglishDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8English?.Disadvantaged,
				NationalAverageAttainment8EnglishDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8English?.Disadvantaged,

				SipAttainment8ScoreMaths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoremaths),
				SipAttainment8ScoreMathsPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoremaths?.NotDisadvantaged,
				SipAttainment8ScoreMathsTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoremaths?.NotDisadvantaged,
				SipAttainment8ScoreMathsDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoremaths?.Disadvantaged,
				SipAttainment8ScoreMathsDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoremaths?.Disadvantaged,
				SipAttainment8ScoreMathsDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoremaths?.Disadvantaged,
				NationalAverageAttainment8Maths = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8Maths?.NotDisadvantaged,
				NationalAverageAttainment8MathsPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8Maths?.NotDisadvantaged,
				NationalAverageAttainment8MathsTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8Maths?.NotDisadvantaged,
				NationalAverageAttainment8MathsDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8Maths?.Disadvantaged,
				NationalAverageAttainment8MathsDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8Maths?.Disadvantaged,
				NationalAverageAttainment8MathsDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8Maths?.Disadvantaged,

				SipAttainment8ScoreEbacc = keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoreebacc?.NotDisadvantaged,
				SipAttainment8ScoreEbaccPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoreebacc?.NotDisadvantaged,
				SipAttainment8ScoreEbaccTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoreebacc?.NotDisadvantaged,
				SipAttainment8ScoreEbaccDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.SipAttainment8scoreebacc?.Disadvantaged,
				SipAttainment8ScoreEbaccDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipAttainment8scoreebacc?.Disadvantaged,
				SipAttainment8ScoreEbaccDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipAttainment8scoreebacc?.Disadvantaged,
				NationalAverageAttainment8Ebacc = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8EBacc?.NotDisadvantaged,
				NationalAverageAttainment8EbaccPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8EBacc?.NotDisadvantaged,
				NationalAverageAttainment8EbaccTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8EBacc?.NotDisadvantaged,
				NationalAverageAttainment8EbaccDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageA8EBacc.Disadvantaged,
				NationalAverageAttainment8EbaccDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageA8EBacc?.Disadvantaged,
				NationalAverageAttainment8EbaccDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageA8EBacc?.Disadvantaged,

				SipNumberOfPupilsProgress8 = keyStage4PerformanceResults.ElementAt(0)?.SipNumberofpupilsprogress8?.NotDisadvantaged,
				SipNumberOfPupilsProgress8PreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipNumberofpupilsprogress8?.NotDisadvantaged,
				SipNumberOfPupilsProgress8TwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipNumberofpupilsprogress8?.NotDisadvantaged,
				SipNumberOfPupilsProgress8Disadvantaged = keyStage4PerformanceResults.ElementAt(0)?.SipNumberofpupilsprogress8?.Disadvantaged,
				SipNumberOfPupilsProgress8DisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipNumberofpupilsprogress8?.Disadvantaged,
				SipNumberOfPupilsProgress8DisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipNumberofpupilsprogress8?.Disadvantaged,
				NationalAverageP8PupilsIncluded = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8PupilsIncluded?.NotDisadvantaged,
				NationalAverageP8PupilsIncludedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8PupilsIncluded?.NotDisadvantaged,
				NationalAverageP8PupilsIncludedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8PupilsIncluded?.NotDisadvantaged,
				NationalAverageP8PupilsIncludedDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8PupilsIncluded?.Disadvantaged,
				NationalAverageP8PupilsIncludedDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8PupilsIncluded?.Disadvantaged,
				NationalAverageP8PupilsIncludedDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8PupilsIncluded?.Disadvantaged,

				SipProgress8Score = keyStage4PerformanceResults.ElementAt(0)?.SipProgress8Score?.NotDisadvantaged,
				SipProgress8ScorePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipProgress8Score?.NotDisadvantaged,
				SipProgress8ScoreTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipProgress8Score?.NotDisadvantaged,
				SipProgress8ScoreDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.SipProgress8Score?.Disadvantaged,
				SipProgress8ScoreDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipProgress8Score?.Disadvantaged,
				SipProgress8ScoreDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipProgress8Score?.Disadvantaged,
				SipProgress8UpperConfidence = keyStage4PerformanceResults.ElementAt(0)?.SipProgress8upperconfidence?.ToString(),
				SipProgress8UpperConfidencePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipProgress8upperconfidence?.ToString(),
				SipProgress8UpperConfidenceTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipProgress8upperconfidence?.ToString(),
				SipProgress8LowerConfidence = keyStage4PerformanceResults.ElementAt(0)?.SipProgress8lowerconfidence?.ToString(),
				SipProgress8LowerConfidencePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipProgress8lowerconfidence?.ToString(),
				SipProgress8LowerConfidenceTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipProgress8lowerconfidence?.ToString(),
				NationalAverageProgress8Score = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Score?.NotDisadvantaged,
				NationalAverageProgress8ScorePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Score?.NotDisadvantaged,
				NationalAverageProgress8ScoreTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Score?.NotDisadvantaged,
				NationalAverageProgress8ScoreDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Score?.Disadvantaged,
				NationalAverageProgress8ScoreDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Score?.Disadvantaged,
				NationalAverageProgress8ScoreDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Score?.Disadvantaged,
				NationalAverageProgress8UpperConfidence = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8UpperConfidence?.ToString(),
				NationalAverageProgress8UpperConfidencePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8UpperConfidence?.ToString(),
				NationalAverageProgress8UpperConfidenceTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8UpperConfidence?.ToString(),
				NationalAverageProgress8LowerConfidence = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8LowerConfidence?.ToString(),
				NationalAverageProgress8LowerConfidencePreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8LowerConfidence?.ToString(),
				NationalAverageProgress8LowerConfidenceTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8LowerConfidence?.ToString(),

				SipProgress8English = keyStage4PerformanceResults.ElementAt(0)?.SipProgress8english?.NotDisadvantaged,
				SipProgress8EnglishPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipProgress8english?.NotDisadvantaged,
				SipProgress8EnglishTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipProgress8english?.NotDisadvantaged,
				SipProgress8EnglishDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.SipProgress8english?.Disadvantaged,
				SipProgress8EnglishDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.SipProgress8english?.Disadvantaged,
				SipProgress8EnglishDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.SipProgress8english?.Disadvantaged,
				NationalAverageProgress8English = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8English?.NotDisadvantaged,
				NationalAverageProgress8EnglishPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8English?.NotDisadvantaged,
				NationalAverageProgress8EnglishTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8English?.NotDisadvantaged,
				NationalAverageProgress8EnglishDisadvantaged = keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8English?.Disadvantaged,
				NationalAverageProgress8EnglishDisadvantagedPreviousYear = keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8English?.Disadvantaged,
				NationalAverageProgress8EnglishDisadvantagedTwoYearsAgo = keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8English?.Disadvantaged,

				SipProgress8Maths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8maths),
				SipProgress8MathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8maths),
				SipProgress8MathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8maths),
				NationalAverageProgress8Maths = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Maths),
				NationalAverageProgress8MathsPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Maths),
				NationalAverageProgress8MathsTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Maths),

				SipProgress8Ebacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.SipProgress8ebacc),
				SipProgress8EbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.SipProgress8ebacc),
				SipProgress8EbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.SipProgress8ebacc),
				NationalAverageProgress8Ebacc = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(0)?.NationalAverageP8Ebacc),
				NationalAverageProgress8EbaccPreviousYear = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(1)?.NationalAverageP8Ebacc),
				NationalAverageProgress8EbaccTwoYearsAgo = DisplayExtensions.DisplayKeyStageDisadvantagedResult(keyStage4PerformanceResults.ElementAt(2)?.NationalAverageP8Ebacc),

				// LA fields
			};
		}
	}
}
