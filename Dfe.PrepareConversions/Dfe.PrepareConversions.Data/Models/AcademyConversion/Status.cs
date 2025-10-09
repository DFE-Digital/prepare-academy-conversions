using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.AcademyConversion;

public enum Status
{
   [Description("Not started")] NotStarted = 0,
   [Description("In progress")] InProgress = 1,
   Completed = 2
}