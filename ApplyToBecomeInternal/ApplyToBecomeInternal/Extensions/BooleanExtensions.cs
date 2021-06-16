namespace ApplyToBecomeInternal.Extensions
{
	public static class BooleanExtensions
	{
		public static string ToYesNoString(this bool value) => value ? "Yes" : "No";
		public static string ToYesNoString(this bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "";
	}
}