using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication
{
   public class Lease
   {
      public string LeaseTerm { get; set; }
      public decimal RepaymentAmount { get; set; }
      public decimal InterestRate { get; set; }
      public decimal PaymentsToDate { get; set; }
      public string Purpose { get; set; }
      public string ValueOfAssets { get; set; }
      public string ResponsibleForAssets { get; set; }
   }
}
