using System;

namespace Dfe.PrepareConversions.ViewModels;

public class ProjectListViewModel
{
   public string Id { get; init; }
   public string NameOfTrust { get; init; }
   public string SchoolName { get; init; }
   public string SchoolURN { get; init; }
   public string LocalAuthority { get; init; }
   public string ApplicationReceivedDate { get; init; }
   public string AssignedDate { get; set; }
   public string HeadTeacherBoardDate { get; init; }
   public string ProposedAcademyOpeningDate { get; init; }
   public ProjectStatus Status { get; init; }
   public string AssignedUserFullName { get; init; }
   public DateTime? CreatedOn { get; init; }

   public bool ShowHtbDate => string.IsNullOrWhiteSpace(HeadTeacherBoardDate) is false;
}