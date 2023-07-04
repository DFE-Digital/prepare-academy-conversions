using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Extensions;
using Dfe.Academisation.ExtensionMethods;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Linq;
using static Dfe.PrepareConversions.Extensions.DisplayExtensions;

namespace Dfe.PrepareConversions.ViewModels;

public class KeyStage4PerformanceTableViewModel
{
   public string Year { get; set; }
   public string PreviousYear { get; set; }
   public string TwoYearsAgo { get; set; }

   public HtmlString Attainment8Score { get; set; }
   public HtmlString Attainment8ScorePreviousYear { get; set; }
   public HtmlString Attainment8ScoreTwoYearsAgo { get; set; }
   public HtmlString LaAverageAttainment8Score { get; set; }
   public HtmlString LaAverageAttainment8ScorePreviousYear { get; set; }
   public HtmlString LaAverageAttainment8ScoreTwoYearsAgo { get; set; }
   public HtmlString NationalAverageAttainment8Score { get; set; }
   public HtmlString NationalAverageAttainment8ScorePreviousYear { get; set; }
   public HtmlString NationalAverageAttainment8ScoreTwoYearsAgo { get; set; }

   public HtmlString Attainment8ScoreEnglish { get; set; }
   public HtmlString Attainment8ScoreEnglishPreviousYear { get; set; }
   public HtmlString Attainment8ScoreEnglishTwoYearsAgo { get; set; }
   public HtmlString LaAverageAttainment8ScoreEnglish { get; set; }
   public HtmlString LaAverageAttainment8ScoreEnglishPreviousYear { get; set; }
   public HtmlString LaAverageAttainment8ScoreEnglishTwoYearsAgo { get; set; }
   public HtmlString NationalAverageAttainment8ScoreEnglish { get; set; }
   public HtmlString NationalAverageAttainment8ScoreEnglishPreviousYear { get; set; }
   public HtmlString NationalAverageAttainment8ScoreEnglishTwoYearsAgo { get; set; }

   public HtmlString Attainment8ScoreMaths { get; set; }
   public HtmlString Attainment8ScoreMathsPreviousYear { get; set; }
   public HtmlString Attainment8ScoreMathsTwoYearsAgo { get; set; }
   public HtmlString LaAverageAttainment8ScoreMaths { get; set; }
   public HtmlString LaAverageAttainment8ScoreMathsPreviousYear { get; set; }
   public HtmlString LaAverageAttainment8ScoreMathsTwoYearsAgo { get; set; }
   public HtmlString NationalAverageAttainment8ScoreMaths { get; set; }
   public HtmlString NationalAverageAttainment8ScoreMathsPreviousYear { get; set; }
   public HtmlString NationalAverageAttainment8ScoreMathsTwoYearsAgo { get; set; }

   public HtmlString Attainment8ScoreEbacc { get; set; }
   public HtmlString Attainment8ScoreEbaccPreviousYear { get; set; }
   public HtmlString Attainment8ScoreEbaccTwoYearsAgo { get; set; }
   public HtmlString LaAverageAttainment8ScoreEbacc { get; set; }
   public HtmlString LaAverageAttainment8ScoreEbaccPreviousYear { get; set; }
   public HtmlString LaAverageAttainment8ScoreEbaccTwoYearsAgo { get; set; }
   public HtmlString NationalAverageAttainment8ScoreEbacc { get; set; }
   public HtmlString NationalAverageAttainment8ScoreEbaccPreviousYear { get; set; }
   public HtmlString NationalAverageAttainment8ScoreEbaccTwoYearsAgo { get; set; }

   public HtmlString NumberOfPupilsProgress8 { get; set; }
   public HtmlString NumberOfPupilsProgress8PreviousYear { get; set; }
   public HtmlString NumberOfPupilsProgress8TwoYearsAgo { get; set; }
   public HtmlString LaAveragePupilsIncludedProgress8 { get; set; }
   public HtmlString LaAveragePupilsIncludedProgress8PreviousYear { get; set; }
   public HtmlString LaAveragePupilsIncludedProgress8TwoYearsAgo { get; set; }
   public HtmlString NationalAveragePupilsIncludedProgress8 { get; set; }
   public HtmlString NationalAveragePupilsIncludedProgress8PreviousYear { get; set; }
   public HtmlString NationalAveragePupilsIncludedProgress8TwoYearsAgo { get; set; }

