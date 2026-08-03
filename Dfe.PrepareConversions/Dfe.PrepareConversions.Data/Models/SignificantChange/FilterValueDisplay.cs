
namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

/// <summary>
///    A selectable filter option. Value is what gets posted to the search endpoint; Display is the
///    checkbox label. They differ for Status ("PreDecision" / "Pre decision") and Tier ("1" / "Tier 1").
/// </summary>
public class FilterValueDisplay
{
   public string Value { get; set; } = string.Empty;
   public string Display { get; set; } = string.Empty;
}