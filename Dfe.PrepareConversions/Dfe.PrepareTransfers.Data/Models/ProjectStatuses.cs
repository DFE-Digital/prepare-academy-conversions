using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareTransfers.Data.Models;

public enum ProjectStatuses
{
   Empty = 0,

   [Display(Name = "Not started")] NotStarted,

   [Display(Name = "In progress")] InProgress,

   [Display(Name = "Completed")] Completed
}