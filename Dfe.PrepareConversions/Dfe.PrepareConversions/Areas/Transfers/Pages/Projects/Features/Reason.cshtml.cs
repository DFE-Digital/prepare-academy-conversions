﻿using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Transfers.Validators.Features;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.Projects;
using FluentValidation.AspNetCore;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace Dfe.PrepareTransfers.Web.Pages.Projects.Features
{
    public class Reason : CommonPageModel
    {
        private readonly IProjects _projects;

        public Reason(IProjects projects)
        {
            _projects = projects;
        }

        [BindProperty]
        public TransferFeatures.ReasonForTheTransferTypes ReasonForTheTransfer { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var project = await _projects.GetByUrn(Urn);

            var projectResult = project.Result;
            IncomingTrustName = projectResult.IncomingTrustName;
            ReasonForTheTransfer = projectResult.Features.ReasonForTheTransfer;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var project = await _projects.GetByUrn(Urn);

            var validator = new FeaturesReasonValidator();
            var validationResults = await validator.ValidateAsync(this);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            project.Result.Features.ReasonForTheTransfer = ReasonForTheTransfer;
            // reset this value so old ones that don't match are not left
            project.Result.Features.SpecificReasonsForTheTransfer = new List<TransferFeatures.SpecificReasonForTheTransferTypes>();

            await _projects.UpdateFeatures(project.Result);

            if (ReturnToPreview)
            {
                return RedirectToPage(Links.HeadteacherBoard.Preview.PageName, new { Urn });
            }

            return RedirectToPage("/Projects/Features/SpecificReason", new { Urn });

        }

        public static List<RadioButtonViewModel> ReasonRadioButtons(TransferFeatures.ReasonForTheTransferTypes selected)
        {
            var values =
                EnumHelpers<TransferFeatures.ReasonForTheTransferTypes>.GetDisplayableValues(TransferFeatures.ReasonForTheTransferTypes
                    .Empty);

            var result = values.Select(value => new RadioButtonViewModel
            {
                Value = value.ToString(),
                Name = nameof(ReasonForTheTransfer),
                DisplayName = EnumHelpers<TransferFeatures.ReasonForTheTransferTypes>.GetDisplayValue(value),
                Checked = selected == value
            }).ToList();

            return result;
        }
    }
}