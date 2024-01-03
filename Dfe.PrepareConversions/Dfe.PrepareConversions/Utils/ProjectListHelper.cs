using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using System;

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
   }
}
