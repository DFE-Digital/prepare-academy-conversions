using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class AboutConversionSection : BaseFormSection
	{
		public AboutConversionSection(Application application) : base("About the conversion")
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
		private IEnumerable<FormField> GenerateSchoolFields(Application application) =>
			new[]
			{
				new FormField("The name of the school", application.School.Name)
			};
		private IEnumerable<FormField> GenerateContactFields(Application application) =>
			new[]
			{
				new FormField("Name of headteacher", application.ConversionInformation.HeadTeacher.Name),
				new FormField("Headteacher's email address", application.ConversionInformation.HeadTeacher.EmailAddress),
				new FormField("Headteacher's telephone number", application.ConversionInformation.HeadTeacher.TelephoneNumber),
				new FormField("Name of the chair of the Governing Body", application.ConversionInformation.GoverningBodyChair.Name),
				new FormField("Chair's email address", application.ConversionInformation.GoverningBodyChair.EmailAddress),
				new FormField("Chair's phone number", application.ConversionInformation.GoverningBodyChair.TelephoneNumber),
				new FormField("Approver's name", application.ConversionInformation.Approver.Name),
				new FormField("Approver's email address", application.ConversionInformation.Approver.EmailAddress)          
			};

		private IEnumerable<FormField> GenerateConversionDateFields(Application application) =>
			new[]
			{
				new FormField("Do you want the conversion to happen on a particular date", "Yes"),
				new FormField("Preferred date", application.ConversionInformation.DateForConversion.PreferredDate.ToUkDateString())
			};

		private IEnumerable<FormField> GenerateReasonsForJoiningFields(Application application) =>
			new[]
			{
				new FormField("Why does the school want to join this trust in particular?", application.ConversionInformation.SchoolToTrustRationale)
			};

		private IEnumerable<FormField> GenerateNameChangesFields(Application application) =>
			new[]
			{
				 new FormField("Is the school planning to change its name when it becomes an academy?", application.ConversionInformation.WillSchoolChangeName.ToYesNoString())
			};
	}
}