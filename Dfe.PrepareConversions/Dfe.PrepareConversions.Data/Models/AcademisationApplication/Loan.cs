using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication
{
   public class Loan
   {
      public decimal Amount { get; set; }
      public string Purpose { get; set; }
      public string Provider { get; set; }
      public double InterestRate { get; set; }
      public string Schedule { get; set; }
   }
}
