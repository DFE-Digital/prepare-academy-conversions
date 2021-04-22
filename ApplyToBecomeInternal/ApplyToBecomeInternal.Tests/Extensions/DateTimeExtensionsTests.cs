using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Extensions
{
	public class DateTimeExtensionsTests
	{
		[Theory]
		[InlineData(2020, 12, 31, "31/12/2020")]
		[InlineData(1689, 4, 3, "03/04/1689")]
		public void ToUkDateString_ReturnsFormattedDate(int year, int month, int day, string output) => new DateTime(year, month, day).ToUkDateString().Should().Be(output);
	}
}