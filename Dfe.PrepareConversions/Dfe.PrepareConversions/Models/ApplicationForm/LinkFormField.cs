using Dfe.PrepareConversions.Data.Models.Application;

namespace Dfe.PrepareConversions.Models.ApplicationForm;

public class LinkFormField : FormField
{
   public LinkFormField(string title, string content, string url) : base(title, content)
   {
      Url = url;
   }

   public LinkFormField(string title, Link link) : this(title, link?.Name, link?.Url)
   {
   }

   public string Url { get; }
}
