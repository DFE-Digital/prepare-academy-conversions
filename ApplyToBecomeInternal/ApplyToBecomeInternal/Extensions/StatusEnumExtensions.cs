using ApplyToBecome.Data.Models.AcademyConversion;

namespace ApplyToBecomeInternal.Extensions
{
	public static class StatusEnumExtensions
	{
		public static string ToTagCssClass(this Status status)
		{
			return status switch
			{
				Status.NotStarted => "govuk-tag--grey",
				Status.InProgress => "govuk-tag--blue",
				Status.Complete => string.Empty,
				_ => "govuk-tag--grey"
			};
		}
	}
}
