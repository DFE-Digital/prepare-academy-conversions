using ApplyToBecomeInternal.Utils;

namespace ApplyToBecomeInternal.Models
{
	public static class Select
	{
		private static string _pageHeader, _backLink, _schoolName;

		public static string BackLink => Typespace.Name(ref _backLink);
		public static string Heading => Typespace.Name(ref _pageHeader);

		public static string SchoolName => Typespace.Name(ref _schoolName);

		public static class Common
		{
			private static string _submitButton;
			public static string SubmitButton => Typespace.Name(ref _submitButton);
		}

		public static class TaskList
		{
			public sealed class Links
			{
				private static string _legalRequirements;
				public static string LegalRequirementLinks => Typespace.Name(ref _legalRequirements);
			}

			public static class LegalRequirements
			{
				private static string _status;
				public static string Status => Typespace.Name(ref _status);
			}
		}

		public static class ProjectType
		{
			public static class Input
			{
				private static string _conversionOption, _transferOption;

				public static string Conversion => Typespace.Name(ref _conversionOption);
				public static string Transfer => Typespace.Name(ref _transferOption);

			}

		}

		public static class Legal
		{
			public static class Input
			{
				private static string _yesOption, _noOption, _notApplicableOption;

				public static string Yes => Typespace.Name(ref _yesOption);
				public static string No => Typespace.Name(ref _noOption);
				public static string NotApplicable => Typespace.Name(ref _notApplicableOption);

			}

			public static class Summary
			{
				private static string _isComplete, _submitButton;

				public static string IsComplete => Typespace.Name(ref _isComplete);
				public static string SubmitButton => Typespace.Name(ref _submitButton);

				public static class GoverningBody
				{
					private static string _status, _change;

					public static string Status => Typespace.Name(ref _status);
					public static string Change => Typespace.Name(ref _change);
				}

				public static class Consultation
				{
					private static string _status, _change;

					public static string Status => Typespace.Name(ref _status);
					public static string Change => Typespace.Name(ref _change);
				}

				public static class DiocesanConsent
				{
					private static string _status, _change;

					public static string Status => Typespace.Name(ref _status);
					public static string Change => Typespace.Name(ref _change);
				}

				public static class FoundationConsent
				{
					private static string _status, _change;

					public static string Status => Typespace.Name(ref _status);
					public static string Change => Typespace.Name(ref _change);
				}
			}
		}

		public static class ProjectList
		{
			public static class Filter
			{
				private static string _expand, _apply, _clear, _banner, _count, _row, _options, _title;

				public static string Status(string suffix = null) => Typespace.Name(suffix);
				public static string Officer(string suffix = null) => Typespace.Name(suffix);

				public static string Banner => Typespace.Name(ref _banner);
				public static string Count => Typespace.Name(ref _count);
				public static string Expand => Typespace.Name(ref _expand);
				public static string Apply => Typespace.Name(ref _apply);
				public static string Clear => Typespace.Name(ref _clear);
				public static string Row => Typespace.Name(ref _row);
				public static string Options => Typespace.Name(ref _options);
				public static string Title => Typespace.Name(ref _title);
			}
		}
	}
}
