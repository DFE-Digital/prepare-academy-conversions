using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class ConsultationSection : BaseFormSection
	{
		public ConsultationSection(ApplyingSchool application) : base("Consultation")
		{
			SubSections = new[]
			{
				new FormSubSection("Details", GenerateDetailsFields(application))
			};
		}

		private static IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application)
		{
			var formFields = new List<FormField>();
			formFields.Add(new FormField("Has the governing body consulted the relevant stakeholders?", application.SchoolHasConsultedStakeholders.ToYesNoString()));
			if (application.SchoolHasConsultedStakeholders == false)
			{
				formFields.Add(new FormField("When does the governing body plan to consult?", application.SchoolPlanToConsultStakeholders));
				
			}

			return formFields;

		}	

	}
}