using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using System;
using DocumentFormat.OpenXml.Drawing;
using System.Linq;
using DocumentFormat.OpenXml.Math;
using System.Globalization;

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

      public static FormAMatProjectListViewModel Build(FormAMatProject formAMATProject)
      {
         // This 
         var project = formAMATProject.Projects.First();

         return new FormAMatProjectListViewModel
         {
            Id = formAMATProject.Id.ToString(),          
            TrustName = formAMATProject.ProposedTrustName,
            ApplciationReference = formAMATProject.ApplicationReference,
            FirstProjectId = formAMATProject.Projects.First().Id,
            AssignedTo = project.AssignedUser?.FullName,
            LocalAuthorities = String.Join(", ", formAMATProject.Projects.Select(x => x.LocalAuthority).Distinct()),
            AdvisoryBoardDate = project.HeadTeacherBoardDate.ToDateString(),
            SchoolNames = String.Join(", ", formAMATProject.Projects.Select(x => x.SchoolName).Distinct()),
            Regions = String.Join(", ", formAMATProject.Projects.Select(x => x.Region).Distinct()),
            Status = MapProjectStatus(project.ProjectStatus)
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
