using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class AboutConversionSection : BaseFormSection
	{
		public AboutConversionSection(ApplyingSchool application) : base("About the conversion")
		{
			SubSections = new[]
			{
				new FormSubSection("The school joining the trust", GenerateSchoolFields(application)),
				new FormSubSection("Contact details", GenerateContactFields(application)),
				new FormSubSection("Date for conversion", GenerateConversionDateFields(application)),
				new FormSubSection("Reasons for joining", GenerateReasonsForJoiningFields(application)),
				new FormSubSection("Name changes", GenerateNameChangesFields(application)),
			};
		}
		private static IEnumerable<FormField> GenerateSchoolFields(ApplyingSchool application) =>
			new[]
			{
				new FormField("The name of the school", application.SchoolName)
			};

		private static IEnumerable<FormField> GenerateContactFields(ApplyingSchool application)
		{
			var formFields = new List<FormField>();
			formFields.Add(new FormField("Name of headteacher", application.SchoolConversionContactHeadName));
			formFields.Add(new FormField("Headteacher's email address", application.SchoolConversionContactHeadEmail));
			formFields.Add(new FormField("Headteacher's phone number", application.SchoolConversionContactHeadTel));
			formFields.Add(new FormField("Name of the chair of the Governing Body", application.SchoolConversionContactChairName));
			formFields.Add(new FormField("Chair's email address", application.SchoolConversionContactChairEmail));
			formFields.Add(new FormField("Chair's phone number", application.SchoolConversionContactChairTel));
			formFields.Add(new FormField("Who is the main contact for the conversion?", application.SchoolConversionContactRole));
			if (application.SchoolConversionContactRole.ToUpper() == "OTHER")
			{
				formFields.Add(new FormField("Main contact's name", application.SchoolConversionMainContactOtherName));
				formFields.Add(new FormField("Main contact's email address", application.SchoolConversionMainContactOtherEmail));
				formFields.Add(new FormField("Main contact's phone number", application.SchoolConversionMainContactOtherTelephone));
				formFields.Add(new FormField("Main contact's role", application.SchoolConversionMainContactOtherRole));
			}
			formFields.Add(new FormField("Approver's name", application.SchoolConversionApproverContactName));
			formFields.Add(new FormField("Approver's email address", application.SchoolConversionApproverContactEmail));

			return formFields;
		}

		private static IEnumerable<FormField> GenerateConversionDateFields(ApplyingSchool application)
		{
			var formFields = new List<FormField>();
			formFields.Add(new FormField("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString()));
			if (application.SchoolConversionTargetDateSpecified == true)
			{
				formFields.Add(new FormField("Preferred date", application.SchoolConversionTargetDate.ToDateString()));
				formFields.Add(new FormField("Explain why you want to convert on this date", application.SchoolConversionTargetDateExplained));
			}

			return formFields;
		}

		private static IEnumerable<FormField> GenerateReasonsForJoiningFields(ApplyingSchool application) =>
			new[]
			{
				new FormField("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining)
			};

		private static IEnumerable<FormField> GenerateNameChangesFields(ApplyingSchool application)
		{
			var formFields = new List<FormField>();

			formFields.Add(new FormField("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString()));
			if (application.SchoolConversionChangeNamePlanned == true)
			{
				formFields.Add(new FormField("What's the proposed new name?", application.SchoolConversionProposedNewSchoolName));
			}

			return formFields;
		}
	}
}