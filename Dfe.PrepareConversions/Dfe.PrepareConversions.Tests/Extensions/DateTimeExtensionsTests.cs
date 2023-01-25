using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions
{
	public class DateTimeExtensionsTests
	{
		[Theory]
		[InlineData(2020, 12, 31, "31/12/2020")]
		[InlineData(1689, 4, 3, "03/04/1689")]
		public void ToUkDateString_ReturnsFormattedDate(int year, int month, int day, string output) => new DateTime(year, month, day).ToUkDateString().Should().Be(output);

		[Theory]
		[InlineData(2020, 12, 31, false, "31 December 2020")]
		[InlineData(1689, 4, 3, false, "3 April 1689")]
		[InlineData(2021, 11, 1, true, "Monday 1 November 2021")]
		[InlineData(2020, 2, 29, true, "Saturday 29 February 2020")]
		public void ToDateString_ReturnsFormattedDate(int year, int month, int day, bool includeDayOfWeek, string output) => new DateTime(year, month, day).ToDateString(includeDayOfWeek).Should().Be(output);

		[Theory]
		[MemberData(nameof(DataForFirstOfMonthTests))]
		public void FirstOfMonth_ReturnsCorrectDateTime(DateTime input, int monthsToAdd, DateTime expected)
		{
			input.FirstOfMonth(monthsToAdd).Should().Be(expected);
		}

		public static IEnumerable<object[]> DataForFirstOfMonthTests =>
			new List<object[]>
		{
			new object[] { new DateTime(2021, 10, 31), 6, new DateTime(2022, 4, 1) },
			new object[] { new DateTime(2021, 1, 1), 0, new DateTime(2021, 1, 1) },
			new object[] { new DateTime(2020, 2, 29), 12, new DateTime(2021, 2, 1) },
			new object[] { new DateTime(2020, 2, 29), -1, new DateTime(2020, 1, 1) }
		};
	}
}