using Dfe.PrepareConversions.Data.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.ViewModels;

public class FormAMatProjectViewModel
{
   public FormAMatProjectViewModel(FormAMatProject project)
   {
      Id = project.Id.ToString();
      TrustName = project.ProposedTrustName;
      ApplciationReference = project.ApplicationReference;
      AssignedUserFullName = project.AssignedUser.FullName;
      Projects = project.Projects;
      //Status = project.;
   }

   public string Id { get; init; }
   public string TrustName { get; init; }
   public string ApplciationReference { get; init; }

   public string AssignedUserFullName { get; init; }
   public string Regions { get; set; }
   public string AssignedTo { get; internal set; }
   public string LocalAuthorities { get; internal set; }
   public string AdvisoryBoardDate { get; internal set; }
   public IEnumerable<AcademyConversionProject> Projects { get; set; }
   public ProjectStatus Status { get; init; }
}