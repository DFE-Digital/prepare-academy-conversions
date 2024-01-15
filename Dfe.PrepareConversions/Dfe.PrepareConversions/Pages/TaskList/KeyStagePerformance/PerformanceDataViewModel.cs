using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.KeyStagePerformance
{
   public class PerformanceDataViewModel: BaseAcademyConversionProjectPageModel
   {
      private readonly ErrorService _errorService;

      public PerformanceDataViewModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
      {
         _errorService = errorService;
      }

      public override async Task<IActionResult> OnPostAsync(int id)
      {
         if (YesChecked is true && string.IsNullOrWhiteSpace(ExternalApplicationFormUrl))
         {
            ModelState.AddModelError(nameof(ExternalApplicationFormUrl), "You must enter valid link for the schools application form");
         }

         if (ModelState.IsValid)
         {

            await _repository.SetProjectExternalApplicationForm(id, YesChecked.Value, YesChecked is true ? ExternalApplicationFormUrl : default);


            return RedirectToPage(Links.ExternalApplicationForm.Index.Page, new { id });
         }

         _errorService.AddErrors(ModelState.Keys, ModelState);
         return await base.OnGetAsync(id);
      }
   }
}
