using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.ProjectDates;

public class ConversionDateHistoryPageModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public IEnumerable<OpeningDateHistoryDto> History { get; set; }


   public ConversionDateHistoryPageModel(IAcademyConversionProjectRepository repository,
                       ErrorService errorService)
      : base(repository)
   {
      _errorService = errorService;
   }


   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      History = (await _repository.GetOpeningDateHistoryForConversionProject(id)).Body;

      return Page();
   }
}
