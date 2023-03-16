using System.Collections.Generic;

namespace Dfe.PrepareConversions.Pages.Shared;

public class BackLink
{
   public BackLink(string linkPage, Dictionary<string, string> linkRouteValues, string linkText = "Back")
   {
      LinkPage = linkPage;
      LinkRouteValues = linkRouteValues;
      LinkText = linkText;
   }

   public string LinkPage { get; set; }
   public Dictionary<string, string> LinkRouteValues { get; set; }
   public string LinkText { get; set; }
}
