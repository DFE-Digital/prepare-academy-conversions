using AngleSharp.Io.Dom;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Dfe.PrepareConversions.Pages.ApplicationDocuments;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   private readonly IConfiguration _configuration;

   public string ReturnPage { get; set; }
   public string ReturnId { get; set; }

   public IndexModel(IAcademyConversionProjectRepository repository, IConfiguration configuration) : base(repository)
   {
      _configuration = configuration;
   }

   [BindProperty]
   public string ApplicationLevelDocumentsFolder { get; set; }
   [BindProperty]
   public string SchoolLevelDocumentsFolder { get; private set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      var rootSharePointFolder = _configuration["Sharepoint:Url"];

      ApplicationLevelDocumentsFolder = $"{rootSharePointFolder}sip_application/{Project.ApplicationReferenceNumber}_{Project.ApplicationSharePointId.Value.ToString("N").ToUpper()}";
      SchoolLevelDocumentsFolder = $"{rootSharePointFolder}sip_applyingschools/{Project.ApplicationReferenceNumber}_{Project.SchoolSharePointId.Value.ToString("N").ToUpper()}";

      return Page();
   }
}
