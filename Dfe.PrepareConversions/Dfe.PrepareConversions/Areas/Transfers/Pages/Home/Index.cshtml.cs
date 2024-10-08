using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models.ProjectList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Dfe.PrepareTransfers.Web.Pages.Home
{
    public class Index : PageModel
    {
        private const int PageSize = 10;

        private readonly ILogger<Index> _logger;
        private readonly IProjects _projectsRepository;
        private List<ProjectSearchResult> _projects;

        public IReadOnlyList<ProjectSearchResult> Projects => _projects.AsReadOnly();
        public int TotalProjectCount { get; private set; }

        public int SearchCount { get; private set; }

        protected string NameOfUser => User?.FindFirstValue("name") ?? string.Empty;


        public Index(IProjects projectsRepository, ILogger<Index> logger)
        {
            _projectsRepository = projectsRepository;
            _logger = logger;
        }

        public int StartingPage { get; private set; } = 1;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => TotalProjectCount > CurrentPage * PageSize;
        public int PreviousPage => CurrentPage - 1;
        public int NextPage => CurrentPage + 1;
        public int TotalPages => TotalProjectCount % PageSize == 0
            ? TotalProjectCount / PageSize
            : (TotalProjectCount / PageSize) + 1;

        [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; }
        [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

        [BindProperty] public ProjectListFilters Filters { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

            if (RedirectToReturnUrl(out IActionResult actionResult)) return actionResult;

            var projectSearchModel = new GetProjectSearchModel(CurrentPage, PageSize, Filters.Title?.Trim(), Filters.SelectedOfficers, Filters.SelectedStatuses);

            RepositoryResult<List<ProjectSearchResult>> projects = await _projectsRepository.GetProjects(projectSearchModel);

            _projects = new List<ProjectSearchResult>(projects.Result.Where(r => r.Reference is not null));
            SearchCount = projects.Result.Count;
            TotalProjectCount = projects.TotalRecords;

            if (CurrentPage - 5 > 1) StartingPage = CurrentPage - 5;

            ApiResponse<ProjectFilterParameters> filterParametersResponse = await _projectsRepository.GetFilterParameters();

            if (filterParametersResponse.Success)
            {
                Filters.AvailableStatuses = filterParametersResponse.Body.Statuses.ConvertAll(r => r.ToSentenceCase());
                Filters.AvailableDeliveryOfficers = filterParametersResponse.Body.AssignedUsers.OrderByDescending(o => o.Equals(ProjectListHelper.ConvertToFirstLast(NameOfUser), StringComparison.OrdinalIgnoreCase))
                   .ThenBy(o => o)
                   .ToList();
            }

            _logger.LogInformation("Home page loaded");
            return Page();
        }


        /// <summary>
        ///    If there is a return url, redirects the user to that page after logging in
        /// </summary>
        /// <param name="actionResult">action result to redirect to</param>
        /// <returns>true if redirecting</returns>
        private bool RedirectToReturnUrl(out IActionResult actionResult)
        {
            actionResult = null;
            var decodedUrl = "";
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                decodedUrl = WebUtility.UrlDecode(ReturnUrl);
            }

            if (Url.IsLocalUrl(decodedUrl))
            {
                actionResult = Redirect(ReturnUrl);
                return true;
            }

            return false;
        }

        public async Task<FileStreamResult> OnGetDownload()
        {
            Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

            var projectSearchModel = new GetProjectSearchModel(CurrentPage, PageSize, Filters.Title?.Trim(), Filters.SelectedOfficers, Filters.SelectedStatuses);

            ApiResponse<FileStreamResult> response = await _projectsRepository.DownloadProjectExport(projectSearchModel);

            if (response.Success)
            {
                var result = response.Body;
                result.FileDownloadName = "project_list.xlsx";

                return result;
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
}
