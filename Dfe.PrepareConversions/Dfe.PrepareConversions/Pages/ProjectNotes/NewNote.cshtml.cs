using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectNotes;

public class NewNoteModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public NewNoteModel(ErrorService errorService, IAcademyConversionProjectRepository repository) : base(repository)
   {
      _errorService = errorService;
   }

   public bool ShowError => _errorService.HasErrors();

   public string SuccessPage
   {
      get => TempData[nameof(SuccessPage)].ToString();
      set => TempData[nameof(SuccessPage)] = value;
   }

   [BindProperty(Name = "project-note-subject")]
   public string ProjectNoteSubject { get; set; }

   [BindProperty(Name = "project-note-body")]
   public string ProjectNoteBody { get; set; }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      if (string.IsNullOrEmpty(ProjectNoteSubject) || string.IsNullOrEmpty(ProjectNoteBody))
      {
         _errorService.AddError("project-note-body", "Enter a subject and project note");

         await SetProject(id);
         return Page();
      }

      ApiResponse<ProjectNote> response =
         await _repository.AddProjectNote(id, new AddProjectNote { Subject = ProjectNoteSubject, Note = ProjectNoteBody, Author = NameOfUser });

      if (!response.Success)
      {
         _errorService.AddApiError();
         await SetProject(id);
         return Page();
      }

      TempData["newNote"] = true;

      return RedirectToPage(SuccessPage, new { id });
   }

}
