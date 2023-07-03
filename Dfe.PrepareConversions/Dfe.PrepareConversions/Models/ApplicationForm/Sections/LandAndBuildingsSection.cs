using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections;

public class LandAndBuildingsSection : BaseFormSection
{
   public LandAndBuildingsSection(ApplyingSchool application) : base("Land and buildings")
   {
      SubSections = new[] { new FormSubSection("Details", GenerateDetailsFields(application)) };
   }

   private static IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application)
   {
      List<FormField> formFields = new();
      formFields.Add(new FormField("As far as you're aware, who owns or holds the school's buildings and land?", application.SchoolBuildLandOwnerExplained));
      formFields.Add(new FormField("Are there any current planned building works?", application.SchoolBuildLandWorksPlanned.ToYesNoString()));
      if (application.SchoolBuildLandWorksPlanned == true)
      {
         formFields.Add(new FormField("Provide details of the works, how they'll be funded and whether the funding will be affected by the conversion",
            application.SchoolBuildLandWorksPlannedExplained));
         formFields.Add(new FormField("When is the scheduled completion date?", application.SchoolBuildLandWorksPlannedCompletionDate.ToDateString()));
      }

      formFields.Add(new FormField("Are there any shared facilities on site?", application.SchoolBuildLandSharedFacilities.ToYesNoString()));
      if (application.SchoolBuildLandSharedFacilities == true)
      {
         formFields.Add(new FormField("List the facilities and the school's plan for them after converting", application.SchoolBuildLandSharedFacilitiesExplained));
      }

      formFields.Add(new FormField("Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?",
         application.SchoolBuildLandGrants.ToYesNoString()));
      if (application.SchoolBuildLandGrants == true)
      {
         formFields.Add(new FormField("Which bodies awarded the grants and what facilities did they fund?", application.SchoolBuildLandGrantsExplained));
      }

      formFields.Add(new FormField("Is the school part of a Private Finance Intiative (PFI) scheme?", application.SchoolBuildLandPFIScheme.ToYesNoString()));
      if (application.SchoolBuildLandPFIScheme == true)
      {
         formFields.Add(new FormField("What kind of PFI Scheme is your school part of?", application.SchoolBuildLandPFISchemeType));
      }

      formFields.Add(new FormField("Is the school part of a Priority School Building Programme?", application.SchoolBuildLandPriorityBuildingProgramme.ToYesNoString()));
      formFields.Add(new FormField("Is the school part of a Building Schools for the Future Programme?", application.SchoolBuildLandFutureProgramme.ToYesNoString()));

      return formFields;
   }
}
