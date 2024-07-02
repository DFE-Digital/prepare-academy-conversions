using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.ProjectDates;

public class ProposedConversionDate : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public ProposedConversionDate(IAcademyConversionProjectRepository repository,
                       ErrorService errorService)
      : base(repository)
   {
      _errorService = errorService;
   }

   public DateTime? ConversionDate { get; set; }

   [BindProperty(Name = "proposed-conversion-month")]
   public string Month { get; set; }

   [BindProperty(Name = "proposed-conversion-year")]
   public string Year { get; set; }

   public bool MonthIsValid { get;set; }
   public bool YearIsValid { get; set; }

   public bool ShowError => _errorService.HasErrors();
   public string ErrorMessage => _errorService.GetErrors().ToList().Last().Message;

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);


      var conversionDate = Project.ProposedConversionDate;
      if (conversionDate.HasValue)
      {
         Month = conversionDate?.Month.ToString();
         Year = conversionDate?.Year.ToString();
      }

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);

      ValidateDate();
      if (_errorService.HasErrors())
      {
         return await OnGetAsync(id);
      }

      var conversionDate = $"{Year}-{Month}-1";

      return RedirectToPage(Links.ProjectDates.ReasonForConversionDateChange.Page, new { id, conversionDate });
   }

   private void ValidateDate()
   {
      List<string> missingParts = new();

      if (string.IsNullOrWhiteSpace(Month)) missingParts.Add("month");
      if (string.IsNullOrWhiteSpace(Year)) missingParts.Add("year");

      if (missingParts.Count == 2)
      {
         _errorService.AddError("proposed-conversion-month", "Enter the proposed conversion date");
         return;
      }

      if (missingParts.Count > 0)
      {
         _errorService.AddError("proposed-conversion-month", SomeMissing(missingParts));
         return;
      }

      YearIsValid = int.TryParse(Year, out int year);
      MonthIsValid = int.TryParse(Month, out int month);

      if (!YearIsValid || !MonthIsValid)
      {
         _errorService.AddError("proposed-conversion-month", "Enter a date in the correct format");
         return;
      }

      if (month < 1 || month > 12)
      {
         _errorService.AddError("proposed-conversion-month", "Month must be between 1 and 12");
         return;
      }

      if (year < 2000 || year > 2050)
      {
         _errorService.AddError("proposed-conversion-month", "Year must be between 2000 and 2050");
         return;
      }
   }

   private string SomeMissing(IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }
}