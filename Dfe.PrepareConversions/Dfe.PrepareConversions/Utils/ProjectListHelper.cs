using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using System;
using DocumentFormat.OpenXml.Drawing;

namespace Dfe.PrepareConversions.Utils
{
   public static class ProjectListHelper
   {
      public static ProjectListViewModel Build(AcademyConversionProject academyConversionProject)
      {
         return new ProjectListViewModel
         {
            Id = academyConversionProject.Id.ToString(),
            SchoolURN = academyConversionProject.Urn.HasValue ? academyConversionProject.Urn.ToString() : "",
            SchoolName = academyConversionProject.SchoolName,
            LocalAuthority = academyConversionProject.LocalAuthority,
            NameOfTrust = academyConversionProject.NameOfTrust,
            ApplicationReceivedDate = academyConversionProject.ApplicationReceivedDate.ToDateString(),
            AssignedDate = academyConversionProject.AssignedDate.ToDateString(),
            HeadTeacherBoardDate = academyConversionProject.HeadTeacherBoardDate.ToDateString(),
            ProposedAcademyOpeningDate = academyConversionProject.ProposedAcademyOpeningDate.ToDateString(),
            Status = MapProjectStatus(academyConversionProject.ProjectStatus),
            AssignedUserFullName = academyConversionProject.AssignedUser?.FullName,
            CreatedOn = academyConversionProject.CreatedOn,
            TypeAndRoute = academyConversionProject.AcademyTypeAndRoute,
            Region = academyConversionProject.Region
         };
      }

      public static ProjectListViewModel Build(FormAMATProject formAMATProject)
      {
         return new ProjectListViewModel
         {
            Id = formAMATProject.Id.ToString(),
            SchoolURN = formAMATProject.Urn.HasValue ? formAMATProject.Urn.ToString() : "",
            SchoolName = formAMATProject.SchoolName,
            LocalAuthority = formAMATProject.LocalAuthority,
            NameOfTrust = formAMATProject.NameOfTrust,
            ApplicationReceivedDate = formAMATProject.ApplicationReceivedDate.ToDateString(),
            AssignedDate = formAMATProject.AssignedDate.ToDateString(),
            HeadTeacherBoardDate = formAMATProject.HeadTeacherBoardDate.ToDateString(),
            ProposedAcademyOpeningDate = formAMATProject.ProposedAcademyOpeningDate.ToDateString(),
            Status = MapProjectStatus(formAMATProject.ProjectStatus),
            AssignedUserFullName = formAMATProject.AssignedUser?.FullName,
            CreatedOn = formAMATProject.CreatedOn,
            TypeAndRoute = formAMATProject.AcademyTypeAndRoute,
            Region = formAMATProject.Region
         };
      }

      public static ProjectStatus MapProjectStatus(string status)
      {
         const string green = nameof(green);
         const string yellow = nameof(yellow);
         const string orange = nameof(orange);
         const string red = nameof(red);

         if (Enum.TryParse(status, out AdvisoryBoardDecisions result))
         {
            return result switch
            {
               AdvisoryBoardDecisions.Approved => new ProjectStatus(result.ToString().ToUpper(), green),
               AdvisoryBoardDecisions.Deferred => new ProjectStatus(result.ToString().ToUpper(), orange),
               AdvisoryBoardDecisions.Declined => new ProjectStatus(result.ToString().ToUpper(), red),
               _ => new ProjectStatus(result.ToString().ToUpper(), yellow)
            };
         }

         return status?.ToLowerInvariant() switch
         {
            "approved with conditions" => new ProjectStatus("Approved with Conditions", green),
            _ => new ProjectStatus("PRE ADVISORY BOARD", yellow)
         };
      }
   
      public static string MapPerformanceDataHint(string schoolType)
      {
         var sType = schoolType?.ToLower();

         return sType switch
         {
            "pupil referral unit" => $"Your document will automatically include some Ofsted inspection data. Educational performance data isn't published for pupil referral units.\r\n\r\nAsk the pupil referral unit to share their educational performance and absence data with you. You can add that to the document once you have created it.",
            _ => "This information will be added to your project document automatically."
         };
      }
   }
}
