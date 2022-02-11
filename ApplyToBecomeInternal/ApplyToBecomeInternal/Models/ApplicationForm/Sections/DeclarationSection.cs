using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class DeclarationSection : BaseFormSection
	{
		public DeclarationSection(ApplyingSchool application) : base("Declaration")
		{
			SubSections = new[] {new FormSubSection("Details", GenerateDetailsFields(application))};
		}

		private IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application) =>
			new[]
			{
				new FormField("I agree with all of these statements, and believe that the facts stated in this application are true", application.SchoolApplicantDeclarationIsApplicationCorrect.ToYesNoString()),
				new FormField("Signed by", application.SchoolDeclarationSignedByName)
			};
	}
}