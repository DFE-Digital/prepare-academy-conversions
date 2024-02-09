namespace Dfe.PrepareConversions.ViewModels;

public class FormAMatProjectListViewModel
{
   public string Id { get; init; }

   public int FirstProjectId { get; init; }

   public string TrustName { get; init; }
   public string ApplciationReference { get; init; }

   public string SchoolNames { get; init; }
   public string AssignedUserFullName { get; init; }
   public string Regions { get; set; }
   public string AssignedTo { get; internal set; }
   public string LocalAuthorities { get; internal set; }
   public string AdvisoryBoardDate { get; internal set; }

   public ProjectStatus Status { get; init; }
}