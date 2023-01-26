using Dfe.PrepareConversions.Data.Models.AcademyConversion;

namespace Dfe.PrepareConversions.Extensions
{
	public static class StatusEnumExtensions
	{
		public static string ToTagCssClass(this Status status)
		{
			return status switch
			{
				Status.NotStarted => "govuk-tag--grey",
				Status.InProgress => "govuk-tag--blue",
				Status.Completed => string.Empty,
				_ => "govuk-tag--grey"
			};
		}
	}
}
