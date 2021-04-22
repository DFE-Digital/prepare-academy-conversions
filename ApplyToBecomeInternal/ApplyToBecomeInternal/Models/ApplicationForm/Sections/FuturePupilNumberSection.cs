using ApplyToBecome.Data.Models.Application;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FuturePupilNumberSection : BaseFormSection
	{
		public FuturePupilNumberSection(Application application) : base("Future pupil numbers")
		{
			SubSections = new[] {new FormSubSection("Details", GenerateDetailsFields(application))};
		}

		private IEnumerable<FormField> GenerateDetailsFields(Application application) =>
			new[]
			{
				new FormField("Projected pupil numbers on roll in the year the academy opens (year 1)", application.FuturePupilNumbers.Year1.ToString()),
				new FormField("Projected pupil numbers on roll in the following year after the academy has opened (year 2)", application.FuturePupilNumbers.Year2.ToString()),
				new FormField("Projected pupil numbers on roll in the following year (year 3) ", application.FuturePupilNumbers.Year3.ToString()),
				new FormField("What do you base these projected numbers on? ", application.FuturePupilNumbers.ProjectionReasoning),
				new FormField("What is the school's published admissions number (PAN)?", application.FuturePupilNumbers.PublishedAdmissionsNumber.ToString())
			};
	}
}