using System;

namespace Dfe.PrepareConversions.ViewModels;

public class SummaryListRowViewModel
{
   public string Id { get; set; }
   public string Key { get; set; }
   public string Value { get; set; }
   public string ValueLink { get; set; }
   public string AdditionalText { get; set; }
   public bool HasValue => !string.IsNullOrWhiteSpace(Value);
   public bool HasAdditionalText => !string.IsNullOrWhiteSpace(AdditionalText);
   public bool HasValueLink => !string.IsNullOrWhiteSpace(ValueLink);
   public string Page { get; set; }
   public string Fragment { get; set; }
   public string RouteId { get; set; }
   public string Return { get; set; }
   public string HiddenText { get; set; }
   public string KeyWidth { get; set; }
   public string ValueWidth { get; set; }
   public string Name { get; set; }
   public bool HighlightNegativeValue { get; set; }
   public bool IsReadOnly { get; set; }

   public string NegativeStyleClass
   {
      get
      {
         string negativeStyleClass = string.Empty;
         if (HasValue)
         {
            decimal decimalValue;
            if (Decimal.TryParse(Value.Replace("£", ""), out decimalValue))
            {
               negativeStyleClass = HighlightNegativeValue && decimalValue < 0 ? "negative-value" : string.Empty;
            }
         }

         return negativeStyleClass;
      }
   }
}
