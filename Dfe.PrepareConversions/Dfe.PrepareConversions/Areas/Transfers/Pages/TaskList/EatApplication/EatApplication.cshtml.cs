using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using GovUK.Dfe.ExternalApplications.Api.Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.EatApplication
{
    [IgnoreAntiforgeryToken]
    public class EatApplication : CommonPageModel
    {
        private readonly ILogger<EatApplication> _logger;
        private readonly IProjects _projectsRepository;
        private readonly IApplicationsClient _applicationsClient;
        private readonly IDistributedCache _distributedCache;

        public Project Project { get; set; }
        public string HtmlContent { get; set; }
        public bool IsLoading { get; set; }

        [BindProperty]
        public string FileId { get; set; }

        [BindProperty]
        public string ApplicationId { get; set; }

        public EatApplication(IProjects projectsRepository, ILogger<EatApplication> logger, IApplicationsClient applicationsClient, IDistributedCache distributedCache)
        {
            _projectsRepository = projectsRepository;
            _logger = logger;
            _applicationsClient = applicationsClient;
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> OnGetAsync(bool fetch = false)
        {
            Project = (await _projectsRepository.GetByUrn(Urn)).Result;
            
            // Ensure this page is only accessible for TRF- projects
            if (!Project.Reference.StartsWith("TRF-"))
            {
                return NotFound();
            }

            // Create cache key based on project reference
            var cacheKey = $"EatApplication_Html_{Project.Reference}";
            
            // Check if HTML content exists in cache
            var cachedHtml = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrWhiteSpace(cachedHtml))
            {
                _logger.LogInformation("Retrieved EAT Application HTML from cache for project {ProjectReference}", Project.Reference);
                HtmlContent = cachedHtml;
                // Forms are already fixed in cached content, but ensure they are correct
                HtmlContent = FixFormActions(HtmlContent, Urn);
                IsLoading = false;
                return Page();
            }

            // If fetch parameter is true, fetch the content now
            if (fetch)
            {
                // Fetch from API
                var fileResponse = await _applicationsClient.DownloadApplicationPreviewHtmlAsync(Project.Reference);

                // Read the HTML content from the FileResponse stream
                if (fileResponse != null && fileResponse.Stream != null)
                {
                    using (var reader = new StreamReader(fileResponse.Stream))
                    {
                        HtmlContent = await reader.ReadToEndAsync();
                    }
                }
                else
                {
                    HtmlContent = "<p class='govuk-body'>No application data available.</p>";
                }

                // Fix form actions to point to the correct handler
                HtmlContent = FixFormActions(HtmlContent, Urn);

                // Store in cache with 1-hour expiration
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                await _distributedCache.SetStringAsync(cacheKey, HtmlContent, cacheOptions);
                _logger.LogInformation("Cached EAT Application HTML for project {ProjectReference} with 1-hour expiration", Project.Reference);
                
                IsLoading = false;
                return Page();
            }

            // Show loading page that will redirect to fetch
            IsLoading = true;
            return Page();
        }

        public async Task<IActionResult> OnPostDownloadEatFileAsync()
        {
            if (string.IsNullOrEmpty(FileId) || string.IsNullOrEmpty(ApplicationId))
            {
                _logger.LogWarning("File download attempted with missing FileId or ApplicationId");
                return BadRequest("FileId and ApplicationId are required");
            }

            try
            {
                _logger.LogInformation("Downloading file {FileId} for application {ApplicationId}", FileId, ApplicationId);

                // Download the file from the external applications API
                var fileResponse = await _applicationsClient.DownloadFileAsync(new Guid(FileId), new Guid(ApplicationId), CancellationToken.None);

                if (fileResponse == null || fileResponse.Stream == null)
                {
                    _logger.LogWarning("File download returned null response for FileId {FileId}", FileId);
                    return NotFound("File not found");
                }

                var contentType = fileResponse.Headers.TryGetValue("Content-Type", out var ct)
                   ? ct.FirstOrDefault()
                   : "application/octet-stream";

            // Get the filename from the response, or use a default
            string fileName = "downloadedfile";
               if (fileResponse.Headers.TryGetValue("Content-Disposition", out var cd))
               {
                  var disposition = cd.FirstOrDefault();
                  if (!string.IsNullOrEmpty(disposition))
                  {
                     var fileNameMatch = System.Text.RegularExpressions.Regex.Match(
                        disposition,
                        @"filename\*=UTF-8''(?<fileName>.+)|filename=""?(?<fileName>[^\"";]+)""?"
                     );
                     if (fileNameMatch.Success)
                        fileName = System.Net.WebUtility.UrlDecode(fileNameMatch.Groups["fileName"].Value);
                  }
               }


            // Return the file to the browser for download
            return File(fileResponse.Stream, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file {FileId} for application {ApplicationId}", FileId, ApplicationId);
                return StatusCode(500, "An error occurred while downloading the file");
            }
        }

        private string FixFormActions(string htmlContent, string urn)
        {
            // Replace the form action "DownloadEatFile" with the correct handler path
            var correctAction = $"/transfers/project/{urn}/eat-application?handler=DownloadEatFile";
            
            // Replace the form action attribute
            htmlContent = System.Text.RegularExpressions.Regex.Replace(
                htmlContent,
                @"action=""DownloadEatFile""",
                $@"action=""{correctAction}""",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            return htmlContent;
        }
    }
}

