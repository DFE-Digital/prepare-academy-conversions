using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectList;

public class IndexModel : PaginatedPageModel
{
   private readonly IAcademyConversionProjectRepository _repository;

   public IndexModel(IAcademyConversionProjectRepository repository)
   {
      _repository = repository;
   }
   public IEnumerable<BaseFormSection> Sections { get; set; }
   protected override ApiV2PagingInfo Paging { get; set; }
   public IEnumerable<ProjectListViewModel> Projects { get; set; }
   public int ProjectCount => Projects.Count();

   public int TotalProjects { get; set; }

   [BindProperty]
   public ProjectListFilters Filters { get; set; } = new();

   public async Task OnGetAsync()
   {
      Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

      ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>> response =
         await _repository.GetAllProjects(CurrentPage, PageSize, Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions);

      Paging = response.Body?.Paging;
      Projects = response.Body?.Data.Select(ProjectListHelper.Build).ToList();
      TotalProjects = response.Body?.Paging?.RecordCount ?? 0;

      ApiResponse<ProjectFilterParameters> filterParametersResponse = await _repository.GetFilterParameters();

      if (filterParametersResponse.Success)
      {
         Filters.AvailableStatuses = filterParametersResponse.Body.Statuses.ConvertAll(r => r.ToSentenceCase());
         Filters.AvailableDeliveryOfficers = filterParametersResponse.Body.AssignedUsers;
         Filters.AvailableRegions = filterParametersResponse.Body.Regions;
      }
   }
   public async Task<FileStreamResult> OnGetDownload()
   {
      Filters.PersistUsing(TempData).PopulateFrom(Request.Query);
      ApiResponse<FileStreamResult> response = await _repository.DownloadProjectExport(CurrentPage, PageSize, Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions);

      if (response.Success)
      {
         return response.Body;
      }
      else
      {
         var stream = new MemoryStream();
         var writer = new StreamWriter(stream);
         writer.Write("");
         writer.Flush();
         stream.Position = 0;

         var fileStreamResult = new FileStreamResult(stream, "text/csv")
         {
            FileDownloadName = "empty.csv"
         };

         return fileStreamResult;
      }
   }

}
