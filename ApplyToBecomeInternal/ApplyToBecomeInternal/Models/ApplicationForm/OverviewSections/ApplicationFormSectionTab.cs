using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models.ApplicationForm.OverviewSections
{
	public class ApplicationFormSectionTab : BaseFormSection
	{
		public ApplicationFormSectionTab(Application application) : base("Overview", GenerateBaseFields(application))
		{
			SubSections = new[] {
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateBaseFields(Application application) =>
			new[] {
				new FormField("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
				new FormField("Lead applicant", application.ApplicationLeadAuthorName),
			};

		private static IEnumerable<FormField> GenerateDetailsFields(Application application)
		{
			var formFields = new List<FormField> {
				new LinkFormField("Upload evidence that the trust consents to the school joining", "MadeUpDocName", application.TrustConsentEvidenceDocumentLink)
			};
			formFields.Add(new FormField("Will there be any changes to the governance of the trust due to the school joining?", application.ChangesToTrust.ToYesNoString()));
			if (application.ChangesToTrust == true) formFields.Add(new FormField("What are the changes?", application.ChangesToTrustExplained));
			formFields.Add(new FormField("Will there be any changes at a local level due to this school joining?", application.ChangesToLaGovernance.ToYesNoString()));
			

			return formFields;
		}
	}
}