using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class ConsultationSection : BaseFormSection
	{
		public ConsultationSection(Application application) : base("Consultation")
		{
			SubSections = new[]
			{
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateDetailsFields(Application application) =>
			new[] {
				new FormField("Has the governing body consulted the relevant stakeholders?", application.HasGovernmentConsultedStakeholders.ToYesNoString())
			};

	}
}