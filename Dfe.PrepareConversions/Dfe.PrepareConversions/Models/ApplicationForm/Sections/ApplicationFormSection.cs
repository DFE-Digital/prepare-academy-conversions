using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections
{
	public class ApplicationFormSection : BaseFormSection
	{
		public ApplicationFormSection(Application application) : base("Overview", GenerateBaseFields(application))
		{
			SubSections = new[] { 
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateBaseFields(Application application) =>
			new[] {
				new FormField("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
				new FormField("Application reference", application.ApplicationId),
				new FormField("Lead applicant", application.ApplicationLeadAuthorName),
			};

		private static IEnumerable<FormField> GenerateDetailsFields(Application application)
		{
			var formFields = new List<FormField> {
				new FormField("Trust name", application.TrustName),
			};
			formFields.Add(new FormField("Will there be any changes to the governance of the trust due to the school joining?", application.ChangesToTrust == null ? "Unknown" : application.ChangesToTrust.ToYesNoString()));
			if ( application.ChangesToTrust == true ) formFields.Add(new FormField("What are the changes?", application.ChangesToTrustExplained) );
			formFields.Add(new FormField("Will there be any changes at a local level due to this school joining?", application.ChangesToLaGovernance == null ? "Unknown" : application.ChangesToLaGovernance.ToYesNoString()));
			if (application.ChangesToLaGovernance == true) formFields.Add(new FormField("What are the changes and how do they fit into the current lines of accountability in the trust?", application.ChangesToLaGovernanceExplained));

			return formFields;
		}
	}
}