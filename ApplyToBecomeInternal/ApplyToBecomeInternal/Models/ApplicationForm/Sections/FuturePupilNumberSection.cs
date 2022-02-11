using ApplyToBecome.Data.Models.Application;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FuturePupilNumberSection : BaseFormSection
	{
		public FuturePupilNumberSection(ApplyingSchool application) : base("Future pupil numbers")
		{
			SubSections = new[] {new FormSubSection("Details", GenerateDetailsFields(application))};
		}

		private IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application) =>
			new[]
			{
				new FormField("Projected pupil numbers on roll in the year the academy opens (year 1)", application.SchoolCapacityYear1.ToString()),
				new FormField("Projected pupil numbers on roll in the following year after the academy has opened (year 2)", application.SchoolCapacityYear2.ToString()),
				new FormField("Projected pupil numbers on roll in the following year (year 3) ", application.SchoolCapacityYear3.ToString()),
				new FormField("What do you base these projected numbers on? ", application.SchoolCapacityAssumptions),
				new FormField("What is the school's published admissions number (PAN)?", application.SchoolCapacityPublishedAdmissionsNumber.ToString())
			};
	}
}