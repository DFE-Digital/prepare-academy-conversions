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

public class ConfirmProjectDatesModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public ConfirmProjectDatesModel(IAcademyConversionProjectRepository repository, ErrorService errorService): base(repository)
   {
      _errorService = errorService;
   }

   [BindProperty(Name = "project-dates-complete", BinderType = typeof(CheckboxInputModelBinder))]
   public bool ProjectDatesSectionComplete { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await SetProject(id);

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await SetProject(id);

      var projectDatesModel = new SetProjectDatesModel(id, Project.HeadTeacherBoardDate, Project.PreviousHeadTeacherBoardDate, ProjectDatesSectionComplete);

      await _repository.SetProjectDates(id, projectDatesModel);

      return RedirectToPage(Links.TaskList.Index.Page , new { id });
   }
}
