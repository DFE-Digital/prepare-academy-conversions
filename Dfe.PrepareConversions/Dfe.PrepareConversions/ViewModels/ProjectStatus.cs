namespace Dfe.PrepareConversions.ViewModels;

public class ProjectStatus
{
   public ProjectStatus(string value, string colour)
   {
      Value = value;
      Colour = colour;
   }

   public string Value { get; }
   public string Colour { get; }
}
