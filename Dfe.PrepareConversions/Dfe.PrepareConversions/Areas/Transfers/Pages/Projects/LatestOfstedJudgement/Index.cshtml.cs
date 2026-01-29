using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.LatestOfstedJudgement
{
    public class Index : CommonPageModel
    {
        private readonly IGetInformationForProject _getInformationForProject;
        private readonly IProjects _projectsRepository;
        private readonly IGetEstablishment _getEstablishment;

        public Data.Models.Academies.LatestOfstedJudgement LatestOfstedJudgement { get; set; }

        [BindProperty]
        public AdditionalInformationViewModel AdditionalInformationViewModel { get; set; }

        public EstablishmentDto Establishment { get; set; }

        public bool IsPreview { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AcademyUkprn { get; set; }

        public Index(
            IGetInformationForProject getInformationForProject,
            IProjects projectsRepository,
            IGetEstablishment getEstablishment
        )
        {
            _getInformationForProject = getInformationForProject;
            _projectsRepository = projectsRepository;
            _getEstablishment = getEstablishment;
        }

        public async Task<IActionResult> OnGetAsync(bool addOrEditAdditionalInformation = false)
        {
            var projectInformation = await _getInformationForProject.Execute(Urn);
            var academy = projectInformation.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn);
            LatestOfstedJudgement = academy.LatestOfstedJudgement;

            OutgoingAcademyUrn = projectInformation.Project.OutgoingAcademyUrn;
            ProjectReference = projectInformation.Project.Reference;

            AdditionalInformationViewModel = new AdditionalInformationViewModel
            {
                AdditionalInformation = LatestOfstedJudgement.AdditionalInformation,
                HintText =
                        "If you add comments, they'll be included in the latest Ofsted judgement section of your project template.",
                Urn = projectInformation.Project.Urn,
                AddOrEditAdditionalInformation = addOrEditAdditionalInformation,
                ReturnToPreview = ReturnToPreview,
                HideWarning = true
            };

            Establishment = await _getEstablishment.GetEstablishmentByUkprn(academy.Ukprn);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var model = await _projectsRepository.GetByUrn(Urn);
            var academy = model.Result.TransferringAcademies.First(a => a.OutgoingAcademyUkprn == AcademyUkprn);
            academy.LatestOfstedReportAdditionalInformation = AdditionalInformationViewModel?.AdditionalInformation;
            await _projectsRepository.UpdateAcademy(model.Result.Urn, academy);

            if (ReturnToPreview)
            {
                return RedirectToPage(Links.HeadteacherBoard.Preview.PageName, new { Urn });
            }

            return RedirectToPage("/Projects/LatestOfstedJudgement/Index", null, new { Urn }, "additional-information-hint");
        }


    }
}