using System.Collections.Generic;

namespace ApplyToBecomeInternal.Services
{
	public interface IDateValidationMessageProvider
	{
		string DefaultMessage => "Enter a date in the correct format";
		string MonthOutOfRange => "Month must be between 1 and 12";

		string AllMissing(string displayName);

		string SomeMissing(string displayName, IEnumerable<string> missingParts) =>
			$"{displayName} must include a {string.Join(" and ", missingParts)}";

		string DayOutOfRange(int daysInMonth) =>
			$"Day must be between 1 and {daysInMonth}";

		(bool, string) ContextSpecificValidation(int day, int month, int year) =>
			(true, string.Empty);
	}
}