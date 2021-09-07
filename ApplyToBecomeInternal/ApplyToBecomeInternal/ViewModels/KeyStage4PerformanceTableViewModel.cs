using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;
using System.Linq;
using static ApplyToBecomeInternal.Extensions.DisplayExtensions;

namespace ApplyToBecomeInternal.ViewModels
{
	public class KeyStage4PerformanceTableViewModel
	{
		public string Year { get; set; }
		public string PreviousYear { get; set; }
		public string TwoYearsAgo { get; set; }

		public string Attainment8Score { get; set; }
		public string Attainment8ScorePreviousYear { get; set; }
		public string Attainment8ScoreTwoYearsAgo { get; set; }
		public string LaAverageAttainment8Score { get; set; }
		public string LaAverageAttainment8ScorePreviousYear { get; set; }
		public string LaAverageAttainment8ScoreTwoYearsAgo { get; set; }
		public string NationalAverageAttainment8Score { get; set; }
		public string NationalAverageAttainment8ScorePreviousYear { get; set; }
		public string NationalAverageAttainment8ScoreTwoYearsAgo { get; set; }

		public string Attainment8ScoreEnglish { get; set; }
		public string Attainment8ScoreEnglishPreviousYear { get; set; }
		public string Attainment8ScoreEnglishTwoYearsAgo { get; set; }
		public string LaAverageAttainment8ScoreEnglish { get; set; }
		public string LaAverageAttainment8ScoreEnglishPreviousYear { get; set; }
		public string LaAverageAttainment8ScoreEnglishTwoYearsAgo { get; set; }
		public string NationalAverageAttainment8ScoreEnglish { get; set; }
		public string NationalAverageAttainment8ScoreEnglishPreviousYear { get; set; }
		public string NationalAverageAttainment8ScoreEnglishTwoYearsAgo { get; set; }

		public string Attainment8ScoreMaths { get; set; }
		public string Attainment8ScoreMathsPreviousYear { get; set; }
		public string Attainment8ScoreMathsTwoYearsAgo { get; set; }
		public string LaAverageAttainment8ScoreMaths { get; set; }
		public string LaAverageAttainment8ScoreMathsPreviousYear { get; set; }
		public string LaAverageAttainment8ScoreMathsTwoYearsAgo { get; set; }
		public string NationalAverageAttainment8ScoreMaths { get; set; }
		public string NationalAverageAttainment8ScoreMathsPreviousYear { get; set; }
		public string NationalAverageAttainment8ScoreMathsTwoYearsAgo { get; set; }

		public string Attainment8ScoreEbacc { get; set; }
		public string Attainment8ScoreEbaccPreviousYear { get; set; }
		public string Attainment8ScoreEbaccTwoYearsAgo { get; set; }
		public string LaAverageAttainment8ScoreEbacc { get; set; }
		public string LaAverageAttainment8ScoreEbaccPreviousYear { get; set; }
		public string LaAverageAttainment8ScoreEbaccTwoYearsAgo { get; set; }
		public string NationalAverageAttainment8ScoreEbacc { get; set; }
		public string NationalAverageAttainment8ScoreEbaccPreviousYear { get; set; }
		public string NationalAverageAttainment8ScoreEbaccTwoYearsAgo { get; set; }

		public string NumberOfPupilsProgress8 { get; set; }
		public string NumberOfPupilsProgress8PreviousYear { get; set; }
		public string NumberOfPupilsProgress8TwoYearsAgo { get; set; }
		public string LaAveragePupilsIncludedProgress8 { get; set; }
		public string LaAveragePupilsIncludedProgress8PreviousYear { get; set; }
		public string LaAveragePupilsIncludedProgress8TwoYearsAgo { get; set; }
		public string NationalAveragePupilsIncludedProgress8 { get; set; }
		public string NationalAveragePupilsIncludedProgress8PreviousYear { get; set; }
		public string NationalAveragePupilsIncludedProgress8TwoYearsAgo { get; set; }

