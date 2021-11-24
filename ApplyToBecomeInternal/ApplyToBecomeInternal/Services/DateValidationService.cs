using System;
using System.Collections.Generic;
using System.Globalization;

namespace ApplyToBecomeInternal.Services
{
	public class DateValidationService
	{
		private readonly List<string> _missingParts = new List<string>();

		public (bool, string) Validate(string dayInput, string monthInput, string yearInput, string displayName)
		{
			if (string.IsNullOrWhiteSpace(dayInput)) _missingParts.Add("day");
			if (string.IsNullOrWhiteSpace(monthInput)) _missingParts.Add("month");
			if (string.IsNullOrWhiteSpace(yearInput)) _missingParts.Add("year");

			if (_missingParts.Count == 3)
			{
				return (false, $"Enter a date for the {displayName.ToLower()}");
			}

			if (_missingParts.Count > 0)
			{
				return (false, $"{displayName} must include a {string.Join(" and ", _missingParts)}");
			}

			var validDate = DateTime.TryParseExact($"{yearInput}-{monthInput}-{dayInput}", "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate);

			if (!validDate)
			{
				return (false, $"{displayName} date must be a real date");
			}

			return (true, "");
		}
	}
}