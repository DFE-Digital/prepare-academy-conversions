using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;
using static System.String;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections
{
   public class TrustInformationSection : BaseFormSection
   {
      public TrustInformationSection(Application application) : base("Trust information")
      {
         SubSections = new[]
         {
         new FormSubSection(Empty, GenerateProposedName(application)),
         new FormSubSection("Opening date", GenerateOpeningDateFields(application)),
         new FormSubSection("Reasons for forming the trust", GenerateReasonsForJoiningFields(application)),
         new FormSubSection("Plans for growth", GeneratePlansForGrowthFields(application)),
         new FormSubSection("School improvement strategy", GenerateSchoolImprovementStrategyFields(application))
         };
      }

      private static IEnumerable<FormField> GenerateProposedName(Application application)
      {
         List<FormField> formFields = new();
         formFields.Add(new FormField("Proposed name of the trust", application.TrustName));

         return formFields;
      }

      private static IEnumerable<FormField> GenerateSchoolImprovementStrategyFields(Application application)
      {
         List<FormField> formFields = new();
         formFields.Add(new FormField("How will the trust support and improve the academies in the trust?", application.FormTrustImprovementSupport));
         formFields.Add(new FormField("How will the trust make sure that the school improvement strategy is fit for purpose and improve standards?", application.FormTrustImprovementStrategy));
         formFields.Add(new FormField("If the trust wants to become an approved sponsor, when would it plan to apply and begin sponsoring schools?", application.FormTrustImprovementApprovedSponsor));

         return formFields;
      }

      private static IEnumerable<FormField> GenerateOpeningDateFields(Application application)
      {
         List<FormField> formFields = new();
         formFields.Add(new FormField("When do you plan to establish the new multi-academy trust?", application.FormTrustOpeningDate.ToDateString()));
         formFields.Add(new FormField("Approver name", application.TrustApproverName));
         formFields.Add(new FormField("Approver email address", application.TrustApproverEmail));
         
         return formFields;
      }
      private static IEnumerable<FormField> GenerateReasonsForJoiningFields(Application application)
      {
         List<FormField> formFields = new();
         formFields.Add(new FormField("Why are the schools forming the trust?", application.FormTrustReasonForming));
         formFields.Add(new FormField("What vision and aspirations have the schools agreed to for the trust?", application.FormTrustReasonVision));
         formFields.Add(new FormField("What geographical areas and communities will the trust serve?", application.FormTrustReasonGeoAreas));
         formFields.Add(new FormField("How will the schools make the most of the freedom that academies have?", application.FormTrustReasonFreedom));
         formFields.Add(new FormField("How will the schools work together to improve teaching and learning?", application.FormTrustReasonImproveTeaching));

         return formFields;
      }
      private static IEnumerable<FormField> GeneratePlansForGrowthFields(Application application)
      {
         List<FormField> formFields = new();
         formFields.Add(new FormField("Do you plan to grow the trust over the next 5 years?", application.FormTrustGrowthPlansYesNo.ToYesNoString()));
         formFields.Add(new FormField("What are your plans?", application.FormTrustPlansForNoGrowth ?? application.FormTrustPlanForGrowth));

         return formFields;
      }

   }
}