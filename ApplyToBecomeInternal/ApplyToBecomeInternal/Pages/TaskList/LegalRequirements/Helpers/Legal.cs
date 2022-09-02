using ApplyToBecomeInternal.Utils;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers
{
	public sealed class Legal
	{
		public static string SchoolName => Typespace.Name;
		public static string PageHeader => Typespace.Name;
		public static string BackLink => Typespace.Name;

		public sealed class Summary
		{
			public static string IsComplete => Typespace.Name;
			public static string SubmitButton => Typespace.Name;

			public sealed class GoverningBody
			{
				public static string Status => Typespace.Name;
				public static string Change => Typespace.Name;
			}

			public sealed class Consultation
			{
				public static string Status => Typespace.Name;
				public static string Change => Typespace.Name;
			}

			public sealed class DiocesanConsent
			{
				public static string Status => Typespace.Name;
				public static string Change => Typespace.Name;
			}

			public sealed class FoundationConsent
			{
				public static string Status => Typespace.Name;
				public static string Change => Typespace.Name;
			}
		}
	}
}
