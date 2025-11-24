using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.EatApplication
{
    public class EatApplication : CommonPageModel
    {
        private readonly ILogger<EatApplication> _logger;
        private readonly IProjects _projectsRepository;

        public Project Project { get; set; }

        public EatApplication(IProjects projectsRepository, ILogger<EatApplication> logger)
        {
            _projectsRepository = projectsRepository;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Project = (await _projectsRepository.GetByUrn(Urn)).Result;
            
            // Ensure this page is only accessible for TRF- projects
            if (!Project.Reference.StartsWith("TRF-"))
            {
                return NotFound();
            }

            return Page();
        }
    }
}

