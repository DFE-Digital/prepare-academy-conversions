using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class ApplicationFormSection : BaseFormSection
	{
		public ApplicationFormSection(FullApplication application) : base("School application form", GenerateBaseFields(application))
		{
			SubSections = new[] { 
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateBaseFields(FullApplication application) =>
			new[] {
				new FormField("Application to join", $"{application.TrustName} with {application.SchoolApplication.SchoolName}"),
				new FormField("Lead applicant", application.ApplicationLeadAuthorName),
			};

		private static IEnumerable<FormField> GenerateDetailsFields(FullApplication application) =>
			new[] {
				new LinkFormField("Upload evidence that the trust consents to the school joining", application.EvidenceDocument),
				new FormField("Will there be any changes to the governance of the trust due to the school joining?", application.ChangesToTrust.ToYesNoString()),
				new FormField("Will there be any changes at a local level due to this school joining?", application.ChangesToLaGovernance.ToYesNoString()),
			};

	}
}