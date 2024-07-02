using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models
{
   public class SetProjectDatesModel
   {
      public int Id { get; set; }
      public DateTime? AdvisoryBoardDate { get; set; }
      public DateTime? PreviousAdvisoryBoard { get; set; }
      public DateTime? ProposedConversionDate { get; set; }
      public bool? ProjectDatesSectionComplete { get; set; }
      public IEnumerable<ReasonChange>? ReasonsChanged { get; set; }
      public string? ChangedBy { get; set; }

      public SetProjectDatesModel() { }

      public SetProjectDatesModel(int id, DateTime? advisoryBoardDate, DateTime? previousAdvisoryBoard, DateTime? proposedConversionDate, bool? projectDatesSectionComplete, string? changedBy = default, IEnumerable<ReasonChange>? reasonsChanged = default)
      {
         Id = id;
         AdvisoryBoardDate = advisoryBoardDate;
         PreviousAdvisoryBoard = previousAdvisoryBoard;
         ProposedConversionDate = proposedConversionDate;
         ProjectDatesSectionComplete = projectDatesSectionComplete;
         ChangedBy = changedBy;
         ReasonsChanged = reasonsChanged;
      }
   }
}
