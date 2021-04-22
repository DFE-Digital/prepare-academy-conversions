using System.Globalization;

namespace ApplyToBecome.Data.Models.Application
{
	public class Money
	{
		public Money(decimal value)
		{
			Value = value;
		}

		public decimal Value { get; }

		public static implicit operator Money(decimal value) => new Money(value);
		public static implicit operator string(Money money) => money.ToString();

		public override string ToString() => Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-GB"));
	}
}