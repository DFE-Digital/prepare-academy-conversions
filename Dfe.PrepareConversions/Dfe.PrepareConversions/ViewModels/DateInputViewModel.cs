namespace Dfe.PrepareConversions.ViewModels;

public class DateInputViewModel
{
   public string Id { get; set; }
   public string Name { get; set; }
   public string Day { get; set; }
   public string Month { get; set; }
   public string Year { get; set; }
   public string Label { get; set; }
   public string SubLabel { get; set; }
   public bool HeadingLabel { get; set; }
   public string Hint { get; set; }
   public string ErrorMessage { get; set; }
   public bool DayInvalid { get; set; }
   public bool MonthInvalid { get; set; }
   public bool YearInvalid { get; set; }
   public string PreviousInformation { get; set; }
   public string AdditionalInformation { get; set; }
   public string DateString { get; set; }
   public string DetailsHeading { get; set; }
   public string DetailsBody { get; set; }
}
