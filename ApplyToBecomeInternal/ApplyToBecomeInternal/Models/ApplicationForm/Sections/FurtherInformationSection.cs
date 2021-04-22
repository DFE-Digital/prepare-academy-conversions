using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Extensions;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ApplicationForm.Sections
{
	public class FurtherInformationSection : BaseFormSection
	{
		public FurtherInformationSection(Application application) : base("Further information") => 
			SubSections = new[] {new FormSubSection("Additional details", GenerateFields(application))};

		private IEnumerable<FormField> GenerateFields(Application application) =>
			new[]
			{
				new FormField("What will the school bring to the trust they are joining?", application.FurtherInformation.WhatWillSchoolBringToTrust),
				new FormField("Have Ofsted inspected the school but not published the report yet?", application.FurtherInformation.HasUnpublishedOfstedInspection.ToYesNoString()),
				new FormField("Are there any safeguarding investigations ongoing at the school?", application.FurtherInformation.HasSafeguardingInvestigations.ToYesNoString()),
				new FormField("Is the school part of a local authority reorganisation?", application.FurtherInformation.IsPartOfLocalAuthorityReorganisation.ToYesNoString()),
				new FormField("Is the school part of any local authority closure plans?", application.FurtherInformation.IsPartOfLocalAuthorityClosurePlans.ToYesNoString()),
				new FormField("Is your school linked to a diocese?", application.FurtherInformation.IsLinkedToDiocese.ToYesNoString()),
				new FormField("Name of diocese?", application.FurtherInformation.NameOfDiocese),
				new LinkFormField("Upload a letter of consent from the diocese", application.FurtherInformation.DioceseLetterOfConsent),
				new FormField("Is your school part of a federation?", application.FurtherInformation.IsPartOfFederation.ToYesNoString()),
				new FormField("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?", application.FurtherInformation.IsSupportedByFoundationTrustOrOtherBody.ToYesNoString()),
				new FormField(
					"Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?",
					application.FurtherInformation.HasSACREChristianWorshipExcemption.ToYesNoString()),
				new FormField("Please provide a list of your main feeder schools", application.FurtherInformation.MainFeederSchools),
				new LinkFormField(
					"The school's Governing Body must have passed a resolution to apply to convert to academy status. Upload a copy of the school’s consent to converting and joining the trust.",
					application.FurtherInformation.SchoolConsent),
				new FormField("Has an equalities impact assessment been carried out and considered by the governing body?", application.FurtherInformation.EqualitiesImpactAssessmentResult),
				new FormField("Do you want to add any further information?", application.FurtherInformation.AdditionalInformation??"No")
			};
	}
}