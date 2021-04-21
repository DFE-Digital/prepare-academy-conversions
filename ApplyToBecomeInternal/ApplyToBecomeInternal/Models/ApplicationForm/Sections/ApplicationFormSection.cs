using ApplyToBecome.Data.Models;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class ApplicationFormSection : BaseFormSection
	{
		public ApplicationFormSection(Application application) : base("Application Form", GenerateBaseFields(application))
		{
			SubSections = new[] { 
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateBaseFields(Application application) =>
			new[] {
				new FormField("Application to join", application.GetField("")),
				new FormField("Lead applicant", application.GetField("")),
			};

		private static IEnumerable<FormField> GenerateDetailsFields(Application application) =>
			new[] {
				new FormField("Upload evidence that the trust consents to the school joining", application.GetField("")),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", application.GetField("")),
				new FormField("Will there be any changes at a local level due to this school joining?", application.GetField("")),
			};

	}
}