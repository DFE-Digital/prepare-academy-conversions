using System;

namespace Dfe.PrepareConversions.Data.Models
{
   public class SetProjectDatesModel
   {
      public int Id { get; set; }
      public DateTime? AdvisoryBoardDate { get; set; }
      public DateTime? PreviousAdvisoryBoard { get; set; }
      public bool? ProjectDatesSectionComplete { get; set; }

      public SetProjectDatesModel() { }

      public SetProjectDatesModel(int id, DateTime? advisoryBoardDate, DateTime? previousAdvisoryBoard, bool? projectDatesSectionComplete)
      {
         Id = id;
         AdvisoryBoardDate = advisoryBoardDate;
         PreviousAdvisoryBoard = previousAdvisoryBoard;
         ProjectDatesSectionComplete = projectDatesSectionComplete;
      }
   }
}