   public HtmlString Progress8Score { get; set; }
   public HtmlString Progress8ScorePreviousYear { get; set; }
   public HtmlString Progress8ScoreTwoYearsAgo { get; set; }
   public string Progress8ConfidenceInterval { get; set; }
   public string Progress8ConfidenceIntervalPreviousYear { get; set; }
   public string Progress8ConfidenceIntervalTwoYearsAgo { get; set; }
   public HtmlString LaAverageProgress8Score { get; set; }
   public HtmlString LaAverageProgress8ScorePreviousYear { get; set; }
   public HtmlString LaAverageProgress8ScoreTwoYearsAgo { get; set; }
   public string LaAverageProgress8ConfidenceInterval { get; set; }
   public string LaAverageProgress8ConfidenceIntervalPreviousYear { get; set; }
   public string LaAverageProgress8ConfidenceIntervalTwoYearsAgo { get; set; }
   public HtmlString NationalAverageProgress8Score { get; set; }
   public HtmlString NationalAverageProgress8ScorePreviousYear { get; set; }
   public HtmlString NationalAverageProgress8ScoreTwoYearsAgo { get; set; }
   public string NationalAverageProgress8ConfidenceInterval { get; set; }
   public string NationalAverageProgress8ConfidenceIntervalPreviousYear { get; set; }
   public string NationalAverageProgress8ConfidenceIntervalTwoYearsAgo { get; set; }

   public HtmlString Progress8ScoreEnglish { get; set; }
   public HtmlString Progress8ScoreEnglishPreviousYear { get; set; }
   public HtmlString Progress8ScoreEnglishTwoYearsAgo { get; set; }
   public HtmlString LaAverageProgress8ScoreEnglish { get; set; }
   public HtmlString LaAverageProgress8ScoreEnglishPreviousYear { get; set; }
   public HtmlString LaAverageProgress8ScoreEnglishTwoYearsAgo { get; set; }
   public HtmlString NationalAverageProgress8ScoreEnglish { get; set; }
   public HtmlString NationalAverageProgress8ScoreEnglishPreviousYear { get; set; }
   public HtmlString NationalAverageProgress8ScoreEnglishTwoYearsAgo { get; set; }

   public HtmlString Progress8ScoreMaths { get; set; }
   public HtmlString Progress8ScoreMathsPreviousYear { get; set; }
   public HtmlString Progress8ScoreMathsTwoYearsAgo { get; set; }
   public HtmlString LaAverageProgress8ScoreMaths { get; set; }
   public HtmlString LaAverageProgress8ScoreMathsPreviousYear { get; set; }
   public HtmlString LaAverageProgress8ScoreMathsTwoYearsAgo { get; set; }
   public HtmlString NationalAverageProgress8ScoreMaths { get; set; }
   public HtmlString NationalAverageProgress8ScoreMathsPreviousYear { get; set; }
   public HtmlString NationalAverageProgress8ScoreMathsTwoYearsAgo { get; set; }

   public HtmlString Progress8ScoreEbacc { get; set; }
   public HtmlString Progress8ScoreEbaccPreviousYear { get; set; }
   public HtmlString Progress8ScoreEbaccTwoYearsAgo { get; set; }
   public HtmlString LaAverageProgress8ScoreEbacc { get; set; }
   public HtmlString LaAverageProgress8ScoreEbaccPreviousYear { get; set; }
   public HtmlString LaAverageProgress8ScoreEbaccTwoYearsAgo { get; set; }
   public HtmlString NationalAverageProgress8ScoreEbacc { get; set; }
   public HtmlString NationalAverageProgress8ScoreEbaccPreviousYear { get; set; }
   public HtmlString NationalAverageProgress8ScoreEbaccTwoYearsAgo { get; set; }

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
      IEnumerable<KeyStage4PerformanceResponse> distinctKeyStage4Performance = keyStage4Performance.DistinctBy(p => p.Year);
      List<KeyStage4PerformanceResponse> keyStage4PerformanceOrdered = distinctKeyStage4Performance.OrderByDescending(ks4 => ks4.Year)
         .Concat(Enumerable.Range(0, 3).Select(_ => new KeyStage4PerformanceResponse())).Take(3).ToList();

