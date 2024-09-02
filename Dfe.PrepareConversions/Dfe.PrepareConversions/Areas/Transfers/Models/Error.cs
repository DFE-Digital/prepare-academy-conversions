using System.Collections.Generic;

namespace Dfe.PrepareConversions.Areas.Transfers.Models;

public class Error
{
   public string Key { get; set; }
   public string Message { get; set; }
   public List<string> InvalidInputs { get; set; } = new();
}
