using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Models
{
   public class SetIncomingTrustDataModel
   {
      public SetIncomingTrustDataModel(int id,
         string trustReferrenceNumber,
         string trustName)
      {
         Id = id;
         TrustReferrenceNumber = trustReferrenceNumber;
         TrustName = trustName;
      }

      public int Id { get; set; }
      public string TrustReferrenceNumber { get; set; }
      public string TrustName { get; set; }
   }
}
