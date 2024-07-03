using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.ProjectDates;

public class AdvisoryBoardDateModel : BaseAcademyConversionProjectPageModel, IDateValidationMessageProvider
{
   private readonly ErrorService _errorService;

   public AdvisoryBoardDateModel(IAcademyConversionProjectRepository repository,
                       ErrorService errorService)
      : base(repository)
   {
      _errorService = errorService;
   }

   [BindProperty(Name = "advisory-board-date", BinderType = typeof(DateInputModelBinder))]
   //[DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
   [Required]
   public DateTime? AdvisoryBoardDate { get; set; }

   public bool ShowError { get; set; }

   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      return $"Enter the advisory board date";
   }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);


      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);

      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(Request.Form.Keys, ModelState);
         ShowError = true;
         return await base.OnGetAsync(id);
      }

      var projectDatesModel = new SetProjectDatesModel(id, AdvisoryBoardDate, Project.PreviousHeadTeacherBoardDate, Project.ProposedConversionDate, Project.ProjectDatesSectionComplete);

      await _repository.SetProjectDates(id, projectDatesModel);

      return RedirectToPage(Links.ProjectDates.ConfirmProjectDates.Page, new { id });
   }
}
