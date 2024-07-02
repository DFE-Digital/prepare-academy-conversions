using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Models
{
   public class OpeningDateHistoryDto
   {
      public DateTime ChangedAt { get; set; }
      public string ChangedBy { get; set; }
      public DateTime? OldDate { get; set; }
      public DateTime? NewDate { get; set; }
      public List<ReasonChange> ReasonsChanged { get; set; }
   }


   public class ReasonChange
   {
      public ReasonChange(string heading, string details)
      {
         Heading = heading;
         Details = details;
      }

      public string Heading { get; set; }
      public string Details { get; set; }

   }
}
