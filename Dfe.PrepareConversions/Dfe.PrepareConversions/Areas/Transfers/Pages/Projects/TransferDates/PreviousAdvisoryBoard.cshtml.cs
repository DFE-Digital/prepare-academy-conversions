using Dfe.PrepareConversions.Areas.Transfers.Validators.TransferDates;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.TransferDates;
using Dfe.PrepareTransfers.Web.Transfers.Validators.TransferDates;

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.TransferDates
{
    public class PreviousAdvisoryBoard : CommonPageModel
    {
        private readonly IProjects _projectsRepository;

        [BindProperty] public PreviouslyConsideredViewModel PreviouslyConsideredViewModel { get; set; }
        public string TrustName { get; set; }

        public PreviousAdvisoryBoard(IProjects projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var project = await _projectsRepository.GetByUrn(Urn);

            var projectResult = project.Result;
            TrustName = projectResult.OutgoingTrustName;
            PreviouslyConsideredViewModel = new PreviouslyConsideredViewModel
            {
               PreviouslyConsideredDate = new DateViewModel
                {
                    Date = DateViewModel.SplitDateIntoDayMonthYear(projectResult.Dates.PreviouslyConsideredDate),
                    UnknownDate = projectResult.Dates.HasHtbDate is false
                }
            };
            IncomingTrustName = projectResult.IncomingTrustName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var project = await _projectsRepository.GetByUrn(Urn);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var projectResult = project.Result;

            var validationContext = new ValidationContext<PreviouslyConsideredViewModel>(PreviouslyConsideredViewModel)
            {
                RootContextData =
                {
                    ["TargetDate"] = projectResult.Dates.Target
                }
            };
            var validator = new PreviouslyConsideredDateValidator();
            var validationResult = await validator.ValidateAsync(validationContext);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, nameof(PreviouslyConsideredViewModel));
                return Page();
            }

            projectResult.Dates.PreviouslyConsideredDate = PreviouslyConsideredViewModel.PreviouslyConsideredDate.DateInputAsString();

            await _projectsRepository.UpdateDates(projectResult);

            if (ReturnToPreview)
            {
                return RedirectToPage(Links.HeadteacherBoard.Preview.PageName, new { Urn });
            }

            return RedirectToPage("/Projects/TransferDates/Index", new { Urn });
        }
    }
}