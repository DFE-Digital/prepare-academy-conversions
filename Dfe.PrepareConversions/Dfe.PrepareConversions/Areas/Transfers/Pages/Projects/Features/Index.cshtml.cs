﻿using Dfe.PrepareTransfers.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.Features
{
    public class Index : CommonPageModel
    {
        private readonly IProjects _projects;

        public TransferFeatures.ReasonForTheTransferTypes ReasonForTheTransfer { get; set; }
        public List<TransferFeatures.SpecificReasonForTheTransferTypes> SpecificReasonForTheTransfer { get; set; }
        public TransferFeatures.TransferTypes TypeOfTransfer { get; set; }
        public string OtherTypeOfTransfer { get; set; }
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
            TypeOfTransfer = projectResult.Features.TypeOfTransfer;
            OtherTypeOfTransfer = projectResult.Features.OtherTypeOfTransfer;
            OutgoingAcademyUrn = projectResult.OutgoingAcademyUrn;
            ReasonForTheTransfer = projectResult.Features.ReasonForTheTransfer;
            SpecificReasonForTheTransfer = projectResult.Features.SpecificReasonsForTheTransfer;
            IsReadOnly = projectResult.IsReadOnly;
            ProjectSentToCompleteDate = projectResult.ProjectSentToCompleteDate;
            MarkSectionCompletedViewModel = new MarkSectionCompletedViewModel
            {
                IsCompleted = projectResult.Features.IsCompleted ?? false,
                ShowIsCompleted = FeaturesSectionDataIsPopulated(projectResult)
            };
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var project = await _projects.GetByUrn(Urn);
            
            var projectResult = project.Result;
            
            projectResult.Features.IsCompleted = MarkSectionCompletedViewModel.IsCompleted;

            await _projects.UpdateFeatures(projectResult);

            return RedirectToPage(ReturnToPreview ? Links.HeadteacherBoard.Preview.PageName : "/Projects/Index", new {Urn});
        }

        private static bool FeaturesSectionDataIsPopulated(Project project) =>
            project.Features.ReasonForTheTransfer != TransferFeatures.ReasonForTheTransferTypes.Empty &&
            project.Features.TypeOfTransfer != TransferFeatures.TransferTypes.Empty;
    }
}