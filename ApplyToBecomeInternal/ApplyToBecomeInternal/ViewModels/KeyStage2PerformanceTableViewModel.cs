using Microsoft.AspNetCore.Html;

namespace ApplyToBecomeInternal.ViewModels
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
	}
}
