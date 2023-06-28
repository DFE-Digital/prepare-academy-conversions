using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions;

public class DisplayExtensionsTests
{
   [Theory]
   [InlineData("2017-2018", "2017 to 2018")]
   [InlineData("2016 - 2017", "2016 to 2017")]
   [InlineData("2015-  2016", "2015 to 2016")]
   [InlineData("2014   -2015", "2014 to 2015")]
   [InlineData("2014", "2014")]
   public void Should_format_key_stage_date_correctly(string year, string expectedFormattedYear)
   {
      string formattedYear = year.FormatKeyStageYear();
      formattedYear.Should().Be(expectedFormattedYear);
   }

   [Theory]
   [InlineData("202.212", "202.212")]
   [InlineData("105", "105")]
   [InlineData("105.0", "105")]
   [InlineData("0.0", "0")]
   [InlineData("2.99999999999", "2.99999999999")]
   public void Should_format_as_double_correctly(string value, string expectedValue)
   {
      string valueFormattedAsDouble = value.FormatAsDouble();
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

   [Theory]
   [InlineData("No data", false)]
   [InlineData("something else", true)]
   public void Should_return_correctly_if_data(string value, bool expectedValue)
   {
      value.HasData().Should().Be(expectedValue);
   }

   [Theory]
   [InlineData(null, null)]
   [InlineData("", "")]
   public void Should_HtmlFormatKeyStageDisadvantagedResult_returns_no_data(string disadantaged, string notDisadvantaged)
   {
      HtmlString response = DisplayExtensions.HtmlFormatKeyStageDisadvantagedResult(new DisadvantagedPupilsResponse
      {
         Disadvantaged = disadantaged, NotDisadvantaged = notDisadvantaged
      });

      response.Value.Should().Be("No data");
   }

   private static void AssertValueConvertedCorrectly(decimal? value, string expectedValue)
   {
      string decimalAsString = value.ToSafeString();
      decimalAsString.Should().Be(expectedValue);
   }
}
