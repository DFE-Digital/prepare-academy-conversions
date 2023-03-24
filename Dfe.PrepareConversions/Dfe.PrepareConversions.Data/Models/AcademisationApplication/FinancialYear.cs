using System;

namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication;

public class FinancialYear
{
   public DateTime FinancialYearEndDate { get; set; }
   public double Revenue { get; set; }
   public string RevenueStatus { get; set; }
   public string RevenueStatusExplained { get; set; }
   public string RevenueStatusFileLink { get; set; }
   public double CapitalCarryForward { get; set; }
   public string CapitalCarryForwardStatus { get; set; }
   public string CapitalCarryForwardExplained { get; set; }
}
