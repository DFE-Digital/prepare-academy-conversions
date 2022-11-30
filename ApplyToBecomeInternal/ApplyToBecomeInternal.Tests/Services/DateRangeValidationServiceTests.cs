using ApplyToBecomeInternal.Services;
using System;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Services
{
	public class DateRangeValidationServiceTests
	{
		private readonly DateRangeValidationService _validator;
		private static DateTime PastDate => DateTime.Today.AddMonths(-1);
		private static DateTime FutureDate => DateTime.Today.AddMonths(1);

		public DateRangeValidationServiceTests()
		{
			_validator = new DateRangeValidationService();
		}

		public class Past : DateRangeValidationServiceTests
		{
			[Fact]
			public void GivenDateInThePast_ReturnTrue()
			{
				var result = _validator.Validate(PastDate, DateRangeValidationService.DateRange.Past, "Input");

				Assert.True(result.Item1);
			}

			[Fact]
			public void GivenTodaysDate_ReturnFalse()
			{
				(bool valid, string message) = _validator.Validate(DateTime.Today, DateRangeValidationService.DateRange.Past, "Input");

				Assert.False(valid);
				Assert.Equal("Input date must be in the past", message);
			}

			[Fact]
			public void GivenFutureDate_ReturnFalse()
			{
				(bool valid, string message) = _validator.Validate(FutureDate, DateRangeValidationService.DateRange.Past, "Input");

				Assert.False(valid);
				Assert.Equal("Input date must be in the past", message);
			}
		}

		public class PastOrToday : DateRangeValidationServiceTests
		{
			[Fact]
			public void GivenDateInThePast_ReturnValid()
			{
				var result = _validator.Validate(PastDate, DateRangeValidationService.DateRange.PastOrToday, "Past or today");

				Assert.True(result.Item1);
			}

			[Fact]
			public void GivenTodaysDate_ReturnValid()
			{
				var result = _validator.Validate(DateTime.Today, DateRangeValidationService.DateRange.PastOrToday, "Past or today");

				Assert.True(result.Item1);
			}

			[Fact]
			public void GivenFutureDate_ReturnFalse()
			{
				(bool valid, string message) = _validator.Validate(FutureDate, DateRangeValidationService.DateRange.PastOrToday, "Past or today");

				Assert.False(valid);
				Assert.Equal("Past or today date must be today or in the past", message);
			}
		}

		public class Future : DateRangeValidationServiceTests
		{
			[Fact]
			public void GivenDateInThePast_ReturnNotValid()
			{
				(bool valid, string message) = _validator.Validate(PastDate, DateRangeValidationService.DateRange.Future, "Future");

				Assert.False(valid);
				Assert.Equal("Future date must be in the future", message);
			}

			[Fact]
			public void GivenTodaysDate_ReturnNotValid()
			{
				(bool valid, string message) = _validator.Validate(DateTime.Today, DateRangeValidationService.DateRange.Future, "Future");

				Assert.False(valid);
				Assert.Equal("Future date must be in the future", message);
			}

			[Fact]
			public void GivenFutureDate_ReturnValid()
			{
				(bool valid, string _) = _validator.Validate(FutureDate, DateRangeValidationService.DateRange.Future, "Future");

				Assert.True(valid);
			}
		}
		
		public class FutureOrToday : DateRangeValidationServiceTests
		{
			[Fact]
			public void GivenDateInThePast_ReturnNotValid()
			{
				(bool valid, string message) = _validator.Validate(PastDate, DateRangeValidationService.DateRange.FutureOrToday, "Future");

				Assert.False(valid);
				Assert.Equal("Future date must be today or in the future", message);
			}

			[Fact]
			public void GivenTodaysDate_ReturnValid()
			{
				(bool valid, string _) = _validator.Validate(DateTime.Today, DateRangeValidationService.DateRange.FutureOrToday, "Future");

				Assert.True(valid);
			}

			[Fact]
			public void GivenFutureDate_ReturnValid()
			{
				(bool valid, string _) = _validator.Validate(FutureDate, DateRangeValidationService.DateRange.FutureOrToday, "Future");

				Assert.True(valid);
			}
		}
	}
}