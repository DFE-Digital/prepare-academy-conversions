using Dfe.PrepareConversions.Services;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services;

public class DateValidationServiceTests
{
   private readonly DateValidationService _validator;

   public DateValidationServiceTests()
   {
      _validator = new DateValidationService(null);
   }

   [Theory]
   [InlineData("10", "10")]
   [InlineData("1", "1")]
   [InlineData("01", "01")]
   [InlineData("31", "12")]
   public void GivenValidDate_ReturnValid(string day, string month)
   {
      (bool valid, string _) = _validator.Validate(day, month, $"{DateTime.Today.Year}", "Date input");
      Assert.True(valid);
   }

   [Theory]
   [InlineData(null, "10", "2020", "Date input", "Date input must include a day")]
   [InlineData("10", null, "2020", "Input", "Input must include a month")]
   [InlineData("10", "10", null, "Input", "Input must include a year")]
   [InlineData(null, null, "2020", "Input", "Input must include a day and month")]
   [InlineData(null, "10", null, "Input", "Input must include a day and year")]
   [InlineData("10", null, null, "Input", "Input must include a month and year")]
   [InlineData(null, null, null, "Date Input", "Enter a date for the date input")]
   public void GivenDateWithMissingFields_ReturnInvalidWithCorrectErrorMessage(string day, string month, string year, string displayName, string expectedMessage)
   {
      (bool valid, string message) = _validator.Validate(day, month, year, displayName);
      Assert.False(valid);
      Assert.Equal(expectedMessage, message);
   }

   [Theory]
   [InlineData("0", "2022")]
   [InlineData("13", "2022")]
   [InlineData("-1", "2022")]
   public void Should_report_the_month_is_out_of_range(string month, string year)
   {
      (bool valid, string message) = _validator.Validate("1", month, year, "Input");

      valid.Should().BeFalse();
      message.Should().Be("Month must be between 1 and 12");
   }

   [Theory]
   [InlineData("1", "ABC")]
   [InlineData("ABC", "1")]
   public void Should_handle_non_numeric_data_specifically(string day, string month)
   {
      (bool valid, string message) = _validator.Validate(day, month, $"{DateTime.Today.Year}", "Input");

      valid.Should().BeFalse();
      message.Should().Be("Enter a date in the correct format");
   }

   [Theory]
   [InlineData("-1", "1", "2022", 31)]
   [InlineData("0", "2", "2022", 28)]
   [InlineData("32", "1", "2022", 31)]
   [InlineData("30", "2", "2020", 29)]
   public void Should_report_the_day_is_out_of_range_when_it_is_not_valid_for_the_month_and_year(string day, string month, string year, int expectedDays)
   {
      (bool valid, string message) = _validator.Validate(day, month, year, "Input");

      valid.Should().BeFalse();
      message.Should().Be($"Day must be between 1 and {expectedDays}");
   }

   [Fact]
   public void Should_return_month_range_when_providing_a_month_greater_than_twelve()
   {
      (bool valid, string message) = _validator.Validate("1", "99", "2022", "Input");

      valid.Should().BeFalse();
      message.Should().Be("Month must be between 1 and 12");
   }
}
