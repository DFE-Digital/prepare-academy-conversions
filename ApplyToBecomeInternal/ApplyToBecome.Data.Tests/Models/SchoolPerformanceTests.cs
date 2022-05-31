
using ApplyToBecome.Data.Models;
using FluentAssertions;
using System;
using Xunit;

namespace ApplyToBecome.Data.Tests.Models
{
	public class SchoolPerformanceTests
	{
		[Theory]
		[InlineData(null, null, false)]
		[InlineData(null, "12/05/2016", false)]
		[InlineData("12/05/2016", null, true)]
		[InlineData("12/05/2016", "12/05/2016", false)]
		[InlineData("11/05/2016", "12/05/2016", false)]
		[InlineData("12/05/2016", "11/05/2016", true)]
		public void LatestInspectionIsSection8_ReturnsCorrectly(string latestSection8, string latestFull, bool expectedValue)
		{
			var latestOfstedJudgement = new SchoolPerformance
			{
				DateOfLatestSection8Inspection = AssignDate(latestSection8),
				InspectionEndDate = AssignDate(latestFull)
			};

			latestOfstedJudgement.LatestInspectionIsSection8.Should().Be(expectedValue);
		}

		private static DateTime? AssignDate(string date)
		{
			if (date != null)
			{
				return DateTime.Parse(date);
			}

			return null;
		}
	}
}
