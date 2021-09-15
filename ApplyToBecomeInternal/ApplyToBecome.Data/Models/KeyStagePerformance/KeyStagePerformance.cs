using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecome.Data.Models.KeyStagePerformance
{
	public class KeyStagePerformance
	{
		public IEnumerable<KeyStage2PerformanceResponse> KeyStage2 { get; set; }
		public IEnumerable<KeyStage4PerformanceResponse> KeyStage4 { get; set; }
		public IEnumerable<KeyStage5PerformanceResponse> KeyStage5 { get; set; }

		public bool HasKeyStage2PerformanceTables => KeyStage2?.Any(HasKeyStage2Performance) ?? false;
		public bool HasKeyStage4PerformanceTables => KeyStage4?.Any(HasKeyStage4Performance) ?? false;
		public bool HasKeyStage5PerformanceTables => KeyStage5?.Any(HasKeyStage5Performance) ?? false;

		private bool HasKeyStage2Performance(KeyStage2PerformanceResponse keyStage2Performance)
		{
			return HasValue(keyStage2Performance.PercentageMeetingExpectedStdInRWM)
			       || HasValue(keyStage2Performance.PercentageAchievingHigherStdInRWM)
			       || HasValue(keyStage2Performance.ReadingProgressScore)
			       || HasValue(keyStage2Performance.WritingProgressScore)
			       || HasValue(keyStage2Performance.MathsProgressScore);
		}
		
		private bool HasKeyStage4Performance(KeyStage4PerformanceResponse keyStage4Performance)
		{
			return HasValue(keyStage4Performance.SipAttainment8score)
			       || HasValue(keyStage4Performance.SipAttainment8scoreenglish)
			       || HasValue(keyStage4Performance.SipAttainment8scoremaths)
			       || HasValue(keyStage4Performance.SipAttainment8scoreebacc)
			       || HasValue(keyStage4Performance.SipNumberofpupilsprogress8)
			       || HasValue(keyStage4Performance.SipProgress8Score)
			       || HasValue(keyStage4Performance.SipProgress8english)
			       || HasValue(keyStage4Performance.SipProgress8maths)
			       || HasValue(keyStage4Performance.SipProgress8ebacc);
		}

		private bool HasKeyStage5Performance(KeyStage5PerformanceResponse keyStage5Performance)
		{
			return keyStage5Performance.AcademicQualificationAverage != null
			       || keyStage5Performance.AppliedGeneralQualificationAverage != null;
		}
		
		private bool HasValue(DisadvantagedPupilsResponse disadvantagedPupilsResponse)
		{
			return !string.IsNullOrEmpty(disadvantagedPupilsResponse.NotDisadvantaged)
				|| !string.IsNullOrEmpty(disadvantagedPupilsResponse.Disadvantaged);
		}
	}
}