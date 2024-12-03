using Dfe.PrepareTransfers.Data;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Microsoft.AspNetCore.Mvc;
using dataModels = Dfe.PrepareTransfers.Data.Models;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.AcademyAndTrustInformation
{
    public class Index : CommonPageModel
    {
        public TransferAcademyAndTrustInformation.RecommendationResult Recommendation { get; set; }
        public string Author { get; set; }
        public string AdvisoryBoardDate { get; set; }
        public string TargetDate { get; set; }

        private readonly IGetInformationForProject _getInformationForProject;
        
        private readonly IProjects _projectsRepository;

        public Index(IGetInformationForProject getInformationForProject, IProjects projectsRepository)
        {
            _getInformationForProject = getInformationForProject;
            _projectsRepository = projectsRepository;
        }

        public async Task<IActionResult> OnGetAsync(string urn)
        {
            var projectInformation = await _getInformationForProject.Execute(urn);
            var project = await _projectsRepository.GetByUrn(Urn);
            dataModels.Project projectResult = project.Result;
            ProjectReference = projectInformation.Project.Reference;
            Recommendation = projectInformation.Project.AcademyAndTrustInformation.Recommendation;
            Author = projectInformation.Project.AcademyAndTrustInformation.Author;
            AdvisoryBoardDate = projectInformation.Project.Dates?.Htb;
            IncomingTrustName = projectInformation.Project.IncomingTrustName;
            TargetDate = projectInformation.Project.Dates?.Target;
            OutgoingAcademyUrn = projectInformation.Project.OutgoingAcademyUrn;
            Urn = projectInformation.Project.Urn;
            IsReadOnly = projectResult.IsReadOnly;
            ProjectSentToCompleteDate = projectResult.ProjectSentToCompleteDate;
            IsFormAMAT = projectInformation.Project.IsFormAMat.HasValue && projectInformation.Project.IsFormAMat.Value == true;
            IncomingTrustReferenceNumber = projectResult.IncomingTrustReferenceNumber;
            
            return Page();
        }
    }
}