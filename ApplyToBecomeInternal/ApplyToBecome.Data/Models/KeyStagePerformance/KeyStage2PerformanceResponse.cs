namespace ApplyToBecome.Data.Models.KeyStagePerformance
{
	public class KeyStage2PerformanceResponse
	{
		public string Year { get; set; }
		public DisadvantagedPupilsResponse PercentageMeetingExpectedStdInRWM { get; set; }
		public DisadvantagedPupilsResponse PercentageAchievingHigherStdInRWM { get; set; }
		public DisadvantagedPupilsResponse ReadingProgressScore { get; set; }
		public DisadvantagedPupilsResponse WritingProgressScore { get; set; }
		public DisadvantagedPupilsResponse MathsProgressScore { get; set; }
		public DisadvantagedPupilsResponse NationalAveragePercentageMeetingExpectedStdInRWM { get; set; }
		public DisadvantagedPupilsResponse NationalAveragePercentageAchievingHigherStdInRWM { get; set; }
	}
}