      return new KeyStage4PerformanceTableViewModel
      {
         Year = keyStage4PerformanceOrdered.ElementAt(0)?.Year.FormatKeyStageYear(),
         PreviousYear = keyStage4PerformanceOrdered.ElementAt(1)?.Year.FormatKeyStageYear(),
         TwoYearsAgo = keyStage4PerformanceOrdered.ElementAt(2)?.Year.FormatKeyStageYear(),
         Attainment8Score = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8score),
         Attainment8ScorePreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8score),
         Attainment8ScoreTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8score),
         LaAverageAttainment8Score = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8Score),
         LaAverageAttainment8ScorePreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8Score),
         LaAverageAttainment8ScoreTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8Score),
         NationalAverageAttainment8Score = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8Score),
         NationalAverageAttainment8ScorePreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8Score),
         NationalAverageAttainment8ScoreTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8Score),
         Attainment8ScoreEnglish = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8scoreenglish),
         Attainment8ScoreEnglishPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8scoreenglish),
         Attainment8ScoreEnglishTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8scoreenglish),
         LaAverageAttainment8ScoreEnglish = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8English),
         LaAverageAttainment8ScoreEnglishPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8English),
         LaAverageAttainment8ScoreEnglishTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8English),
         NationalAverageAttainment8ScoreEnglish = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8English),
         NationalAverageAttainment8ScoreEnglishPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8English),
         NationalAverageAttainment8ScoreEnglishTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8English),
         Attainment8ScoreMaths = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8scoremaths),
         Attainment8ScoreMathsPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8scoremaths),
         Attainment8ScoreMathsTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8scoremaths),
         LaAverageAttainment8ScoreMaths = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8Maths),
         LaAverageAttainment8ScoreMathsPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8Maths),
         LaAverageAttainment8ScoreMathsTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8Maths),
         NationalAverageAttainment8ScoreMaths = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8Maths),
         NationalAverageAttainment8ScoreMathsPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8Maths),
         NationalAverageAttainment8ScoreMathsTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8Maths),
         Attainment8ScoreEbacc = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipAttainment8scoreebacc),
         Attainment8ScoreEbaccPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipAttainment8scoreebacc),
         Attainment8ScoreEbaccTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipAttainment8scoreebacc),
         LaAverageAttainment8ScoreEbacc = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageA8EBacc),
         LaAverageAttainment8ScoreEbaccPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageA8EBacc),
         LaAverageAttainment8ScoreEbaccTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageA8EBacc),
         NationalAverageAttainment8ScoreEbacc = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageA8EBacc),
         NationalAverageAttainment8ScoreEbaccPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageA8EBacc),
         NationalAverageAttainment8ScoreEbaccTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageA8EBacc),
         NumberOfPupilsProgress8 = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipNumberofpupilsprogress8),
         NumberOfPupilsProgress8PreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipNumberofpupilsprogress8),
         NumberOfPupilsProgress8TwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipNumberofpupilsprogress8),
         LaAveragePupilsIncludedProgress8 = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8PupilsIncluded),
         LaAveragePupilsIncludedProgress8PreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8PupilsIncluded),
         LaAveragePupilsIncludedProgress8TwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8PupilsIncluded),
         NationalAveragePupilsIncludedProgress8 = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8PupilsIncluded),
         NationalAveragePupilsIncludedProgress8PreviousYear =
            HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8PupilsIncluded),
         NationalAveragePupilsIncludedProgress8TwoYearsAgo =
            HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8PupilsIncluded),
         Progress8Score = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8Score),
         Progress8ScorePreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8Score),
         Progress8ScoreTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8Score),
         LaAverageProgress8Score = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8Score),
         LaAverageProgress8ScorePreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8Score),
         LaAverageProgress8ScoreTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8Score),
         NationalAverageProgress8Score = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8Score),
         NationalAverageProgress8ScorePreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8Score),
         NationalAverageProgress8ScoreTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8Score),
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
         Progress8ScoreEnglish = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8english),
         Progress8ScoreEnglishPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8english),
         Progress8ScoreEnglishTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8english),
         LaAverageProgress8ScoreEnglish = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8English),
         LaAverageProgress8ScoreEnglishPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8English),
         LaAverageProgress8ScoreEnglishTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8English),
         NationalAverageProgress8ScoreEnglish = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8English),
         NationalAverageProgress8ScoreEnglishPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8English),
         NationalAverageProgress8ScoreEnglishTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8English),
         Progress8ScoreMaths = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8maths),
         Progress8ScoreMathsPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8maths),
         Progress8ScoreMathsTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8maths),
         LaAverageProgress8ScoreMaths = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8Maths),
         LaAverageProgress8ScoreMathsPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8Maths),
         LaAverageProgress8ScoreMathsTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8Maths),
         NationalAverageProgress8ScoreMaths = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8Maths),
         NationalAverageProgress8ScoreMathsPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8Maths),
         NationalAverageProgress8ScoreMathsTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8Maths),
         Progress8ScoreEbacc = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.SipProgress8ebacc),
         Progress8ScoreEbaccPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.SipProgress8ebacc),
         Progress8ScoreEbaccTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.SipProgress8ebacc),
         LaAverageProgress8ScoreEbacc = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.LAAverageP8Ebacc),
         LaAverageProgress8ScoreEbaccPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.LAAverageP8Ebacc),
         LaAverageProgress8ScoreEbaccTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.LAAverageP8Ebacc),
         NationalAverageProgress8ScoreEbacc = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(0)?.NationalAverageP8Ebacc),
         NationalAverageProgress8ScoreEbaccPreviousYear = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(1)?.NationalAverageP8Ebacc),
         NationalAverageProgress8ScoreEbaccTwoYearsAgo = HtmlFormatKeyStageDisadvantagedResult(keyStage4PerformanceOrdered.ElementAt(2)?.NationalAverageP8Ebacc),
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
