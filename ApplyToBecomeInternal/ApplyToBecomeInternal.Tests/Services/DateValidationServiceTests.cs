using ApplyToBecomeInternal.Services;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Services
{
	public class DateValidationServiceTests
	{
		private readonly DateValidationService _validator;

		public DateValidationServiceTests()
		{
			_validator = new DateValidationService();
		}

		[Theory]
		[InlineData("10", "10", "2020")]
		[InlineData("1", "1", "2020")]
		[InlineData("01", "01", "2020")]
		[InlineData("31", "12", "2020")]
		public void GivenValidDate_ReturnValid(string day, string month, string year)
		{
			(bool valid, string _) = _validator.Validate(day, month, year, "Date input");
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
		[InlineData("Abc", "10", "2020")]
		[InlineData("10", "Abc", "2020")]
		[InlineData("10", "10", "Abc")]
		[InlineData("Abc", "Abc", "2020")]
		[InlineData("Abc", "10", "Abc")]
		[InlineData("10", "Abc", "Abc")]
		[InlineData("Abc", "Abc", "Abc")]
		public void GivenDateWithNonNumericFields_ReturnInvalidWithCorrectErrorMessage(string day, string month, string year)
		{
			(bool valid, string message) = _validator.Validate(day, month, year, "Input");

			Assert.False(valid);
			Assert.Equal("Input date must be a real date", message);
		}

		[Theory]
		[InlineData("30", "2", "2020")]
		[InlineData("0", "2", "2020")]
		[InlineData("10", "0", "2020")]
		[InlineData("10", "2", "0")]
		[InlineData("2", "30", "2020")]
		[InlineData("-1", "02", "2020")]
		[InlineData("1", "02", "-1")]
		public void GivenNonRealDates_ReturnInvalidWithCorrectErrorMessage(string day, string month, string year)
		{
			(bool valid, string message) = _validator.Validate(day, month, year, "Input");

			Assert.False(valid);
			Assert.Equal("Input date must be a real date", message);
		}
	}
}