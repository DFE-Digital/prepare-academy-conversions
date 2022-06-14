namespace ApplyToBecomeInternal.Extensions
{
	public static class BooleanExtensions
	{
		public static string ToYesNoString(this bool value) => value ? "Yes" : "No";
		public static string ToYesNoString(this bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "";
		public static string ToSurplusDeficitString(this bool value) => value ? "Deficit" : "Surplus";
		public static string ToSurplusDeficitString(this bool? value) => value.HasValue ? value.Value ? "Deficit" : "Surplus" : "";
	}
}