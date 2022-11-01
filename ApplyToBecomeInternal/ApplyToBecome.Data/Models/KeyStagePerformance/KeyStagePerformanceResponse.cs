using System.Collections.Generic;

namespace ApplyToBecome.Data.Models.KeyStagePerformance
{
	public class KeyStagePerformanceResponse
	{
		public string SchoolName { get; set; }
		public IEnumerable<KeyStage2PerformanceResponse> KeyStage2 { get; set; }
		public IEnumerable<KeyStage4PerformanceResponse> KeyStage4 { get; set; }
		public IEnumerable<KeyStage5PerformanceResponse> KeyStage5 { get; set; }
	}
}
