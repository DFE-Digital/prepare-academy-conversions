using Dfe.PrepareConversions.Data.Models.Application;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests;

public class MoneyTests
{
   [Theory]
   [InlineData(123, "£123.00")]
   [InlineData(123456, "£123,456.00")]
   [InlineData(123456.78, "£123,456.78")]
   [InlineData(0.78, "£0.78")]
   [InlineData(0.00, "£0.00")]
   public void ToString_ReturnsFormattedCurrencyString(decimal input, string expected)
   {
      Money money = new(input);
      money.ToString().Should().Be(expected);
   }

   [Theory]
   [InlineData(123)]
   [InlineData(123456)]
   [InlineData(123456.78)]
   [InlineData(0.78)]
   [InlineData(0.00)]
   public void ImplicitOperatorForAssignmentFromDecimal_WithDecimal_ReturnsMoneyWithValue(decimal input)
   {
      Money money = input;
      money.Value.Should().Be(input);
   }

   [Theory]
   [InlineData(123, "£123.00")]
   [InlineData(123456, "£123,456.00")]
   [InlineData(123456.78, "£123,456.78")]
   [InlineData(0.78, "£0.78")]
   [InlineData(0.00, "£0.00")]
   public void ImplicitOperatorForAssignmentToString_WithDecimal_ReturnsMoneyWithValue(decimal input, string output)
   {
      Money money = new(input);
      money.ToString().Should().Be(output);
   }
}
