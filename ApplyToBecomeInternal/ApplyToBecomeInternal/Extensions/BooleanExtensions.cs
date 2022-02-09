namespace ApplyToBecomeInternal.Extensions
{
	public static class BooleanExtensions
	{
		public static string ToYesNoString(this bool value) => value ? "Yes" : "No";
		public static string ToYesNoString(this bool? value) => value.HasValue ? value.Value ? "Yes" : "No" : "";
	}

	public static class IntExtensions // CML put class into its own file if we need these, but expecting to delete
	{
		public static string ToYesNoString(this int value) => $"{value} needs conversion to Y/N";
		public static string ToYesNoString(this int? value) => $"{value} needs conversion to Y/N";
		public static string ToSurplusDeficitString(this int value) => $"{value} needs conversion to surplus/deficit";
		public static string ToSurplusDeficitString(this int? value) => $"{value} needs conversion to surplus/deficit";
		//public static string ToSchoolOrTrust(this int value) => $"{value} needs conversion to school/trust";
		//public static string ToSchoolOrTrust(this int? value) => $"{value} needs conversion to school/trust";
	}
}