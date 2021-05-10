using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class ApplicationFormSection : BaseFormSection
	{
		public ApplicationFormSection(Application application) : base("School application Form", GenerateBaseFields(application))
		{
			SubSections = new[] { 
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateBaseFields(Application application) =>
			new[] {
				new FormField("Application to join", $"{application.Trust.Name} with {application.School.Name}"),
				new FormField("Lead applicant", application.LeadApplicant),
			};

		private static IEnumerable<FormField> GenerateDetailsFields(Application application) =>
			new[] {
				new LinkFormField("Upload evidence that the trust consents to the school joining", application.Details.EvidenceDocument),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", application.Details.ChangesToGovernance.ToYesNoString()),
				new FormField("Will there be any changes at a local level due to this school joining?", application.Details.ChangesAtLocalLevel.ToYesNoString()),
			};

	}
}