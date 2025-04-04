using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using dataModels = Dfe.PrepareTransfers.Data.Models;


namespace Dfe.PrepareTransfers.Web.Pages.Projects.TransferDates
{
    public class Index : CommonPageModel
    {
        private readonly IProjects _projectsRepository;
        public string AdvisoryBoardDate { get; set; }
        public string PreviousAdvisoryBoardDate { get; set; }
        public bool? HasAdvisoryBoardDate { get; set; }
        public string TargetDate { get; set; }
        public bool? HasTargetDate { get; set; }
        [BindProperty]
        public MarkSectionCompletedViewModel MarkSectionCompletedViewModel { get; set; }
        public Index(IProjects projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var project = await _projectsRepository.GetByUrn(Urn);
            dataModels.Project projectResult = project.Result;
            IncomingTrustName = projectResult.IncomingTrustName;
            ProjectReference = projectResult.Reference;
            AdvisoryBoardDate = projectResult.Dates?.Htb;
            PreviousAdvisoryBoardDate = projectResult.Dates?.PreviousAdvisoryBoardDate;
            HasAdvisoryBoardDate = projectResult.Dates?.HasHtbDate;
            TargetDate = projectResult.Dates?.Target;
            HasTargetDate = projectResult.Dates?.HasTargetDateForTransfer;
            OutgoingAcademyUrn = projectResult.OutgoingAcademyUrn;
            IsReadOnly = projectResult.IsReadOnly;
            ProjectSentToCompleteDate = projectResult.ProjectSentToCompleteDate;
            MarkSectionCompletedViewModel = new MarkSectionCompletedViewModel
            {
                IsCompleted = projectResult.Dates.IsCompleted ?? false,
                ShowIsCompleted = true
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RepositoryResult<dataModels.Project> project = await _projectsRepository.GetByUrn(Urn);

            var projectResult = project.Result;

            projectResult.Dates.IsCompleted = MarkSectionCompletedViewModel.IsCompleted;

            await _projectsRepository.UpdateDates(projectResult);

            return RedirectToPage(ReturnToPreview ? Links.Project.Index.PageName : "/Projects/Index",
                new { Urn });
        }
    }
}