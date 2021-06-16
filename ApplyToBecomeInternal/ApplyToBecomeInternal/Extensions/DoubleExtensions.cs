using System.Globalization;

namespace ApplyToBecomeInternal.Extensions
{
	public static class DoubleExtensions
	{
		public static string ToMoneyString(this decimal value, bool includePoundSign = false)
		{
			return string.Format(CultureInfo.CreateSpecificCulture("en-GB"), includePoundSign ? "{0:C2}" : "{0:F2}", value);
		}
	}
}
