using AngleSharp.Io.Dom;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace Dfe.PrepareConversions.Pages.ProjectDocuments;

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

      //https://educationgovuk.sharepoint.com/sites/sip-dev/sip_application/Forms/AllItems.aspx?id=%2Fsites%2Fsip%2Ddev%2Fsip%5Fapplication%2FA2B%5F0027%5F63B137B6D21DEB11A813000D3A38AA47

      ApplicationLevelDocumentsFolder = $"{rootSharePointFolder}sip_application/{Project.ApplicationReferenceNumber}_{Project.ApplicationSharePointId.Value.ToString("N").ToUpper()}";
      SchoolLevelDocumentsFolder = $"{rootSharePointFolder}sip_applyingschools/{Project.ApplicationReferenceNumber}_{Project.SchoolSharePointId.Value.ToString("N").ToUpper()}";

      return Page();
   }
}
