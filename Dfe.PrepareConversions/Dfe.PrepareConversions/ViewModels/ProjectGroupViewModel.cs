using Dfe.PrepareConversions.Data.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.ViewModels;

public class ProjectGroupViewModel
{
   public ProjectGroupViewModel(ProjectGroup project)
   {
      Id = project.Id.ToString();
      TrustName = project.TrustName;
      Ukprn = project.TrustUkprn;
      AssignedUserFullName = project.AssignedUser.FullName;
      Projects = project.Projects;
      ReferenceNumber = project.ReferenceNumber;
   }

   public string Id { get; init; }
   public string TrustName { get; init; }
   public string Ukprn { get; init; }
   public string ReferenceNumber { get; init; }

   public string AssignedUserFullName { get; init; }
   public string Regions { get; set; }
   public string AssignedTo { get; internal set; }
   public string LocalAuthorities { get; internal set; }
   public string AdvisoryBoardDate { get; internal set; }
   public IEnumerable<AcademyConversionProject> Projects { get; set; }
   public ProjectStatus Status { get; init; }
}