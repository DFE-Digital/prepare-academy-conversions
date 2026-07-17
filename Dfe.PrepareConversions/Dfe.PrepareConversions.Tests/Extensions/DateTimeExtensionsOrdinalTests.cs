using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions;

public class DateTimeExtensionsOrdinalTests
{
   [Theory]
   [InlineData(2026, 7, 1, "1st July 2026")]
   [InlineData(2026, 7, 2, "2nd July 2026")]
   [InlineData(2026, 7, 3, "3rd July 2026")]
   [InlineData(2026, 7, 4, "4th July 2026")]
   [InlineData(2026, 7, 11, "11th July 2026")]
   [InlineData(2026, 7, 12, "12th July 2026")]
   [InlineData(2026, 7, 13, "13th July 2026")]
   [InlineData(2026, 7, 21, "21st July 2026")]
   [InlineData(2026, 7, 22, "22nd July 2026")]
   [InlineData(2026, 7, 23, "23rd July 2026")]
   [InlineData(2026, 7, 31, "31st July 2026")]
   public void ToOrdinalDateString_formats_day_with_ordinal_suffix(int year, int month, int day, string expected)
   {
      new DateTime(year, month, day).ToOrdinalDateString().Should().Be(expected);
   }

   [Fact]
   public void ToOrdinalDateString_nullable_returns_empty_for_null()
   {
      ((DateTime?)null).ToOrdinalDateString().Should().BeEmpty();
   }

   [Fact]
   public void ToOrdinalDateString_nullable_formats_value()
   {
      ((DateTime?)new DateTime(2026, 7, 23)).ToOrdinalDateString().Should().Be("23rd July 2026");
   }
}