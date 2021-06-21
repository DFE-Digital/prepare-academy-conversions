using System;
using System.Text.Json.Serialization;

namespace ApplyToBecome.Data.Models
{
	public class SchoolPerformance
	{
		public string PersonalDevelopment { get; set; }
		public string BehaviourAndAttitudes { get; set; }
		public string EarlyYearsProvision { get; set; }
		[JsonIgnore]
		public DateTime? OfstedLastInspection { get; set; }
		public string EffectivenessOfLeadershipAndManagement { get; set; }
		public string OverallEffectiveness { get; set; }
		public string QualityOfEducation { get; set; }
		public string SixthFormProvision { get; set; }
	}
}