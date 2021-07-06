namespace ApplyToBecome.Data.Models.KeyStagePerformance
{
	public class KeyStage2PerformanceResponse
	{
		// add in local auth response - look at api response format
		public string Year { get; set; }
		public DisadvantagedPupilsResponse PercentageMeetingExpectedStdInRWM { get; set; }
		public DisadvantagedPupilsResponse PercentageAchievingHigherStdInRWM { get; set; }
		public DisadvantagedPupilsResponse ReadingProgressScore { get; set; }
		public DisadvantagedPupilsResponse WritingProgressScore { get; set; }
		public DisadvantagedPupilsResponse MathsProgressScore { get; set; }
		public DisadvantagedPupilsResponse NationalAveragePercentageMeetingExpectedStdInRWM { get; set; }
		public DisadvantagedPupilsResponse NationalAveragePercentageAchievingHigherStdInRWM { get; set; }
		public DisadvantagedPupilsResponse LAAveragePercentageMeetingExpectedStdInRWM { get; set; }
		public DisadvantagedPupilsResponse LAAveragePercentageAchievingHigherStdInRWM { get; set; }
		public DisadvantagedPupilsResponse LAAverageReadingProgressScore { get; set; }
		public DisadvantagedPupilsResponse LAAverageWritingProgressScore { get; set; }
		public DisadvantagedPupilsResponse LAAverageMathsProgressScore { get; set; }
	}
}
