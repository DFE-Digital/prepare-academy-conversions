using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Extensions
{
	public class BooleanExtensionsTests
	{
		[Fact]
		public void ToYesNoString_WithFalse_ReturnsNo() => false.ToYesNoString().Should().Be("No");
		
		[Fact]
		public void ToYesNoString_WithTrue_ReturnsYes() => true.ToYesNoString().Should().Be("Yes");
	}
}