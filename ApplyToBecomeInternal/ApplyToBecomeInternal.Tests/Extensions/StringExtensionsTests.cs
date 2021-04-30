using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Extensions
{
	public class StringExtensionsTests
	{
		[Theory]
		[InlineData("abbba", "b", 1, 2, 3)]
		[InlineData("wrgws", "w", 0, 3)]
		[InlineData("dgjhdftyhsdrtgat", "t", 6, 12, 15)]
		[InlineData("", "t")]
		public void AllIndicesOf_WithStrings_ReturnsIndices(string input, string value, params int[] indices) =>
			input.AllIndicesOf(value).Should().BeEquivalentTo(indices);
	}
}