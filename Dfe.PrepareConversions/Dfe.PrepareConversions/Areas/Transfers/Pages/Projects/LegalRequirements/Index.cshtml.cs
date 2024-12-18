using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.LegalRequirements;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.LegalRequirements
{
    public class Index : CommonPageModel
    {
        private readonly IProjects _projects;
        public LegalRequirementsViewModel LegalRequirementsViewModel { get; private set; }

        [BindProperty]
        public MarkSectionCompletedViewModel MarkSectionCompletedViewModel { get; set; }

        public Index(IProjects projects)
        {
            _projects = projects;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var project = await _projects.GetByUrn(Urn);

            var projectResult = project.Result;
            ProjectReference = projectResult.Reference;
            IsReadOnly = projectResult.IsReadOnly;
            ProjectSentToCompleteDate = projectResult.ProjectSentToCompleteDate;
            LegalRequirementsViewModel = new LegalRequirementsViewModel(
                projectResult.LegalRequirements.IncomingTrustAgreement,
                projectResult.LegalRequirements.DiocesanConsent,
                projectResult.LegalRequirements.OutgoingTrustConsent,
                projectResult.Urn,
                projectResult.IsReadOnly,
                projectResult.ProjectSentToCompleteDate
            );
            MarkSectionCompletedViewModel = new MarkSectionCompletedViewModel
            {
                IsCompleted = projectResult.LegalRequirements.IsCompleted ?? false,
                ShowIsCompleted = LegalRequirementsSectionDataIsPopulated(projectResult)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var project = await _projects.GetByUrn(Urn);

            var projectResult = project.Result;

            projectResult.LegalRequirements.IsCompleted = MarkSectionCompletedViewModel.IsCompleted;

            await _projects.UpdateLegalRequirements(projectResult);

            return RedirectToPage(ReturnToPreview ? Links.HeadteacherBoard.Preview.PageName : Links.Project.Index.PageName,
                new { Urn });
        }

        private static bool LegalRequirementsSectionDataIsPopulated(Project project) =>
            project.LegalRequirements.DiocesanConsent != null
            && project.LegalRequirements.IncomingTrustAgreement != null;


    }
}