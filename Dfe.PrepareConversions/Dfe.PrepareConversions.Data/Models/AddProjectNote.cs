using System;

namespace Dfe.PrepareConversions.Data.Models;

public class AddProjectNote
{
   public string Subject { get; set; }
   public string Note { get; set; }
   public string Author { get; set; }
   public DateTime Date { get; set; } = DateTimeSource.UtcNow();
}
