using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Utils;

public static class ProjectListHelper
{
   public static ProjectListViewModel Build(AcademyConversionProject academyConversionProject)
   {
      return new ProjectListViewModel
      {
         Id = academyConversionProject.Id.ToString(),
         IsFormAMat = academyConversionProject.IsFormAMat.HasValue && academyConversionProject.IsFormAMat.Value,
         SchoolURN = academyConversionProject.Urn.HasValue ? academyConversionProject.Urn.ToString() : "",
         SchoolName = academyConversionProject.SchoolName,
         LocalAuthority = academyConversionProject.LocalAuthority,
         NameOfTrust = academyConversionProject.NameOfTrust,
         ApplicationReceivedDate = academyConversionProject.ApplicationReceivedDate.ToDateString(),
         AssignedDate = academyConversionProject.AssignedDate.ToDateString(),
         HeadTeacherBoardDate = academyConversionProject.HeadTeacherBoardDate.ToDateString(),
         ProposedConversionDate = academyConversionProject.ProposedConversionDate.ToDateString(),
         Status = MapProjectStatus(academyConversionProject.ProjectStatus),
         AssignedUserFullName = academyConversionProject.AssignedUser?.FullName,
         CreatedOn = academyConversionProject.CreatedOn,
         TypeAndRoute = academyConversionProject.AcademyTypeAndRoute,
         Region = academyConversionProject.Region
      };
   }

   // Convert from "LASTNAME, Firstname" to "Firstname Lastname"
   public static string ConvertToFirstLast(string name)
   {
      if (string.IsNullOrEmpty(name)) return string.Empty;

      string[] parts = name.Split(',');
      if (parts.Length == 2)
      {
         return $"{parts[1].Trim()} {parts[0].Trim()}";
      }

      return name;
   }

   public static FormAMatProjectListViewModel Build(FormAMatProject formAMATProject)
   {
      // This 
      AcademyConversionProject project = formAMATProject.Projects.First();

      return new FormAMatProjectListViewModel
      {
         Id = formAMATProject.Id.ToString(),
         TrustName = formAMATProject.ProposedTrustName,
         ApplciationReference = formAMATProject?.ApplicationReference ?? string.Empty,
         FirstProjectId = formAMATProject.Projects.First().Id,
         AssignedTo = project.AssignedUser?.FullName,
         LocalAuthorities = string.Join(", ", formAMATProject.Projects.Select(x => x.LocalAuthority).Distinct()),
         AdvisoryBoardDate = project.HeadTeacherBoardDate.ToDateString(),
         SchoolNames = string.Join(", ", formAMATProject.Projects.Select(x => x.SchoolName).Distinct()),
         Regions = string.Join(", ", formAMATProject.Projects.Select(x => x.Region).Distinct()),
         Status = GetFormAMatStatuses(formAMATProject.Projects)
      };
   }

   public static ProjectGroupListViewModel Build(ProjectGroup projectGroup)
   {
      // This 
      AcademyConversionProject project = projectGroup.Projects.FirstOrDefault();

      return new ProjectGroupListViewModel
      {
         Id = projectGroup.Id.ToString(),
         TrustName = projectGroup.TrustName,
         TrustReference = projectGroup.TrustReferenceNumber,
         TrustUkprn = projectGroup.TrustUkprn,
         GroupReference = projectGroup.ReferenceNumber,
         //FirstProjectId = project?.Id,
         AssignedTo = project?.AssignedUser?.FullName,
         LocalAuthorities = string.Join(", ", projectGroup.Projects.Select(x => x.LocalAuthority).Distinct()),
         AdvisoryBoardDate = project?.HeadTeacherBoardDate.ToDateString(),
         SchoolNames = string.Join(", ", projectGroup.Projects.Select(x => x.SchoolName).Distinct()),
         Regions = string.Join(", ", projectGroup.Projects.Select(x => x.Region).Distinct()),
         Status = GetFormAMatStatuses(projectGroup.Projects)
      };
   }

   public static List<ProjectStatus> GetFormAMatStatuses(ICollection<AcademyConversionProject> projects)
   {
      List<ProjectStatus> statuses = new();
      foreach (AcademyConversionProject project in projects)
      {
         statuses.Add(MapProjectStatus(project.ProjectStatus));
      }

      return statuses;
   }

   public static ProjectStatus MapProjectStatus(string status)
   {
      const string green = nameof(green);
      const string yellow = nameof(yellow);
      const string orange = nameof(orange);
      const string red = nameof(red);
      const string purple = nameof(purple);
      if (status.Equals("DAO Revoked")) status = "DAORevoked";
      if (Enum.TryParse(status, out AdvisoryBoardDecisions result))
      {
         return result switch
         {
            AdvisoryBoardDecisions.Approved => new ProjectStatus(result.ToString().ToFirstUpper(), green),
            AdvisoryBoardDecisions.Deferred => new ProjectStatus(result.ToString().ToFirstUpper(), orange),
            AdvisoryBoardDecisions.Declined => new ProjectStatus(result.ToString().ToFirstUpper(), red),
            AdvisoryBoardDecisions.DAORevoked => new ProjectStatus("DAO revoked", red),
            AdvisoryBoardDecisions.Withdrawn => new ProjectStatus(result.ToString().ToFirstUpper(), purple),
            _ => new ProjectStatus(result.ToString().ToFirstUpper(), yellow)
         };
      }

      return status?.ToLowerInvariant() switch
      {
         "approved with conditions" => new ProjectStatus("Approved with conditions", green),
         "daorevoked" => new ProjectStatus("DAO revoked", red),
         _ => new ProjectStatus("Pre advisory board", yellow)
      };
   }

   public static string MapPerformanceDataHint(string schoolType, bool hasSchoolAbsenceData)
   {
      string sType = schoolType?.ToLower();

      return sType switch
      {
         "pupil referral unit" =>
            "Your document will automatically include some Ofsted inspection data. Educational performance data isn't published for pupil referral units.\r\n\r\nAsk the pupil referral unit to share their educational performance and absence data with you. You can add that to the document once you have created it.",
         _ => hasSchoolAbsenceData ? "Only educational attendance information will be added to your project template." : "This information will not be added to your project."
      };
   }
}