		public string Progress8Score { get; set; }
		public string Progress8ScorePreviousYear { get; set; }
		public string Progress8ScoreTwoYearsAgo { get; set; }
		public string Progress8ConfidenceInterval { get; set; }
		public string Progress8ConfidenceIntervalPreviousYear { get; set; }
		public string Progress8ConfidenceIntervalTwoYearsAgo { get; set; }
		public string LaAverageProgress8Score { get; set; }
		public string LaAverageProgress8ScorePreviousYear { get; set; }
		public string LaAverageProgress8ScoreTwoYearsAgo { get; set; }
		public string LaAverageProgress8ConfidenceInterval { get; set; }
		public string LaAverageProgress8ConfidenceIntervalPreviousYear { get; set; }
		public string LaAverageProgress8ConfidenceIntervalTwoYearsAgo { get; set; }
		public string NationalAverageProgress8Score { get; set; }
		public string NationalAverageProgress8ScorePreviousYear { get; set; }
		public string NationalAverageProgress8ScoreTwoYearsAgo { get; set; }
		public string NationalAverageProgress8ConfidenceInterval { get; set; }
		public string NationalAverageProgress8ConfidenceIntervalPreviousYear { get; set; }
		public string NationalAverageProgress8ConfidenceIntervalTwoYearsAgo { get; set; }

		public string Progress8ScoreEnglish { get; set; }
		public string Progress8ScoreEnglishPreviousYear { get; set; }
		public string Progress8ScoreEnglishTwoYearsAgo { get; set; }
		public string LaAverageProgress8ScoreEnglish { get; set; }
		public string LaAverageProgress8ScoreEnglishPreviousYear { get; set; }
		public string LaAverageProgress8ScoreEnglishTwoYearsAgo { get; set; }
		public string NationalAverageProgress8ScoreEnglish { get; set; }
		public string NationalAverageProgress8ScoreEnglishPreviousYear { get; set; }
		public string NationalAverageProgress8ScoreEnglishTwoYearsAgo { get; set; }

		public string Progress8ScoreMaths { get; set; }
		public string Progress8ScoreMathsPreviousYear { get; set; }
		public string Progress8ScoreMathsTwoYearsAgo { get; set; }
		public string LaAverageProgress8ScoreMaths { get; set; }
		public string LaAverageProgress8ScoreMathsPreviousYear { get; set; }
		public string LaAverageProgress8ScoreMathsTwoYearsAgo { get; set; }
		public string NationalAverageProgress8ScoreMaths { get; set; }
		public string NationalAverageProgress8ScoreMathsPreviousYear { get; set; }
		public string NationalAverageProgress8ScoreMathsTwoYearsAgo { get; set; }

		public string Progress8ScoreEbacc { get; set; }
		public string Progress8ScoreEbaccPreviousYear { get; set; }
		public string Progress8ScoreEbaccTwoYearsAgo { get; set; }
		public string LaAverageProgress8ScoreEbacc { get; set; }
		public string LaAverageProgress8ScoreEbaccPreviousYear { get; set; }
		public string LaAverageProgress8ScoreEbaccTwoYearsAgo { get; set; }
		public string NationalAverageProgress8ScoreEbacc { get; set; }
		public string NationalAverageProgress8ScoreEbaccPreviousYear { get; set; }
		public string NationalAverageProgress8ScoreEbaccTwoYearsAgo { get; set; }

		public string PercentageEnteringEbacc { get; set; }
		public string PercentageEnteringEbaccPreviousYear { get; set; }
		public string PercentageEnteringEbaccTwoYearsAgo { get; set; }
		public string LaPercentageEnteringEbacc { get; set; }
		public string LaPercentageEnteringEbaccPreviousYear { get; set; }
		public string LaPercentageEnteringEbaccTwoYearsAgo { get; set; }
		public string NaPercentageEnteringEbacc { get; set; }
		public string NaPercentageEnteringEbaccPreviousYear { get; set; }
		public string NaPercentageEnteringEbaccTwoYearsAgo { get; set; }

