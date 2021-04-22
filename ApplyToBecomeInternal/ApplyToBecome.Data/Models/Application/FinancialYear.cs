using System;

namespace ApplyToBecome.Data.Models.Application
{
	public class FinancialYear
	{
		public FinancialYear(DateTime date, Money value, FinancialYearState state)
		{
			Date = date;
			Value = value;
			State = state;
		}
		
		public DateTime Date { get; }
		public Money Value { get; }
		public FinancialYearState State { get; }
	}

	public class ForecastFinancialYear : FinancialYear
	{
		public ForecastFinancialYear(DateTime date, Money value, FinancialYearState state, Money carryForward, FinancialYearState carryForwardState) : base(date, value, state)
		{
			CarryForward = carryForward;
			CarryForwardState = carryForwardState;
		}
		
		public Money CarryForward { get; }
		public FinancialYearState CarryForwardState { get; }
	}
}