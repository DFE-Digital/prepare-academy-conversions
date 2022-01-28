using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class AboutConversionSection : BaseFormSection
	{
		public AboutConversionSection(SchoolApplication application) : base("About the conversion")
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
		private IEnumerable<FormField> GenerateSchoolFields(SchoolApplication application) =>
			new[]
			{
				new FormField("The name of the school", application.SchoolName)
			};
		private IEnumerable<FormField> GenerateContactFields(SchoolApplication application) =>
			new[]
			{
				new FormField("Name of headteacher", application.SchoolConversionContactHeadName),
				new FormField("Headteacher's email address", application.SchoolConversionContactHeadEmail),
				new FormField("Headteacher's telephone number", application.SchoolConversionContactHeadTel),
				new FormField("Name of the chair of the Governing Body", application.SchoolConversionContactChairName),
				new FormField("Chair's email address", application.SchoolConversionContactChairEmail),
				new FormField("Chair's phone number", application.SchoolConversionContactChairTel),
				new FormField("Approver's name", application.SchoolConversionApproverContactName),
				new FormField("Approver's email address", application.SchoolConversionApproverContactEmail)          
			};

		private IEnumerable<FormField> GenerateConversionDateFields(SchoolApplication application) =>
			new[]
			{
				new FormField("Do you want the conversion to happen on a particular date", application.SchoolConversionTargetDateSpecified.ToYesNoString()),
				new FormField("Preferred date", application.SchoolConversionTargetDate.Value.ToUkDateString())
			};

		private IEnumerable<FormField> GenerateReasonsForJoiningFields(SchoolApplication application) =>
			new[]
			{
				new FormField("Why does the school want to join this trust in particular?", application.SchoolConversionReasonsForJoining)
			};

		private IEnumerable<FormField> GenerateNameChangesFields(SchoolApplication application) =>
			new[]
			{
				 new FormField("Is the school planning to change its name when it becomes an academy?", application.SchoolConversionChangeNamePlanned.ToYesNoString())
			};
	}
}