		public static KeyStage4PerformanceTableViewModel Build(IEnumerable<KeyStage4PerformanceResponse> keyStage4Performance)
		{
			var keyStage4PerformanceOrdered = keyStage4Performance.Take(3).OrderByDescending(ks4 => ks4.Year)
				.Concat(Enumerable.Range(0, 3).Select(_ => new KeyStage4PerformanceResponse())).Take(3).ToList();

			return new KeyStage4PerformanceTableViewModel
			{
				Year = keyStage4PerformanceOrdered.ElementAt(0)?.Year.FormatKeyStageYear(),
				PreviousYear = keyStage4PerformanceOrdered.ElementAt(1)?.Year.FormatKeyStageYear(),
				TwoYearsAgo = keyStage4PerformanceOrdered.ElementAt(2)?.Year.FormatKeyStageYear(),
				Attainment8Score = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8score),
				Attainment8ScorePreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8score),
				Attainment8ScoreTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8score),
				LaAverageAttainment8Score = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8Score),
				LaAverageAttainment8ScorePreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8Score),
				LaAverageAttainment8ScoreTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8Score),
				NationalAverageAttainment8Score = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8Score),
				NationalAverageAttainment8ScorePreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8Score),
				NationalAverageAttainment8ScoreTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8Score),
				Attainment8ScoreEnglish = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8scoreenglish),
				Attainment8ScoreEnglishPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8scoreenglish),
				Attainment8ScoreEnglishTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8scoreenglish),
				LaAverageAttainment8ScoreEnglish = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8English),
				LaAverageAttainment8ScoreEnglishPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8English),
				LaAverageAttainment8ScoreEnglishTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8English),
				NationalAverageAttainment8ScoreEnglish = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8English),
				NationalAverageAttainment8ScoreEnglishPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8English),
				NationalAverageAttainment8ScoreEnglishTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8English),
				Attainment8ScoreMaths = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8scoremaths),
				Attainment8ScoreMathsPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8scoremaths),
				Attainment8ScoreMathsTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8scoremaths),
				LaAverageAttainment8ScoreMaths = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8Maths),
				LaAverageAttainment8ScoreMathsPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8Maths),
				LaAverageAttainment8ScoreMathsTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8Maths),
				NationalAverageAttainment8ScoreMaths = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8Maths),
				NationalAverageAttainment8ScoreMathsPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8Maths),
				NationalAverageAttainment8ScoreMathsTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8Maths),
				Attainment8ScoreEbacc = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8scoreebacc),
				Attainment8ScoreEbaccPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8scoreebacc),
				Attainment8ScoreEbaccTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8scoreebacc),
				LaAverageAttainment8ScoreEbacc = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8EBacc),
				LaAverageAttainment8ScoreEbaccPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8EBacc),
				LaAverageAttainment8ScoreEbaccTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8EBacc),
				NationalAverageAttainment8ScoreEbacc = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8EBacc),
				NationalAverageAttainment8ScoreEbaccPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8EBacc),
				NationalAverageAttainment8ScoreEbaccTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8EBacc),
				NumberOfPupilsProgress8 = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipNumberofpupilsprogress8),
				NumberOfPupilsProgress8PreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipNumberofpupilsprogress8),
				NumberOfPupilsProgress8TwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipNumberofpupilsprogress8),
				LaAveragePupilsIncludedProgress8 = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8PupilsIncluded),
				LaAveragePupilsIncludedProgress8PreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8PupilsIncluded),
				LaAveragePupilsIncludedProgress8TwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8PupilsIncluded),
				NationalAveragePupilsIncludedProgress8 = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8PupilsIncluded),
				NationalAveragePupilsIncludedProgress8PreviousYear =
					FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8PupilsIncluded),
				NationalAveragePupilsIncludedProgress8TwoYearsAgo =
					FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8PupilsIncluded),
				Progress8Score = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8Score),
				Progress8ScorePreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8Score),
				Progress8ScoreTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8Score),
				LaAverageProgress8Score = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8Score),
				LaAverageProgress8ScorePreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8Score),
				LaAverageProgress8ScoreTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8Score),
				NationalAverageProgress8Score = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8Score),
				NationalAverageProgress8ScorePreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8Score),
				NationalAverageProgress8ScoreTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8Score),
				Progress8ConfidenceInterval =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8lowerconfidence,
						keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8upperconfidence),
				Progress8ConfidenceIntervalPreviousYear =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8lowerconfidence,
						keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8upperconfidence),
				Progress8ConfidenceIntervalTwoYearsAgo =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8lowerconfidence,
						keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8upperconfidence),
				LaAverageProgress8ConfidenceInterval =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8LowerConfidence,
						keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8UpperConfidence),
				LaAverageProgress8ConfidenceIntervalPreviousYear =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8LowerConfidence,
						keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8UpperConfidence),
				LaAverageProgress8ConfidenceIntervalTwoYearsAgo =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8LowerConfidence,
						keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8UpperConfidence),
				NationalAverageProgress8ConfidenceInterval =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8LowerConfidence,
						keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8UpperConfidence),
				NationalAverageProgress8ConfidenceIntervalPreviousYear =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8LowerConfidence,
						keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8UpperConfidence),
				NationalAverageProgress8ConfidenceIntervalTwoYearsAgo =
					FormatConfidenceInterval(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8LowerConfidence,
						keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8UpperConfidence),
				Progress8ScoreEnglish = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8english),
				Progress8ScoreEnglishPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8english),
				Progress8ScoreEnglishTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8english),
				LaAverageProgress8ScoreEnglish = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8English),
				LaAverageProgress8ScoreEnglishPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8English),
				LaAverageProgress8ScoreEnglishTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8English),
				NationalAverageProgress8ScoreEnglish = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8English),
				NationalAverageProgress8ScoreEnglishPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8English),
				NationalAverageProgress8ScoreEnglishTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8English),
				Progress8ScoreMaths = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8maths),
				Progress8ScoreMathsPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8maths),
				Progress8ScoreMathsTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8maths),
				LaAverageProgress8ScoreMaths = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8Maths),
				LaAverageProgress8ScoreMathsPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8Maths),
				LaAverageProgress8ScoreMathsTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8Maths),
				NationalAverageProgress8ScoreMaths = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8Maths),
				NationalAverageProgress8ScoreMathsPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8Maths),
				NationalAverageProgress8ScoreMathsTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8Maths),
				Progress8ScoreEbacc = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8ebacc),
				Progress8ScoreEbaccPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8ebacc),
				Progress8ScoreEbaccTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8ebacc),
				LaAverageProgress8ScoreEbacc = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8Ebacc),
				LaAverageProgress8ScoreEbaccPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8Ebacc),
				LaAverageProgress8ScoreEbaccTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8Ebacc),
				NationalAverageProgress8ScoreEbacc = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8Ebacc),
				NationalAverageProgress8ScoreEbaccPreviousYear = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8Ebacc),
				NationalAverageProgress8ScoreEbaccTwoYearsAgo = FormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8Ebacc),
				PercentageEnteringEbacc = keyStage4PerformanceOrdered.ElementAt(0)?.Enteringebacc.ToSafeString(),
				PercentageEnteringEbaccPreviousYear = keyStage4PerformanceOrdered.ElementAt(1)?.Enteringebacc.ToSafeString(),
				PercentageEnteringEbaccTwoYearsAgo = keyStage4PerformanceOrdered.ElementAt(2)?.Enteringebacc.ToSafeString(),
				LaPercentageEnteringEbacc = keyStage4PerformanceOrdered.ElementAt(0)?.LAEnteringEbacc.ToSafeString(),
				LaPercentageEnteringEbaccPreviousYear = keyStage4PerformanceOrdered.ElementAt(1)?.LAEnteringEbacc.ToSafeString(),
				LaPercentageEnteringEbaccTwoYearsAgo = keyStage4PerformanceOrdered.ElementAt(2)?.LAEnteringEbacc.ToSafeString(),
				NaPercentageEnteringEbacc = keyStage4PerformanceOrdered.ElementAt(0)?.NationalEnteringEbacc.ToSafeString(),
				NaPercentageEnteringEbaccPreviousYear = keyStage4PerformanceOrdered.ElementAt(1)?.NationalEnteringEbacc.ToSafeString(),
				NaPercentageEnteringEbaccTwoYearsAgo = keyStage4PerformanceOrdered.ElementAt(2)?.NationalEnteringEbacc.ToSafeString()
			};
		}
	}
}