namespace Dfe.PrepareConversions.Data.Models.KeyStagePerformance
{
	public class KeyStage5PerformanceResponse
	{
		public string Year { get; set; }
		public decimal? AcademicQualificationAverage { get; set; }
		public decimal? AppliedGeneralQualificationAverage { get; set; }
		public decimal? NationalAcademicQualificationAverage { get; set; }
		public decimal? NationalAppliedGeneralQualificationAverage { get; set; }
		public decimal? LAAcademicQualificationAverage { get; set; }
		public decimal? LAAppliedGeneralQualificationAverage { get; set; }
		public DisadvantagedPupilsResponse AppliedGeneralProgress { get; set; }
		public DisadvantagedPupilsResponse AcademicProgress { get; set; }
	}
}
