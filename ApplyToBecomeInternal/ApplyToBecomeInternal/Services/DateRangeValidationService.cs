using System;

namespace ApplyToBecomeInternal.Services
{
	public class DateRangeValidationService
	{
		public (bool, string) Validate(DateTime date, DateRange dateRange, string displayName)
		{
			switch (dateRange)
			{
				case DateRange.Past:
					if (date >= DateTime.Today)
					{
						return (false, $"{displayName} date must be in the past");
					}

					break;
				
				case DateRange.PastOrToday:
					if (date > DateTime.Today)
					{
						return (false, $"{displayName} date must be today or in the past");
					}

					break;
				
				case DateRange.Future:
					if (date <= DateTime.Today)
					{
						return (false, $"{displayName} date must be in the future");
					}

					break;
					
				case DateRange.FutureOrToday:
					if (date < DateTime.Today)
					{
						return (false, $"{displayName} date must be today or in the future");
					}
					break;
				case DateRange.PastOrFuture:
					return (true, "");
			}

			return (true, "");
		}

		public enum DateRange
		{
			Past,
			PastOrToday,
			Future,
			FutureOrToday,
			PastOrFuture
		}
	}
}