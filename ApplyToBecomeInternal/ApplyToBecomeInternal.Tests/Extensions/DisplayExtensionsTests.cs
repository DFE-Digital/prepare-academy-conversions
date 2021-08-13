using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Extensions
{
	public class DisplayExtensionsTests
	{
		[Theory]
		[InlineData("2017-2018","2017 to 2018")]
		[InlineData("2016 - 2017","2016 to 2017")]
		[InlineData("2015-  2016","2015 to 2016")]
		[InlineData("2014   -2015","2014 to 2015")]
		public void Should_format_key_stage_date_correctly(string year, string expectedFormattedYear)
		{
			var formattedYear = year.FormatKeyStageYear();
			formattedYear.Should().Be(expectedFormattedYear);
		}

		[Theory]
		[InlineData("202.212","202.212")]
		[InlineData("105","105")]
		[InlineData("105.0","105")]
		[InlineData("0.0","0")]
		[InlineData("2.99999999999","2.99999999999")]
		public void Should_format_as_double_correctly(string value, string expectedValue)
		{
			var valueFormattedAsDouble = value.FormatAsDouble();
			valueFormattedAsDouble.Should().Be(expectedValue);
		}

		[Fact]
		public void Should_convert_decimal_to_string_correctly()
		{
			AssertValueConvertedCorrectly(202.212m, "202.212");
			AssertValueConvertedCorrectly(105m, "105");
			AssertValueConvertedCorrectly(105.0m, "105");
			AssertValueConvertedCorrectly(0.0m, "0");
			AssertValueConvertedCorrectly(0.9839m, "0.9839");
			AssertValueConvertedCorrectly(2.99999999999m, "2.99999999999");
		}

		private static void AssertValueConvertedCorrectly(decimal? value, string expectedValue)
		{
			var decimalAsString = value.ToSafeString();
			decimalAsString.Should().Be(expectedValue);
		}
	}
}
