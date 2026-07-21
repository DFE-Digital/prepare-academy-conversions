using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Dfe.PrepareConversions.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Models;

namespace Dfe.PrepareConversions.Pages.SignificantChange.ProjectList;

public class IndexModel(ISignificantChangeProjectRepository significantChangeProjectRepository) : PaginatedPageModel
{
   public ISignificantChangeProjectRepository SignificantChangeProjectRepository { get; } = significantChangeProjectRepository;
   protected override ApiV2PagingInfo Paging { get; set; }
   public List<SignificantChangeProjectListViewModel> Projects { get; private set; } = [];

   public int ProjectCount => Projects.Count;

   public int TotalProjects => Paging?.RecordCount ?? ProjectCount;
   
   public async Task OnGetAsync()
   {
      PagePath = Links.SignificantChange.ListView.Page;

      var response = await SignificantChangeProjectRepository.GetAllProjects(CurrentPage, PageSize);
      Paging = response.Body?.Paging ?? new ApiV2PagingInfo { Page = CurrentPage, RecordCount = 0, NextPageUrl = null };
      Projects = response.Body?.Data?.Select(SignificantChangeProjectListHelper.Build).ToList() ?? [];
   }
}
