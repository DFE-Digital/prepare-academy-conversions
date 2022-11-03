namespace ApplyToBecomeInternal.Extensions
{
	public static class BooleanExtensions
	{
		public static string ToYesNoString(this bool value) => value ? "Yes" : "No";
		public static string ToYesNoString(this bool? value) 
		{
			if (value.HasValue) return value.Value ? "Yes" : "No";

			return "";
		}

		public static string ToSurplusDeficitString(this bool value) => value ? "Deficit" : "Surplus";
		public static string ToSurplusDeficitString(this bool? value) 
		{
			if (value.HasValue) return value.Value ? "Deficit" : "Surplus";
			
			return "";
		}
	}
}