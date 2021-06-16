namespace ApplyToBecomeInternal.Extensions
{
	public static class IntegerExtensions
	{
		public static string AsPercentageOf(this int? part, int? whole)
		{
			if (!whole.HasValue || !part.HasValue)
			{
				return "";
			}
			return string.Format("{0:F0}%", (100d / whole) * part);
		}
	}
}
