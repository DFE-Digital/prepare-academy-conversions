using ApplyToBecomeInternal.Utils;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers
{
	public sealed class ProjectPage
	{
		private static string _pageHeader, _backLink, _schoolName;

		public static string BackLink => _backLink ??= Typespace.Name;
		public static string Heading => _pageHeader ??= Typespace.Name;

		public static string SchoolName => _schoolName ??= Typespace.Name;

		public sealed class TaskList
		{
			public sealed class Links
			{
				private static string _legalRequirements;
				public static string LegalRequirements => _legalRequirements ??= Typespace.Name;
			}

			public sealed class LegalRequirements
			{
				private static string _status;
				public static string Status => _status ??= Typespace.Name;
			}
		}

		public sealed class Legal
		{
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
}
