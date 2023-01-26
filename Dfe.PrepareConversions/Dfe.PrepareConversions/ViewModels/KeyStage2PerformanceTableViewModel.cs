using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Extensions;
using static Dfe.PrepareConversions.Extensions.DisplayExtensions;

using Microsoft.AspNetCore.Html;

namespace Dfe.PrepareConversions.ViewModels
{
	public class KeyStage2PerformanceTableViewModel
	{
		public string Year { get; set; }
		public string PercentageMeetingExpectedStdInRWM { get; set; }
		public string PercentageAchievingHigherStdInRWM { get; set; }
		public string ReadingProgressScore { get; set; }
		public string WritingProgressScore { get; set; }
		public string MathsProgressScore { get; set; }
		public HtmlString NationalAveragePercentageMeetingExpectedStdInRWM { get; set; }
		public HtmlString NationalAveragePercentageAchievingHigherStdInRWM { get; set; }
		public string NationalAverageReadingProgressScore { get; set; }
		public string NationalAverageWritingProgressScore { get; set; }
		public string NationalAverageMathsProgressScore { get; set; }
		public string LAAveragePercentageMeetingExpectedStdInRWM { get; set; }
		public string LAAveragePercentageAchievingHigherStdInRWM { get; set; }
		public string LAAverageReadingProgressScore { get; set; }
		public string LAAverageWritingProgressScore { get; set; }
		public string LAAverageMathsProgressScore { get; set; }

		public static KeyStage2PerformanceTableViewModel Build(KeyStage2PerformanceResponse keyStage2Performance)
		{
			return new KeyStage2PerformanceTableViewModel
			{
				Year = keyStage2Performance.Year.FormatKeyStageYear(),
				PercentageMeetingExpectedStdInRWM = keyStage2Performance.PercentageMeetingExpectedStdInRWM.NotDisadvantaged.FormatValue(),
				PercentageAchievingHigherStdInRWM = keyStage2Performance.PercentageAchievingHigherStdInRWM.NotDisadvantaged.FormatValue(),
				ReadingProgressScore = keyStage2Performance.ReadingProgressScore.NotDisadvantaged.FormatValue(),
				WritingProgressScore = keyStage2Performance.WritingProgressScore.NotDisadvantaged.FormatValue(),
				MathsProgressScore = keyStage2Performance.MathsProgressScore.NotDisadvantaged.FormatValue(),
				NationalAveragePercentageMeetingExpectedStdInRWM = HtmlFormatKeyStageDisadvantagedResult(keyStage2Performance.NationalAveragePercentageMeetingExpectedStdInRWM),
				NationalAveragePercentageAchievingHigherStdInRWM = HtmlFormatKeyStageDisadvantagedResult(keyStage2Performance.NationalAveragePercentageAchievingHigherStdInRWM),
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
