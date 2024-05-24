using AngleSharp.Io.Dom;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Services.Helpers;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectDocuments;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   private readonly IFileService _fileService;

   public string ReturnPage { get; set; }
   public string ReturnId { get; set; }

   public IndexModel(IAcademyConversionProjectRepository repository, IFileService fileService) : base(repository)
   {
      _fileService = fileService;
   }

   [BindProperty]
   public List<string> DioceseFileNames { get; set; }
   [BindProperty]
   public List<string> ResolutionConsentFileNames { get; private set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      DioceseFileNames = await _fileService.GetFiles(FileUploadConstants.TopLevelSchoolFolderName, Project.SharePointId.ToString(), Project.ApplicationReferenceNumber, FileUploadConstants.DioceseFilePrefixFieldName);
      //TempDataHelper.StoreSerialisedValue($"{EntityId}-dioceseFiles", TempData, DioceseFileNames);
      //FoundationConsentFileNames = await _fileUploadService.GetFiles(FileUploadConstants.TopLevelSchoolFolderName, EntityId.ToString(), ApplicationReference, FileUploadConstants.FoundationConsentFilePrefixFieldName);

      ResolutionConsentFileNames = await _fileService.GetFiles(FileUploadConstants.TopLevelSchoolFolderName, Project.SharePointId.ToString(), Project.ApplicationReferenceNumber, FileUploadConstants.ResolutionConsentfilePrefixFieldName);

      return Page();
   }

   public async Task<HttpResponseMessage> GetFile(string entityName, string recordId, string recordName, string fieldName, string fileName)
   {
      //if (String.IsNullOrEmpty(id))
      //   return Request.CreateResponse(HttpStatusCode.BadRequest);

      var file  = await _fileService.DownloadFile(entityName, recordId, recordName, fieldName, fileName);

      HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

      response.Content = new StreamContent(GenerateStreamFromString(file));
      response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
      response.Content.Headers.ContentDisposition.FileName = fileName;
      //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

      return response;
   }

   public static Stream GenerateStreamFromString(string s)
   {
      var stream = new MemoryStream();
      var writer = new StreamWriter(stream);
      writer.Write(s);
      writer.Flush();
      stream.Position = 0;
      return stream;
   }
}
