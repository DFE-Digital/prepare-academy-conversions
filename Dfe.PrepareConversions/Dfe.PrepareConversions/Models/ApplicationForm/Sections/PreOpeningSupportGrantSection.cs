using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections
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
				new FormField("Do you want these funds paid to the school or the trust?", application.SchoolSupportGrantFundsPaidTo.ToTitleCase()) // might be int with conversion ToSchoolOrTrust()
			};
	}
}