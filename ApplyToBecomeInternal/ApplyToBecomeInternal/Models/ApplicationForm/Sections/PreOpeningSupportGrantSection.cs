using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class PreOpeningSupportGrantSection : BaseFormSection
	{
		public PreOpeningSupportGrantSection(ApplyingSchool application) : base("Pre-opening support grant")
		{
			SubSections = new[]
{
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}
		private static IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application) =>
			new[] {
				new FormField("Do you want these funds paid to the school or the trust?", application.SchoolSupportGrantFundsPaidTo) // might be int with conversion ToSchoolOrTrust()
			};
	}
}