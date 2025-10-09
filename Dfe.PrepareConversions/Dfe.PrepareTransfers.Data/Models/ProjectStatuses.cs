using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareTransfers.Data.Models;

public enum ProjectStatuses
{
   Empty = 0,

   [Display(Name = "Not Started")] NotStarted,

   [Display(Name = "In Progress")] InProgress,

   [Display(Name = "Completed")] Completed
}