using ApplyToBecomeInternal.Utils;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers
{
	public sealed class Legal
	{
		private static string _schoolName, _pageHeader, _backLink;

		public static string SchoolName => _schoolName ??= Typespace.Name;
		public static string PageHeader => _pageHeader ??= Typespace.Name;
		public static string BackLink => _backLink ??= Typespace.Name;

		public sealed class Input
		{
			private static string _yesOption, _noOption, _notApplicableOption;
			private static string _saveAndContinue;

			public static string Yes => _yesOption ??= Typespace.Name;
			public static string No => _noOption ??= Typespace.Name;
			public static string NotApplicable => _notApplicableOption ??= Typespace.Name;

			public static string SaveAndContinue => _saveAndContinue ??= Typespace.Name;
		}

		public sealed class Summary
		{
			private static string _isComplete, _submitButton;

			public static string IsComplete => _isComplete ??= Typespace.Name;
			public static string SubmitButton => _submitButton ??= Typespace.Name;

			public sealed class GoverningBody
			{
				private static string _status, _change;

				public static string Status => _status ??= Typespace.Name;
				public static string Change => _change ??= Typespace.Name;
			}

			public sealed class Consultation
			{
				private static string _status, _change;

				public static string Status => _status ??= Typespace.Name;
				public static string Change => _change ??= Typespace.Name;
			}

			public sealed class DiocesanConsent
			{
				private static string _status, _change;

				public static string Status => _status ??= Typespace.Name;
				public static string Change => _change ??= Typespace.Name;
			}

			public sealed class FoundationConsent
			{
				private static string _status, _change;

				public static string Status => _status ??= Typespace.Name;
				public static string Change => _change ??= Typespace.Name;
			}
		}
	}
}
