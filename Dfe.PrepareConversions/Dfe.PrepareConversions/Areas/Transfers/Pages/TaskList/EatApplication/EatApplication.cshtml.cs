using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using GovUK.Dfe.ExternalApplications.Api.Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.EatApplication
{
    public class EatApplication : CommonPageModel
    {
        private readonly ILogger<EatApplication> _logger;
        private readonly IProjects _projectsRepository;
        private readonly IApplicationsClient _applicationsClient;
        private readonly IDistributedCache _distributedCache;

        public Project Project { get; set; }
        public string HtmlContent { get; set; }
        public bool IsLoading { get; set; }

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
    }
}

