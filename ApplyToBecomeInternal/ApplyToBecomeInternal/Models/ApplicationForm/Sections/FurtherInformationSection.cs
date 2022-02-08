using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FurtherInformationSection : BaseFormSection
	{
		public FurtherInformationSection(ApplyingSchool application) : base("Further information") => 
			SubSections = new[] {new FormSubSection("Additional details", GenerateFields(application))};

		private IEnumerable<FormField> GenerateFields(ApplyingSchool application) =>
			new[]
			{
				new FormField("What will the school bring to the trust they are joining?", application.SchoolAdSchoolContributionToTrust),
				new FormField("Have Ofsted inspected the school but not published the report yet?", application.SchoolAdInspectedButReportNotPublished.ToYesNoString()),
				new FormField("Are there any safeguarding investigations ongoing at the school?", application.SchoolOngoingSafeguardingInvestigations.ToYesNoString()),
				new FormField("Is the school part of a local authority reorganisation?", application.SchoolPartOfLaReorganizationPlan.ToYesNoString()),
				new FormField("Is the school part of any local authority closure plans?", application.SchoolPartOfLaClosurePlan.ToYesNoString()),
				new FormField("Is your school linked to a diocese?", application.SchoolFaithSchool.ToYesNoString()),
				// conditional rows depending on answer above
				//new FormField("Name of diocese?", application.SchoolFaithSchoolDioceseName),
				//new LinkFormField(
				//	"Upload a letter of consent from the diocese", 
				//	new Link(application.DiocesePermissionEvidenceDocument.Name, application.DiocesePermissionEvidenceDocument.Url)),
				new FormField("Is your school part of a federation?", application.SchoolIsPartOfFederation.ToYesNoString()),
				new FormField("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", application.SchoolIsSupportedByFoundation.ToYesNoString()),
				new FormField(
					"Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?",
					application.SchoolHasSACREException.ToYesNoString()),
				new FormField("Please provide a list of your main feeder schools", application.SchoolAdFeederSchools),
				new LinkFormField(
					"The school's Governing Body must have passed a resolution to apply to convert to academy status. Upload a copy of the school’s consent to converting and joining the trust.",
					new Link(application.GoverningBodyConsentEvidenceDocument.Name, application.GoverningBodyConsentEvidenceDocument.Url)),
				new FormField("Has an equalities impact assessment been carried out and considered by the governing body?", application.SchoolAdEqualitiesImpactAssessmentCompleted.ToYesNoString()),
				new FormField("Do you want to add any further information?", application.SchoolAdditionalInformationAdded.ToYesNoString())
			};
	}
}