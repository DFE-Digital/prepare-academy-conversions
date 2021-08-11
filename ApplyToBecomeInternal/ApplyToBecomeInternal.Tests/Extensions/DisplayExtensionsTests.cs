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
	}
}
