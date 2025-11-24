using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using GovUK.Dfe.ExternalApplications.Api.Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.EatApplication
{
    public class EatApplication : CommonPageModel
    {
        private readonly ILogger<EatApplication> _logger;
        private readonly IProjects _projectsRepository;
        private readonly IApplicationsClient _applicationsClient;

        public Project Project { get; set; }
        public string HtmlContent { get; set; }

        public EatApplication(IProjects projectsRepository, ILogger<EatApplication> logger, IApplicationsClient applicationsClient)
        {
            _projectsRepository = projectsRepository;
            _logger = logger;
            _applicationsClient = applicationsClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Project = (await _projectsRepository.GetByUrn(Urn)).Result;
            
            // Ensure this page is only accessible for TRF- projects
            if (!Project.Reference.StartsWith("TRF-"))
            {
                return NotFound();
            }

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

            return Page();
        }
    }
}

