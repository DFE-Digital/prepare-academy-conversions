using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections;

public class FurtherInformationSection : BaseFormSection
{
   public FurtherInformationSection(ApplyingSchool application) : base("Further information")
   {
      SubSections = new[] { new FormSubSection("Additional details", GenerateFields(application)) };
   }

   private static IEnumerable<FormField> GenerateFields(ApplyingSchool application)
   {
      List<FormField> formFields = new();

      formFields.Add(new FormField("What will the school bring to the trust they are joining?", application.SchoolAdSchoolContributionToTrust));
      formFields.Add(new FormField("Have Ofsted inspected the school but not published the report yet?", application.SchoolAdInspectedButReportNotPublished.ToYesNoString()));
      if (application.SchoolAdInspectedButReportNotPublished == true)
      {
         formFields.Add(new FormField("Provide the inspection date and a short summary of the outcome?", application.SchoolAdInspectedButReportNotPublishedExplain));
      }

      formFields.Add(new FormField("Are there any safeguarding investigations ongoing at the school?", application.SchoolOngoingSafeguardingInvestigations.ToYesNoString()));
      if (application.SchoolOngoingSafeguardingInvestigations == true)
      {
         formFields.Add(new FormField("Details of the investigation", application.SchoolOngoingSafeguardingDetails));
      }

      formFields.Add(new FormField("Is the school part of a local authority reorganisation?", application.SchoolPartOfLaReorganizationPlan.ToYesNoString()));
      if (application.SchoolPartOfLaReorganizationPlan == true)
      {
         formFields.Add(new FormField("Details of the reorganisation", application.SchoolLaReorganizationDetails));
      }

      formFields.Add(new FormField("Is the school part of any local authority closure plans?", application.SchoolPartOfLaClosurePlan.ToYesNoString()));
      if (application.SchoolPartOfLaClosurePlan == true)
      {
         formFields.Add(new FormField("Details of the closure plan", application.SchoolLaClosurePlanDetails));
      }

      formFields.Add(new FormField("Is your school linked to a diocese?", application.SchoolFaithSchool.ToYesNoString()));
      if (application.SchoolFaithSchool == true)
      {
         formFields.Add(new FormField("Name of diocese", application.SchoolFaithSchoolDioceseName));
      }

      formFields.Add(new FormField("Is your school part of a federation?", application.SchoolIsPartOfFederation.ToYesNoString()));
      formFields.Add(new FormField("Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?",
         application.SchoolIsSupportedByFoundation.ToYesNoString()));
      if (application.SchoolIsSupportedByFoundation == true)
      {
         formFields.Add(new FormField("Name of this body", application.SchoolSupportedFoundationBodyName));
      }

      formFields.Add(new FormField(
         "Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?",
         application.SchoolHasSACREException.ToYesNoString()));
      if (application.SchoolHasSACREException == true)
      {
         formFields.Add(new FormField("When does the exemption end?",
            application.SchoolSACREExemptionEndDate.HasValue ? application.SchoolSACREExemptionEndDate.ToDateString() : string.Empty));
      }

      formFields.Add(new FormField("Please provide a list of your main feeder schools", application.SchoolAdFeederSchools));
      formFields.Add(new FormField("Has an equalities impact assessment been carried out and considered by the governing body?",
         application.SchoolAdEqualitiesImpactAssessmentCompleted.ToYesNoString()));

      if (application.SchoolAdEqualitiesImpactAssessmentCompleted == true)
      {
         formFields.Add(new FormField("When the governing body considered the equality duty what did they decide?", application.SchoolAdEqualitiesImpactAssessmentDetails));
      }

      formFields.Add(new FormField("Do you want to add any further information?", application.SchoolAdditionalInformationAdded.ToYesNoString()));
      if (application.SchoolAdditionalInformationAdded == true)
      {
         formFields.Add(new FormField("Add any further information", application.SchoolAdditionalInformation));
      }


      return formFields;
   }
}
